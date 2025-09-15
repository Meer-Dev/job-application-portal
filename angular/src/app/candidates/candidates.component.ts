import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
  CandidateServiceProxy,
  CandidateDto,
  CandidateDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { CreateCandidateDialogComponent } from './create-candidate/create-candidate-dialog.component';
import { EditCandidateDialogComponent } from './edit-candidate/edit-candidate-dialog.component';

class PagedCandidatesRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: './candidates.component.html',
  animations: [appModuleAnimation()]
})
export class CandidatesComponent extends PagedListingComponentBase<CandidateDto> {
  candidates: CandidateDto[] = [];
  keyword = '';
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _candidateService: CandidateServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createCandidate(): void {
    this.showCreateOrEditCandidateDialog();
  }

  editCandidate(candidate: CandidateDto): void {
    this.showCreateOrEditCandidateDialog(candidate.id);
  }

  clearFilters(): void {
    this.keyword = '';
    this.getDataPage(1);
  }

  downloadResume(resumePath: string): void {
    if (resumePath) {
      window.open(resumePath, '_blank');
    }
  }

  protected list(
    request: PagedCandidatesRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._candidateService
      .getAll(
        request.keyword,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: CandidateDtoPagedResultDto) => {
        this.candidates = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(candidate: CandidateDto): void {
    abp.message.confirm(
      `Delete candidate '${candidate.name}'?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._candidateService.delete(candidate.id).subscribe(() => {
            abp.notify.success('Successfully deleted');
            this.refresh();
          });
        }
      }
    );
  }

  private showCreateOrEditCandidateDialog(id?: number): void {
    let createOrEditCandidateDialog: BsModalRef;
    if (!id) {
      createOrEditCandidateDialog = this._modalService.show(
        CreateCandidateDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditCandidateDialog = this._modalService.show(
        EditCandidateDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditCandidateDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
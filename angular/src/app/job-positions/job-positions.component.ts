import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
  JobPositionServiceProxy,
  JobPositionDto,
  JobPositionDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { CreateJobPositionDialogComponent } from './create-job-position/create-job-position-dialog.component';
import { EditJobPositionDialogComponent } from './edit-job-position/edit-job-position-dialog.component';

class PagedJobPositionsRequestDto extends PagedRequestDto {
  keyword: string;
  isActive: boolean | null;
}

@Component({
  templateUrl: './job-positions.component.html',
  animations: [appModuleAnimation()]
})
export class JobPositionsComponent extends PagedListingComponentBase<JobPositionDto> {
  jobPositions: JobPositionDto[] = [];
  keyword = '';
  isActive: boolean | null;
  advancedFiltersVisible = false;

  constructor(
    injector: Injector,
    private _jobPositionService: JobPositionServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  createJobPosition(): void {
    this.showCreateOrEditJobPositionDialog();
  }

  editJobPosition(jobPosition: JobPositionDto): void {
    this.showCreateOrEditJobPositionDialog(jobPosition.id);
  }

  clearFilters(): void {
    this.keyword = '';
    this.isActive = undefined;
    this.getDataPage(1);
  }

  protected list(
    request: PagedJobPositionsRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;
    request.isActive = this.isActive;

    this._jobPositionService.getAll(
  request.keyword,
  request.skipCount,
  request.maxResultCount
)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: JobPositionDtoPagedResultDto) => {
        this.jobPositions = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(jobPosition: JobPositionDto): void {
    abp.message.confirm(
      this.l('JobPositionDeleteWarningMessage', jobPosition.title),
      undefined,
      (result: boolean) => {
        if (result) {
          this._jobPositionService.delete(jobPosition.id).subscribe(() => {
            abp.notify.success(this.l('SuccessfullyDeleted'));
            this.refresh();
          });
        }
      }
    );
  }

  private showCreateOrEditJobPositionDialog(id?: number): void {
    let createOrEditJobPositionDialog: BsModalRef;
    if (!id) {
      createOrEditJobPositionDialog = this._modalService.show(
        CreateJobPositionDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditJobPositionDialog = this._modalService.show(
        EditJobPositionDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditJobPositionDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
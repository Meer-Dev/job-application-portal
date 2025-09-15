import { Component, Injector, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CandidateServiceProxy,
  CandidateDto,
  UpdateCandidateDto,
  JobPositionServiceProxy,
  JobPositionDto,
  JobPositionDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-candidate-dialog.component.html'
})
export class EditCandidateDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  candidate: UpdateCandidateDto = new UpdateCandidateDto();
  jobPositions: JobPositionDto[] = [];
  selectedFile: File = null;
  id: number;

  @Output() onSave = new EventEmitter<any>();
candidateForm: any;

  constructor(
    injector: Injector,
    public _candidateService: CandidateServiceProxy,
    public _jobPositionService: JobPositionServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loadJobPositions();
    this.loadCandidate();
  }

  loadJobPositions(): void {
    this._jobPositionService
      .getAll(undefined, true, 0, 1000)
      .subscribe((result: JobPositionDtoPagedResultDto) => {
        this.jobPositions = result.items;
      });
  }

  loadCandidate(): void {
    this._candidateService.get(this.id).subscribe((result: CandidateDto) => {
      this.candidate.id = result.id;
      this.candidate.name = result.name;
      this.candidate.email = result.email;
      this.candidate.phone = result.phone;
      this.candidate.jobPositionId = result.jobPositionId;
      this.candidate.resumePath = result.resumePath;
    });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      const allowedTypes = ['.pdf', '.docx', '.jpg', '.jpeg', '.png'];
      const fileExtension = file.name.toLowerCase().substr(file.name.lastIndexOf('.'));
      
      if (allowedTypes.includes(fileExtension)) {
        this.selectedFile = file;
        this.candidate.resumePath = file.name;
      } else {
        abp.message.warn('Please select a valid file (.pdf, .docx, .jpg, .png)');
        event.target.value = '';
      }
    }
  }

  save(): void {
    this.saving = true;

    this._candidateService.update(this.candidate).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
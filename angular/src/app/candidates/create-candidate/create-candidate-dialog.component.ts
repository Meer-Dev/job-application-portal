import { Component, Injector, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CandidateServiceProxy,
  CreateCandidateDto,
  JobPositionServiceProxy,
  JobPositionDto,
  JobPositionDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-candidate-dialog.component.html'
})
export class CreateCandidateDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  candidate: CreateCandidateDto = new CreateCandidateDto();
  jobPositions: JobPositionDto[] = [];
  selectedFile: File = null;

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
  }

  loadJobPositions(): void {
    this._jobPositionService.getAll(undefined, 0, 1000) // Get active job positions
      .subscribe((result: JobPositionDtoPagedResultDto) => {
        this.jobPositions = result.items.slice(0, 1000);
      });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      const allowedTypes = ['.pdf', '.docx', '.jpg', '.jpeg', '.png'];
      const fileExtension = file.name.toLowerCase().substr(file.name.lastIndexOf('.'));
      
      if (allowedTypes.includes(fileExtension)) {
        this.selectedFile = file;
        // In a real implementation, you would upload to server here
        // For now, just store the filename
        this.candidate.resumePath = file.name;
      } else {
        abp.message.warn('Please select a valid file (.pdf, .docx, .jpg, .png)');
        event.target.value = '';
      }
    }
  }

  save(): void {
    this.saving = true;

    // In a real implementation, you would first upload the file
    // then save the candidate with the file path

    this._candidateService.create(this.candidate).subscribe(
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
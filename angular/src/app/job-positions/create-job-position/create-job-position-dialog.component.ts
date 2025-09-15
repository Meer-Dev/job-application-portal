import { Component, Injector, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  JobPositionServiceProxy,
  CreateJobPositionDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-job-position-dialog.component.html'
})
export class CreateJobPositionDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  jobPosition: CreateJobPositionDto = new CreateJobPositionDto();

  @Output() onSave = new EventEmitter<any>();
jobPositionForm: any;

  constructor(
    injector: Injector,
    public _jobPositionService: JobPositionServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.jobPosition.isActive = true;
  }

  save(): void {
    this.saving = true;

    this._jobPositionService.create(this.jobPosition).subscribe(
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
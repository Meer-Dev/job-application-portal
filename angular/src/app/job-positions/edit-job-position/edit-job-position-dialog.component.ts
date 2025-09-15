import { Component, Injector, OnInit, EventEmitter, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  JobPositionServiceProxy,
  JobPositionDto,
  UpdateJobPositionDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-job-position-dialog.component.html'
})
export class EditJobPositionDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  jobPosition: UpdateJobPositionDto = new UpdateJobPositionDto();
  id: number;

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
    this._jobPositionService.get(this.id).subscribe((result: JobPositionDto) => {
      this.jobPosition.id = result.id;
      this.jobPosition.title = result.title;
      this.jobPosition.description = result.description;
      this.jobPosition.location = result.location;
      this.jobPosition.isActive = result.isActive;
    });
  }

  save(): void {
    this.saving = true;

    this._jobPositionService.update(this.jobPosition).subscribe(
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
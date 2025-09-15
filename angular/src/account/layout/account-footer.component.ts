import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'account-footer',
  templateUrl: './account-footer.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AccountFooterComponent extends AppComponentBase {
  currentYear: number;
  versionText: string;

  constructor(injector: Injector) {
  super(injector);

  this.currentYear = new Date().getFullYear();
  
  // Initialize with default, update when session loads
  this.versionText = '1.0.0';
  
  // Update when session is available
  if (this.appSession && this.appSession.application) {
    try {
      this.versionText =
        this.appSession.application.version +
        ' [' +
        this.appSession.application.releaseDate.format('YYYYDDMM') +
        ']';
    } catch (error) {
      console.warn('Could not load application version info');
      this.versionText = '1.0.0 [' + this.currentYear + '0101]';
    }
  }
}
}

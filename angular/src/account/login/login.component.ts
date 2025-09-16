import { Component, Injector } from '@angular/core';
import { AbpSessionService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  templateUrl: './login.component.html',
  animations: [accountModuleAnimation()]
})
export class LoginComponent extends AppComponentBase {
  submitting = false;

  constructor(
    injector: Injector,
    public authService: AppAuthService,
    private _sessionService: AbpSessionService
  ) {
    super(injector);
  }

  get multiTenancySideIsTeanant(): boolean {
    return this._sessionService.tenantId > 0;
  }

  get isSelfRegistrationAllowed(): boolean {
    if (!this._sessionService.tenantId) {
      return false;
    }

    return true;
  }

  login(): void {
    console.log('Login form values:', {
        username: this.authService.authenticateModel.userNameOrEmailAddress,
        password: this.authService.authenticateModel.password ? '***' : 'empty',
        rememberMe: this.authService.rememberMe
    });

    if (!this.authService.authenticateModel.userNameOrEmailAddress || 
        !this.authService.authenticateModel.password) {
        abp.message.error('Please enter username and password');
        return;
    }

    this.submitting = true;
    abp.multiTenancy.setTenantIdCookie(null);
    this.authService.authenticate(() => (this.submitting = false));
}
}

import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { TokenService, LogService, UtilsService } from 'abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import {
  AuthenticateModel,
  AuthenticateResultModel,
  TokenAuthServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';

@Injectable()
export class AppAuthService {
  authenticateModel: AuthenticateModel;
  authenticateResult: AuthenticateResultModel;
  rememberMe: boolean;

  constructor(
    private _tokenAuthService: TokenAuthServiceProxy,
    private _router: Router,
    private _utilsService: UtilsService,
    private _tokenService: TokenService,
    private _logService: LogService,
    private _appSessionService: AppSessionService
  ) {
    this.clear();
  }

  logout(reload?: boolean): void {
    abp.auth.clearToken();
    abp.utils.deleteCookie(AppConsts.authorization.encryptedAuthTokenName);
    if (reload !== false) {
      location.href = AppConsts.appBaseUrl;
    }
  }

  authenticate(finallyCallback?: () => void): void {
    finallyCallback = finallyCallback || (() => {});
    this._tokenAuthService
      .authenticate(this.authenticateModel)
      .pipe(finalize(() => finallyCallback()))
      .subscribe({
        next: (result: AuthenticateResultModel) => {
          this.processAuthenticateResult(result);
        },
        error: () => {
          this._logService.error('Authentication failed');
          abp.message.error('Login failed. Please check your credentials.');
          this._router.navigate(['/account/login']);
        },
      });
  }

  private processAuthenticateResult(authenticateResult: AuthenticateResultModel) {
    this.authenticateResult = authenticateResult;
    if (authenticateResult.accessToken) {
      this.login(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        this.rememberMe
      );
    } else {
      this._logService.warn('Authentication failed - no access token received');
      abp.message.error('Login failed. Please check your credentials.');
      this._router.navigate(['/account/login']);
    }
  }

  private async login(
    accessToken: string,
    encryptedAccessToken: string,
    expireInSeconds: number,
    rememberMe?: boolean
  ): Promise<void> {
    const tokenExpireDate = rememberMe
      ? new Date(new Date().getTime() + 1000 * expireInSeconds)
      : undefined;

    // Set the JWT only via ABP's TokenService (this updates ABP internals).
    // Do NOT call abp.auth.setToken to avoid the 0-1 args typings mismatch.
    this._tokenService.setToken(accessToken);

    // Option 1: Use abp.utils.setCookieValue directly (if available)
    if (abp.utils && abp.utils.setCookieValue) {
      abp.utils.setCookieValue(
        AppConsts.authorization.encryptedAuthTokenName,
        encryptedAccessToken,
        tokenExpireDate,
        abp.appPath
      );
    } else {
      // Option 2: Use browser's document.cookie directly
      let cookieString = `${AppConsts.authorization.encryptedAuthTokenName}=${encryptedAccessToken}; path=${abp.appPath || '/'}`;
      if (tokenExpireDate) {
        cookieString += `; expires=${tokenExpireDate.toUTCString()}`;
      }
      document.cookie = cookieString;
    }

    // Warm up session (user/tenant) once token is in place
    try {
      await this._appSessionService.init();
    } catch (e) {
      this._logService.error(`Session init failed after login: ${e}`);
      this._router.navigate(['/account/login']);
      return;
    }

    // Navigate into the app shell
    let initialUrl = UrlHelper.initialUrl;
    if (initialUrl.indexOf('/login') > 0) {
      initialUrl = AppConsts.appBaseUrl;
    }
    this._router.navigate(['/app/home']);
  }

  private clear(): void {
    this.authenticateModel = new AuthenticateModel();
    this.authenticateModel.rememberClient = false;
    this.authenticateResult = null;
    this.rememberMe = false;
  }
}
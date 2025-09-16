import { Injectable } from '@angular/core';
import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '../session/app-session.service';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';

@Injectable({ providedIn: 'root' }) // <-- IMPORTANT: provide the guard
export class AppRouteGuard implements CanActivate, CanActivateChild {
  private triedInitOnce = false;

  constructor(
    private _permissionChecker: PermissionCheckerService,
    private _router: Router,
    private _sessionService: AppSessionService
  ) {}

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    if (!this._sessionService.user && !this.triedInitOnce) {
      this.triedInitOnce = true;
      try {
        await this._sessionService.init();
      } catch {
        // ignore init error; we'll fall back to login redirect below
      }
    }

    if (!this._sessionService.user) {
      this._router.navigate(['/account/login']);
      return false;
    }

    // no permission specified => allow
    if (!route.data || !route.data['permission']) {
      return true;
    }

    // permission specified => check
    if (this._permissionChecker.isGranted(route.data['permission'])) {
      return true;
    }

    this._router.navigate([this.selectBestRoute()]);
    return false;
  }

  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> | boolean {
    return this.canActivate(route, state);
  }

  selectBestRoute(): string {
    if (!this._sessionService.user) {
      return '/account/login';
    }

    // Make sure this matches your actual route path
    if (this._permissionChecker.isGranted('Pages.Users')) {
      return '/app/users';
    }

    return '/app/home';
  }
}

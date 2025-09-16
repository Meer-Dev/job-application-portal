import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { JobPositionsComponent } from './job-positions/job-positions.component';
import { CandidatesComponent } from './candidates/candidates.component';

@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: AppComponent,
        children: [
          { path: '', pathMatch: 'full', redirectTo: 'home' },
          { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
          { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
          { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
          { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
          { path: 'about', component: AboutComponent, canActivate: [AppRouteGuard] },
          { path: 'update-password', component: ChangePasswordComponent, canActivate: [AppRouteGuard] },
          { path: 'job-positions', component: JobPositionsComponent, data: { permission: 'Pages.JobPositions' }, canActivate: [AppRouteGuard] },
          { path: 'candidates', component: CandidatesComponent, data: { permission: 'Pages.Candidates' }, canActivate: [AppRouteGuard] },
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/app/about', pathMatch: 'full' },
  {
    path: 'account',
    loadChildren: () => import('account/account.module').then((m) => m.AccountModule),
    data: { preload: true },
  },
  {
    path: 'app',
    loadChildren: () => import('app/app.module').then((m) => m.AppModule),
    data: { preload: true },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
  providers: [],
})
export class RootRoutingModule {}

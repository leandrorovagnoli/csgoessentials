import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';
import { MainLayoutComponent } from './_layout/main-layout/main-layout.component';

const routes: Routes = [
{
    path: '',
    component: MainLayoutComponent,
  loadChildren: () =>
  import('./pages/modules/video.module').then( (m) => m.VideoModule)
},
{
  path: 'admin', component: AdminLoginComponent
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

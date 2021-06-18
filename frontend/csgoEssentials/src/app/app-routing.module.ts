import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminComponent } from './pages/modules/Admin/component/admin.component';

import { MainLayoutComponent } from './_layout/main-layout/main-layout.component';

const routes: Routes = [
{
    path: '', component: MainLayoutComponent,
  loadChildren: () =>
  import('./pages/modules/video/video.module').then( m => m.VideoModule)
},
{
  path: 'admin',
  loadChildren: () =>
  import('./pages/modules/Admin/admin.module').then( m => m.AdminModule)
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

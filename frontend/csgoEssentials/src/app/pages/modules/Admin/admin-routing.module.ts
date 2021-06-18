import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from 'src/app/_layout/dashboard/admin-layout.component';
import { AdminLoginComponent } from './admin-login/admin-login.component';

import { AdminComponent } from './component/admin.component';



const routes: Routes = [
{
  path: '', component: AdminLoginComponent,
},
{
  path: '', component: AdminLayoutComponent, children: [
    {
      path: 'dashboard', component: AdminComponent
    }
  ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

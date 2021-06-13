import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './modules/Admin/components/admin/admin.component';
import { HomeComponent } from './modules/Home/components/home.component';
import { VideoComponent } from './modules/video/video.component';

const routes: Routes = [
  {path: '', component: HomeComponent, children: [
    {path: 'video', component: VideoComponent}
  ]},
  {path: 'admin', component: AdminComponent},


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

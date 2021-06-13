import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminModule } from './modules/Admin/admin.module';
import { HomeModule } from './modules/Home/home.module';
import { FooterComponent } from './core/template/footer/footer.component';
import { HeaderComponent } from './core/template/header/header.component';
import { MainComponent } from './core/template/main/main.component';
import { SidebarComponent } from './core/template/sidebar/sidebar.component';
import { VideoComponent } from './modules/video/video.component';



@NgModule({
  declarations: [
    AppComponent,
    FooterComponent,
    HeaderComponent,
    MainComponent,
    SidebarComponent,
    VideoComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AdminModule,
    HomeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

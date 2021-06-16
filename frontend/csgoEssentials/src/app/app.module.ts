import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminLayoutComponent } from './_layout/dashboard/admin-layout.component';
import { MainLayoutComponent } from './_layout/main-layout/main-layout.component';
import { HeaderComponent } from './_layout/main-layout/header/header.component';
import { FooterComponent } from './_layout/main-layout/footer/footer.component';
import { SidebarComponent } from './_layout/main-layout/sidebar/sidebar.component';
import { AdminLoginComponent } from './pages/admin-login/admin-login.component';


@NgModule({
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    MainLayoutComponent,
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    AdminLoginComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

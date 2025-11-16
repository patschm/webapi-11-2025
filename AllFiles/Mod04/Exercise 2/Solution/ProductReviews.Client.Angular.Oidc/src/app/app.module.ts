import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { API_RESOURCES, APP_CONFIG, authConfig } from './app.config';
import { ProductGroupsModule } from './product-groups';
import { MainComponent } from './main.component';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UnauthorizedComponent } from './unauthorized.component';
import { HomeComponent } from './home/home.component';
import { OAuthModule, OAuthStorage } from 'angular-oauth2-oidc';


@NgModule({
  declarations: [
    MainComponent,
    UnauthorizedComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ProductGroupsModule,
    HttpClientModule,
    OAuthModule.forRoot()
  ],
  providers: [
    {
      provide: OAuthStorage, useValue: localStorage
    },
    { 
      provide: APP_CONFIG, 
      useValue:API_RESOURCES 
    }
  ],
  bootstrap: [MainComponent]
})
export class AppModule { }

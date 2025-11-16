import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { API_RESOURCES, APP_CONFIG, msalConfig } from './app.config';
import { ProductGroupsModule } from './product-groups';
import { MainComponent } from './main.component';
import { BrowserModule } from '@angular/platform-browser';
import { MsalBroadcastService, MsalGuard, MsalGuardConfiguration, MsalInterceptor, MsalInterceptorConfiguration, MsalModule, MsalRedirectComponent, MsalService, MSAL_GUARD_CONFIG, MSAL_INSTANCE, MSAL_INTERCEPTOR_CONFIG } from '@azure/msal-angular';
import { InteractionType, IPublicClientApplication, LogLevel, PublicClientApplication} from '@azure/msal-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { UnauthorizedComponent } from './unauthorized.component';

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication(msalConfig);
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(API_RESOURCES.productGroupListApi.endpoint, API_RESOURCES.productGroupListApi.scopes);
  protectedResourceMap.set(API_RESOURCES.productGroupDetailApi.endpoint, API_RESOURCES.productGroupDetailApi.scopes);
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap
  };
}
export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [...API_RESOURCES.productGroupDetailApi.scopes],
    },
    loginFailedRoute: 'login-failed'
  };
}

@NgModule({
  declarations: [
    MainComponent,
    UnauthorizedComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ProductGroupsModule,
    HttpClientModule,
    MsalModule
  ],
  providers: [
    MsalService,
    MsalBroadcastService,
    MsalGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory
    },
    { 
      provide: MSAL_INTERCEPTOR_CONFIG, 
      useFactory: MSALInterceptorConfigFactory 
    },
    { 
      provide: APP_CONFIG, 
      useValue:API_RESOURCES 
    }
  ],
  bootstrap: [MainComponent, MsalRedirectComponent]
})
export class AppModule { }

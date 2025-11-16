import { Component, OnDestroy, OnInit } from '@angular/core';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { catchError, EMPTY, filter, Subscription, tap } from 'rxjs';
import { authConfig } from './app.config';

@Component({
  selector: 'app-main',
  template: `
    <h1>welcome</h1>
    <router-outlet><router-outlet>
  `,
  styles: [
  ]
})
export class MainComponent implements OnInit, OnDestroy {
  public welcome:string = "Welcome ";
  
  constructor(private authSvc: OAuthService) {  }

  ngOnInit(): void {
    this.configuerFlow();
    // Watch the token_received events
    this.authSvc.events.pipe(
      tap(d=>console.log(`Auth Evt: ${d.type}`)),
      filter(d=>d.type == "token_received"))
    .subscribe(d=>console.log("Acces Token received!"));
  }
  ngOnDestroy(): void {
  }

  async configuerFlow(){
    // Setup the service
    this.authSvc.configure(authConfig);
    try
    {
      let ok = await this.authSvc.loadDiscoveryDocument();
      console.log(ok.type);
      // Raise login screen
      await this.authSvc.tryLoginCodeFlow({
        onTokenReceived: inf=>console.log(inf)
        });
    }
    catch (err)
    {
      console.error(err);
    }
    // Configure refresh tokens
    this.authSvc.setupAutomaticSilentRefresh();
  }
}
  

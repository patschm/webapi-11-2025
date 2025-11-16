import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { filter, Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  template: `
   <div>Welcome to home Route</div><br />
   <button (click)="login()">Login</button>
  `,
  styles: [
  ]
})
export class HomeComponent implements OnInit {
  
  isAuthenticated = false;
  constructor(private authSvc: OAuthService, private router: Router) { }

  ngOnInit(): void {
    console.log("Init Home");
    this.authSvc.events
      .pipe(filter(d=>d.type == "token_received"))
      .subscribe(x=>{
        this.isAuthenticated = true;
        this.router.navigate(["productgrouplist"]);
      });
  }

  login()
  {
    let token = this.authSvc.getAccessToken();
    if (token != null)
    {
      this.router.navigate(["productgrouplist"]);
    }
    else
    {
      this.authSvc.initCodeFlow();
    }  
  }
}

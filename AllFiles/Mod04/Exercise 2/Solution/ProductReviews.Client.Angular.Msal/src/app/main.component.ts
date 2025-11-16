import { Component, OnInit } from '@angular/core';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { InteractionStatus } from '@azure/msal-browser';
import { catchError, EMPTY, filter } from 'rxjs';

@Component({
  selector: 'app-main',
  template: `
    <h1>welcome</h1>
    <router-outlet><router-outlet>
    
  `,
  styles: [
  ]
})
export class MainComponent implements OnInit {
  public welcome:string = "Welcome ";
  constructor(private auth: MsalService, private msalBC:MsalBroadcastService) { }

  login() {
    this.auth.loginPopup()
      .pipe(catchError(err =>{
        console.error(err);
        return EMPTY;
      }))
      .subscribe(resp=> {
        console.log(resp.account);
         this.welcome = resp.account.username;
        });
  }
  ngOnInit(): void {
    this.msalBC.inProgress$
      .pipe(filter(stat=>stat === InteractionStatus.None))
      .subscribe(arg=>this.welcome += arg);
      
    if (this.auth.instance == null)
    {
      this.login();
    }
  }

}

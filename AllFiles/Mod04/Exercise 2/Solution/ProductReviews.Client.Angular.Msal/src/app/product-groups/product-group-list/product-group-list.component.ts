import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { EventMessage, InteractionStatus } from '@azure/msal-browser';
import { catchError, EMPTY, filter, Observable } from 'rxjs';
import { IAppConfig } from 'src/app/app-config';
import { APP_CONFIG } from 'src/app/app.config';
import { ProductGroup } from 'src/app/entities/entities';

@Component({
  selector: 'app-product-group-list',
  templateUrl: 'product-group-list.html',
  styles: [
  ]
})
export class ProductGroupListComponent implements OnInit {
  public productGroups$: Observable<ProductGroup[]> | undefined;
  private isAuthenticated: boolean = false;
  public handleClick(pg: ProductGroup):void
  {

  }

  constructor(private http:HttpClient, private msalBC: MsalBroadcastService, @Inject(APP_CONFIG)private config:IAppConfig) { }

  ngOnInit(): void {
    this.msalBC.inProgress$
      .pipe(filter(stat=>stat === InteractionStatus.None))
      .subscribe(stat=>this.loadProductGroups());
  }

  private loadProductGroups(page:number = 1, count:number = 15) {
      this.productGroups$ = this.http.get<ProductGroup[]>(`${this.config.productGroupListApi.endpoint}?count=${count}`)
                                            .pipe(catchError(err=>{
                                              if  (err.status == 401) alert("Unauthorized");
                                              else console.error(err);
                                              return EMPTY;
                                            }));
    
  }

}

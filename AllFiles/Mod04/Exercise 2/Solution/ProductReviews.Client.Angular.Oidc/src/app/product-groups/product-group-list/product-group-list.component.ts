import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { catchError, EMPTY, filter, Observable, tap } from 'rxjs';
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
  public handleClick(pg: ProductGroup):void
  {

  }

  constructor(private http:HttpClient, @Inject(APP_CONFIG)private config:IAppConfig, private authSvc: OAuthService) { }

  ngOnInit(): void {
      this.productGroups$ =  this.loadProductGroups();  
  }

  private loadProductGroups(page:number = 1, count:number = 15) : Observable<ProductGroup[]> {
    const token = this.authSvc.getAccessToken();
    const options = {
        headers:{
            "Authorization": `Bearer ${token}`
        }
    };
    return this.http.get<ProductGroup[]>(`${this.config.productGroupListApi.endpoint}?count=${count}`, options)
              .pipe(
                catchError(err=>{
                if  (err.status == 401) alert("Unauthorized");
                else console.error(err);
                return EMPTY;
              }));
    
  }

}

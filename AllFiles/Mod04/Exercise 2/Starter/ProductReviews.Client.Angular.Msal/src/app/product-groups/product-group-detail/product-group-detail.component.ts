import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { catchError, EMPTY, filter, switchMap, take } from 'rxjs';
import { IAppConfig } from 'src/app/app-config';
import { APP_CONFIG } from 'src/app/app.config';
import { ProductGroup } from 'src/app/entities/entities';

@Component({
  selector: 'app-product-group-detail',
  templateUrl: 'product-group-detail.html',
  styleUrls: ["product-group-detail.css"]
})
export class ProductGroupDetailComponent implements OnInit {
  public formGroup: FormGroup;

  public submit() {
    let pg: ProductGroup = new ProductGroup();
    Object.assign(pg, this.formGroup.value);
    this.http.put<ProductGroup>(`${this.config.productGroupDetailApi.endpoint}/${pg.id}`, pg)
    .pipe(catchError(err=>{
      if  (err.status == 401 || err.status == 403) alert("Unauthorized");
      return EMPTY;
    }))
      .subscribe({
        next:resp=>{
          this.formGroup.reset();
          this.formGroup.patchValue({
            "id": resp.id,
            "name": resp.name,
            "image": resp.image
          });
        },
        error:err=>{
          console.error(err);
        }
      });

  }
  constructor(private http:HttpClient, private formBld:FormBuilder, private act: ActivatedRoute, @Inject(APP_CONFIG)private config:IAppConfig) { 
    this.formGroup = this.formBld.group({
      "id":[0],
      "name": ["", [Validators.required]],
      "image":[""]
    });
  }

  ngOnInit(): void {
    this.act.params.pipe(
      take(1),
      filter(param=>("id" in param)),
      switchMap(param=> this.http.get<ProductGroup>(`${this.config.productGroupDetailApi.endpoint}/${+param["id"]}`))
    ).subscribe(pg=>{
      this.formGroup.setValue({
        "id":pg.id,
        "name":pg.name,
        "image":pg.image
      });
    });
  }

}

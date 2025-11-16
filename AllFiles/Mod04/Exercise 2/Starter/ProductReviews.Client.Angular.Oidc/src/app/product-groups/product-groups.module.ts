import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductGroupListComponent } from './product-group-list/product-group-list.component';
import { ProductGroupDetailComponent } from './product-group-detail/product-group-detail.component';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    ProductGroupListComponent,
    ProductGroupDetailComponent
  ],
  exports: [
    ProductGroupListComponent,
    ProductGroupDetailComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule
  ]
})
export class ProductGroupsModule { }

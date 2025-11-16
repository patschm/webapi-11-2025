import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { UnauthorizedComponent } from './unauthorized.component';
import { ProductGroupDetailComponent, ProductGroupListComponent } from './product-groups';

const routes: Routes = [
  {
    path:"productgrouplist",
    component:ProductGroupListComponent,
    canActivate:[MsalGuard]
  },
  {
    path:"productgroup/:id",
    component:ProductGroupDetailComponent,
    canActivate:[MsalGuard]
  },
  {
    path: "login-failed",
    component: UnauthorizedComponent
  },
  {
    path:"",
    redirectTo:"productgrouplist",
    pathMatch:"full"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

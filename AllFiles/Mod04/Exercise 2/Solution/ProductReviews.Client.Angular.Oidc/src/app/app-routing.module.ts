import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UnauthorizedComponent } from './unauthorized.component';
import { ProductGroupDetailComponent, ProductGroupListComponent } from './product-groups';
import { MainComponent } from './main.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path:"home",
    component:HomeComponent
  },
  {
    path:"productgrouplist",
    component:ProductGroupListComponent
  },
  {
    path:"productgroup/:id",
    component:ProductGroupDetailComponent
  },
  {
    path: "login-failed",
    component: UnauthorizedComponent
  },
  {
    path:"",
    redirectTo:"home",
    pathMatch:"full"
  },
  {
    path: '**',
    redirectTo: 'home',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

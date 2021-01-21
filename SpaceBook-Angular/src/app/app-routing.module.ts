import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { GalleryComponent } from "./components/gallery/gallery.component";

import { RouterModule,Routes } from "@angular/router";

const routes : Routes = [
  //Default route for navigation
  {path: '', redirectTo:'/dashboard', pathMatch:'full' },
  {path: 'dashboard', component: DashboardComponent},
  {path: 'gallery',component:GalleryComponent},
  { path: '**', redirectTo:'/dashboard'}, //WildCard, if not found...
];


@NgModule({
  exports: [ RouterModule ],
  imports: [
    RouterModule.forRoot(routes)
  ]
  // , 
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { GalleryComponent } from "./components/gallery/gallery.component";
import { DirectMessagingComponent } from "./components/direct-messaging/direct-messaging.component";
import { PictureDetailComponent } from "./components/picture/picture-detail/picture-detail.component";
import { UploadPictureComponent } from "./components/picture/upload-picture/upload-picture.component";
import { LogInComponent } from "./components/user/log-in/log-in.component";
import { FollowingComponent } from "./components/user/following/following.component";
import { ProfileComponent } from "./components/user/profile/profile.component";

import { RouterModule,Routes } from "@angular/router";

const routes : Routes = [
  //Default route for navigation
  {path: '', redirectTo:'/dashboard', pathMatch:'full' },
  {path: 'dashboard', component: DashboardComponent},
  {path: 'user/:id',component:ProfileComponent},
  {path: 'messaging',component:DirectMessagingComponent},
  {path: 'picture/upload',component:UploadPictureComponent},
  {path: 'picture/:id',component:PictureDetailComponent},
  {path: 'authentication',component:LogInComponent},
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

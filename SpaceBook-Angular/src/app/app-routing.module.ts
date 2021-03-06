import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { DashboardComponent } from "./components/dashboard/dashboard.component";
import { GalleryComponent } from "./components/gallery/gallery.component";
import { DirectMessagingComponent } from "./components/direct-messaging/direct-messaging.component";
import { PictureDetailComponent } from "./components/picture/picture-detail/picture-detail.component";
import { UploadPictureComponent } from "./components/picture/upload-picture/upload-picture.component";
import { LogInComponent } from "./components/user/log-in/log-in.component";
import { ProfileComponent } from "./components/user/profile/profile.component";
import { MyProfileComponent } from "./components/user/my-profile/my-profile.component";


import { RouterModule,Routes } from "@angular/router";
import { UserSearchComponent } from './components/user-search/user-search.component';

const routes : Routes = [
  //Default route for navigation
  {path: '', redirectTo:'/dashboard', pathMatch:'full' },
  {path: 'dashboard', component: DashboardComponent},
  {path: 'user',component:MyProfileComponent },
  {path: 'user/:username',component:ProfileComponent},
  {path: 'messaging',component:DirectMessagingComponent},
  {path: 'picture/upload',component:UploadPictureComponent},
  {path: 'picture/:id',component:PictureDetailComponent},
  {path: 'authentication',component:LogInComponent},
  {path: 'search', component: UserSearchComponent},
  { path: '**', redirectTo:'/dashboard'}, //WildCard, if not found...
];


@NgModule({
  exports: [ RouterModule ],
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload'
    })
  ]
  // , 
})
export class AppRoutingModule { }

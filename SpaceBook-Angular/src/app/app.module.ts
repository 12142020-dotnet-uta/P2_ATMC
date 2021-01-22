import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { GalleryComponent } from './components/gallery/gallery.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

/* Bootstrap Modules */
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DirectMessagingComponent } from './components/direct-messaging/direct-messaging.component';
import { PictureDetailComponent } from "./components/picture/picture-detail/picture-detail.component";
import { UploadPictureComponent } from "./components/picture/upload-picture/upload-picture.component";
import { LogInComponent } from './components/user/log-in/log-in.component';

@NgModule({
  declarations: [
    AppComponent,
    GalleryComponent,
    DashboardComponent,
    UploadPictureComponent,
    PictureDetailComponent,
    DirectMessagingComponent,
    LogInComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

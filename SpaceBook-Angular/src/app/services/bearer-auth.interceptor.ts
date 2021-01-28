import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class BearerAuthInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const idToken = localStorage.getItem('id_token');
    console.log("Http request: "+request.url);

    if (idToken) {
      //if token exists, add authorization header
      const cloned = request.clone({
          headers: request.headers.set("Authorization",
              "Bearer " + idToken)
      });
      console.log('added token to request: '+idToken);

      return next.handle(cloned);
  }
  else {
    //otherwise, continue without header
      return next.handle(request);
  }
  }
}

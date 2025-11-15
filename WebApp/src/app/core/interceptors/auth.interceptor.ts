import { HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import {AuthService, TokenSchema } from '../auth/auth.service';
import { environment } from '../../../environments/environment';
import { catchError, map, of, switchMap } from 'rxjs';

export const skipInterceptKey = "X-Skip-Auth-Interceptor";

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  if(skipIntercept(req)){
    req = removeSkipIntercept(req);
    return next(req);
  }

  req = addBaseUrl(req);
  
  // in here auth services is lazy and singleton lifetime
  const authServices = inject(AuthService);
  
  return authServices.getToken().pipe(
    switchMap(token=>{
      if(token === undefined){
        return of(req);
      }
      if(token === null){
        return authServices.RefreshToken().pipe(
          map(newToken=>{
            req = req.clone ({
              setHeaders: { Authorization: `${TokenSchema} ${newToken.accessToken}` },
            })
            return req;
          })
        )
      }
      req = req.clone({
        setHeaders: { Authorization: `${TokenSchema} ${token}` },
      });
      return of(req);
    }),
    switchMap(updatedReq => next(updatedReq)),
    catchError(err => {
      console.error('Auth Interceptor error:', err);
      return next(req);
    })
  )
};



function addBaseUrl(request: HttpRequest<any>) : HttpRequest<any>{
  // http include https
  if(request.url.startsWith('http')){
    return request;
  }
  const baseUrl = environment.api;
  let rootUrl = request.url;
  if(!rootUrl.startsWith('/')){
    rootUrl = '/' + rootUrl;
  }
  const url = `${baseUrl}${rootUrl}`
  return request.clone({url: url});
}

function skipIntercept(request: HttpRequest<any>) : boolean{
  return request.headers.has(skipInterceptKey)
    || request.url.includes('cloudinary.com')
}

function removeSkipIntercept(request: HttpRequest<any>) : HttpRequest<any>{
  if(request.headers.has(skipInterceptKey)){
    return request.clone({
      headers: request.headers.delete(skipInterceptKey)
    });
  }
  return request;
}
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { JwtModel, LoginPassword } from './auth.model';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import {catchError, map, tap} from "rxjs/operators"
import { AuthDefinedApi } from './auth-defined-api';


export const TokenSchema = "Bearer";

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  // inject
  private httpClient = inject(HttpClient);

  // field and properties
  private readonly tokenKey = "token";

  // behavior
  private token$ = new BehaviorSubject<JwtModel | null>(this.getJwtToken());
  private isAuthenticated$ = new BehaviorSubject<boolean>(false);

  // method public export method
  public LoginPassword(loginPassword: LoginPassword) : Observable<JwtModel>{
    return this.handleSaveTokenWhenLoginSuccess(
      this.httpClient.post<JwtModel>(AuthDefinedApi.LoginPassword, loginPassword)
    );
  }
  public LoginWithProvider(provider: string) : Observable<void>{
    return this.httpClient.get<void>(AuthDefinedApi.LoginWithProvider(provider));
  }
  public TokenExchange(userId: string, token: string) : Observable<JwtModel>{
    return this.handleSaveTokenWhenLoginSuccess(
      this.httpClient.post<JwtModel>(AuthDefinedApi.TokenExchange(userId, token), {})
    );
  }
  public LinkWithProvider(userId: string, token: string){
    return this.handleSaveTokenWhenLoginSuccess(
      this.httpClient.post<JwtModel>(AuthDefinedApi.LinkWithProvider(userId, token), {})
    );
  }
  public logout(): Observable<void> {
    return this.httpClient.post<void>(AuthDefinedApi.Logout, {}).pipe(
      tap(()=>{
        this.removeToken();
        this.token$.next(null);
        this.isAuthenticated$.next(false);
      })
    )
  }
  public isAuthenticated() : Observable<boolean>{
    return this.isAuthenticated$.asObservable();
  }
  public refreshToken() : Observable<JwtModel>{
    const jwtModel = this.getJwtToken();
    if(!jwtModel || !jwtModel.accessToken || !jwtModel.refreshToken){
      console.warn('Not find token in local storage');
      return throwError(() => new Error('Not find token in local storage'));
    }
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `${TokenSchema} ${jwtModel.accessToken}`,
      'RefreshToken': jwtModel.refreshToken
    });
    return this.httpClient.post<JwtModel>(
      AuthDefinedApi.RefreshToken, {}, { headers, observe: 'response' }
    ).pipe(
      tap((response) => {
        if (response.status === 200 && response.body) {
          this.saveToken(response.body);
          this.token$.next(response.body);
          this.isAuthenticated$.next(true);
        }
      }),
      map((response) => response.body as JwtModel),
      catchError((error) => {
        if (error.status === 401) {
          console.error('Refresh token expired or invalid');
          this.removeToken();
          this.token$.next(null);
          this.isAuthenticated$.next(false);
        }
        return throwError(() => error);
      })
    );
  }

  public getToken() : Observable<string | undefined | null>{
    // you can check if access token has exprise date => return null
    return this.token$.asObservable().pipe(
      map((jwt)=>{
        if(!jwt?.accessToken){
          this.isAuthenticated$.next(false);
          return undefined;
        }
        if(this.hasValidToken(jwt.accessToken)){
          return jwt.accessToken;
        }
        else{
          this.isAuthenticated$.next(false);
          return null;
        }
      })
    )
  }

  // private helper for auth services
  private saveToken(jwtToken: JwtModel){
    const tokenString = JSON.stringify(jwtToken);
    localStorage.setItem(this.tokenKey, tokenString);
  }
  private getJwtToken() : JwtModel | null{
    const tokenString = localStorage.getItem(this.tokenKey);
    if(tokenString === null){
      return null;
    }
    const token = JSON.parse(tokenString);
    return token;
  }
  private handleSaveTokenWhenLoginSuccess(handleGetToken: Observable<JwtModel>) : Observable<JwtModel>{
    return handleGetToken.pipe(
      tap((jwtModel: JwtModel)=>{
        this.saveToken(jwtModel);
        this.token$.next(jwtModel);
        this.isAuthenticated$.next(true);
      })
    )
  }
  private hasValidToken(accessToken: string) : boolean{
    try{
      // payload in token is includes header payload and signature
      const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));
      const now = Math.floor(Date.now() / 1000);
      const validTimeToken = tokenPayload.exp && tokenPayload.exp > now;
      return validTimeToken;
    }
    catch{
      return false;
    }
  }
  private removeToken(){
    localStorage.removeItem(this.tokenKey);
  }
}



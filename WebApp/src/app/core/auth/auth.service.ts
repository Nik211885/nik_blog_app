import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { JwtModel, LoginPassword, RolesDefine } from './auth.model';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import {catchError, map, switchMap, tap} from "rxjs/operators"
import { AuthDefinedApi } from './auth-defined-api';


export const TokenSchema = "Bearer";

@Injectable({
  providedIn: 'root',
})

export class AuthService {
  // inject
  private httpClient = inject(HttpClient);

  constructor(){
    this.checkAuthenticated();
  }

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
  public IsAuthenticated() : Observable<boolean>{
    return this.isAuthenticated$.asObservable();
  }
  public RefreshToken() : Observable<JwtModel>{
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
  public hasRole(expectedRole: RolesDefine): Observable<boolean>{
    const token = this.getJwtToken();
    if(!token){
      return of(false);
    }
    if (this.hasValidToken(token.accessToken)) {
      const payload = this.decodeToken(token.accessToken);
      return of(payload?.role === expectedRole);
    }
    return this.RefreshToken().pipe(
      switchMap(newToken => {
        const payload = this.decodeToken(newToken.accessToken);
        return of(payload?.role === expectedRole);
      }),
      catchError(() => of(false)) 
    );
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
      const payload = this.decodeToken(accessToken);
      if(!payload || !payload.exp){
        return false;
      }
      const now = Math.floor(Date.now() / 1000);
      return payload.exp > now;
    }
    catch{
      return false;
    }
  }

  private decodeToken(accessToken: string) : any | null{
    try{
      // jwt inclues is header payload and signature => just get body get claim in it
      const payloadBase64 = accessToken.split('.')[1];
      const payloadJson = atob(payloadBase64);
      return JSON.parse(payloadJson);
    }
    catch(e){
      console.error('Invalid JWT token', e);
      return null;
    }
  }

  private checkAuthenticated(){
    this.isAuthenticated().subscribe(isAuth=>{
       this.isAuthenticated$.next(isAuth);
    })
  }

  private isAuthenticated(): Observable<boolean> {
    const jwtToken = this.getJwtToken();

    if (!jwtToken?.accessToken) {
      return of(false);
    }

    if (this.hasValidToken(jwtToken.accessToken)) {
      return of(true);
    }

    return this.RefreshToken().pipe(
      switchMap(newJwt => of(!!newJwt?.accessToken)),
      catchError(() => of(false))
    );
  }
  private removeToken(){
    localStorage.removeItem(this.tokenKey);
  }
}



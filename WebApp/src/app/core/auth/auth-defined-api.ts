export class AuthDefinedApi{
    static readonly LoginPassword = '/api/sign-in/login-password';
    static readonly Logout = '/api/sign-in/logout';
    static readonly RefreshToken = "/api/sign-in/refresh-token";

    static LoginWithProvider(provider: string){
        return `/api/sign-in/login?provider=${provider}`;
    }
    static TokenExchange(userId: string, token: string){
        return `/api/sign-in/token-exchange?userId=${userId}&token=${token}`;
    }
    static LinkWithProvider(userId: string, token: string){
        return `/api/sign-in/link-provider?userId=${userId}&token=${token}`;
    }
}

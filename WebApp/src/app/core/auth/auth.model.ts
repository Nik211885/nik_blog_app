// model
export interface JwtModel{
    accessToken: string,
    refreshToken: string
    expirationAccessToken: Date,
    expirationRefreshToken: Date
}

export interface LoginPassword{
    userName: string,
    password: string,
}

// constains
export enum RolesDefine{
    User = "User",
    Admin = "Admin"
}
namespace Application.Services.SignInManager;

internal static class SignInConstMessage
{
    public const string UserNameCanNotBeNull = "Username không được để trống!";
    public const string PasswordInval = "Mật khẩu không đúng!";
    public const string LoginPasswordInval = "Tài khoản hoặc mật khẩu không đúng!";
    public const string AccountHasLock = "Tài khoản đang bị khóa vì {0} cho tới khi {1}!";
    public const string UserNotFound = "Không tìm thấy tài khỏan!";
    public const string HasErrorWhenLinkWithProvider = "Có lỗi trong quá trình liên kết tài khoản!";
    public const string LoginProviderHasExits = "Đã có liên kết tài khoản này!";
    public const string TokenExchangeExpired = "Mã cấp phiên mới đã được sử dụng!";
}

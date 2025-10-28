namespace Application.Services.UserManager;
/// <summary>
///  Const message for user manage services you can share some instance for other services ;))
/// </summary>
internal static class UserManageMessageConst
{
    public const string UserNameIsRequired = "Username không đc để trống!";
    public const string UserNameMaxLength = "Username không lớn hớn 50 kí tự!";
    public const string EmailIsValidFormat = "Email không đúng định dạng!";
    public const string PasswordIsValidFormat = "Mật khẩu phải chứa các kí tự hoa, thng, số, kí tự đặc biệt và lớn hơn 8 kí tự!";
    public const string PasswordNotMatchConfirm = "Mật khẩu và xác nhận mật khẩu không khớp!";
    public const string UserNameIsExits = "Username này đã tồn tại!";
    public const string EmailIsExits = "Email này đã đc liên kết với một tài khoản khác!";
    public const string UserNotFound = "Nguoi dùng không tồn tại!";
    public const string AccountHasLock = "Tài khoản đã bị khóa!";
    public const string AccountHasUnLock = "Tài khoản chưa bị khóa!";
    public const string EmailAddressHasConfirm = "Tài khoản đã xác nhận địa chỉ email!";
    public const string NewPasswordCanNotLikeOldPassword = "Mật khẩu mới và mật khẩu cũ không được trùng nhau!";
    public const string InvalidUserToken = "Token không đúng!";
}
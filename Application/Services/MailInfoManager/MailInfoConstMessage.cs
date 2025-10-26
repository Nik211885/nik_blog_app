namespace Application.Services.MailInfoManager;

internal static class MailInfoConstMessage
{
    public const string EmailIdHasExits = "Địa chỉ mail đã tồn tại!";
    public const string EmailIdInValid = "Địa chỉ mail không đúng định dạng!";
    public const string HostIsRequired = "Host không để trống!";
    public const string UserNameIsRequired = "Username không để trống!";
    public const string NameIsRequired = "Name không để trống!";
    public const string ApplicationPasswordIsRequired = "Mật khẩu ứng dụng không để trống!";
    public const string MailInfoNotFound = "Không tìm thấy thông tin mail infor cần tìm kiếm!";
    public const string NotificationTemplateNotFound = "Không tìm thấy biểu mẫu!";
    public const string NotificationNotUseMailChanel = "Biểu mẫu không qua kênh gửi mail không cần thêm địa chỉ email!";
    public const string MailInfoInActiveCanNotAddToTemplateActive = "Tài khoản mail không hoạt động không thể thêm vào biểu mẫu đang hoạt động!";
}
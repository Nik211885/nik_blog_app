namespace Application.Services.NotificationTemplateManager;

internal static class NotificationTemplateConstMessage
{
    public const string CodeIsRequired = "Mã biểu mẫu không để trống!";
    public const string SubjectIsRequired = "Chủ đề biểu mẫu không để trống!";
    public const string ChanelMailNeedAddEmailInfo = "Thông báo qua kênh mail cần thêm cấu hình tài khoản mail!";
    public const string NotChanelMailNotNeedAddEmailInfo = "Thông báo không qua kênh mail không cần thêm cấu hình tài khoản mail!";
    public const string HasExitsCode = "Mã biểu mẫu đã tồn tại!";
    public const string ContentCanNotBeNull = "Nội dung biểu mẫu phải tồn tại ít nhất 1 trong hai dạng!";
    public const string ArgumentInContentMustEqualsArguments = "Tham số trong biểu mẫu không khớp số lượng tham số truyền vào!";
    public const string NotificationTemplateNotFound = "Không tìm thấy biểu mẫu!";
    public const string ServicesTypeMustHaveOneOrMoreTemplateActive = "Không có biễu mẫu nào còn hoạt động với dịch vụ này, không thể tắt hoạt động biễu mẫu này được!";
    public const string CantNotFindMailInfoToAddTemplate = "Không tìm thấy thông tin cấu hình mail để thêm vào template hoặc mail config không hoạt động!";
    public const string CanNotFindMailInfoToSendTemplate = "Không tìm thấy cấu hình mail để gửi thông báo!";
}
using System.Diagnostics.CodeAnalysis;

namespace Application.Exceptions;

public static class ThrowHelper
{
    /// <summary>
    ///     Throw unauthorized exception
    /// </summary>
    /// <param name="message">message for unauthorized</param>
    /// <exception cref="UnauthorizedException"></exception>
    [DoesNotReturn]
    public static void ThrowWhenUnauthorized(string message = "Bạn không có quyền truy cập")
    {
        throw new UnauthorizedException(message);
    }
    /// <summary>
    ///     Throw if have exception for business logic
    /// </summary>
    /// <param name="message">message for business exception</param>
    /// <exception cref="UnauthorizedException"></exception>
    [DoesNotReturn]
    public static void ThrowWhenBusinessError(string message = "Đã có lỗi xảy ra")
    {
        throw new BusinessException(message);
    }

    /// <summary>
    ///     Throw not found when item is null
    /// </summary>
    /// <param name="obj">object need check null</param>
    /// <param name="message">message when item is not found</param>
    /// <exception cref="NotFoundException">Exception</exception>
    public static void ThrowWhenNotFoundItem<T>([NotNull] T? obj, string message = "Không tìm thấy nội dung yêu cầu")
    {
        if (obj == null)
        {
            throw new NotFoundException(message);
        }
    }
    /// <summary>
    ///     Throw business when item is exits  
    /// </summary>
    /// <param name="obj">object need check exits</param>
    /// <param name="message">message throw when item has exits</param>
    /// <typeparam name="T">generic for boxing</typeparam>
    /// <exception cref="BusinessException">Throw business exception</exception>
    public static void ThrowBusinessErrorWhenExitsItem<T>(T? obj, string message = "Tài nguyên này đã tồn tại trong hệ thống")
    {
        if (obj != null)
        {
            throw new BusinessException(message);
        }
    }
    /// <summary>
    ///     Throw business exception with delegate function 
    /// </summary>
    /// <param name="obj">data  need check condition</param>
    /// <param name="condition">condition with</param>
    /// <param name="message">message when throw message</param>
    /// <typeparam name="T">Genric with object</typeparam>
    /// <exception cref="BusinessException">Business exception</exception>
    public static void ThrowBusinessWithCondition<T>(T? obj, Func<T?, bool> condition, string message)
    {
        if (condition(obj))
        {
            throw new BusinessException(message);
        }
    }
}
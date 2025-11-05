using Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Extensions;

public static class UserDbSetExtensions
{
    /// <summary>
    ///    Create random user name by email address
    /// </summary>
    /// <param name="users">User name</param>
    /// <param name="emailAddress">email address</param>
    /// <param name="randomSuffixLength">random  for suffix user name</param>
    /// <returns>
    ///     Return user name unique by email address
    /// </returns>
    public static async Task<string> GenerateUniqueUserNameByEmailAddressAsync(this DbSet<User> users, string emailAddress, int randomSuffixLength = 4, CancellationToken cancellationToken = default)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var normalized = emailAddress.Trim().ToLower().Split("@")[0];
        var existingUsernames = await users
            .Where(u => u.UserName.StartsWith(normalized))
            .Select(u => u.UserName)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        if (!existingUsernames.Contains(normalized))
        {
            return normalized;
        }
        var random = new Random();
        string candidate;
        do
        {
            var suffix = new string([.. Enumerable.Repeat(chars, randomSuffixLength).Select(s => s[random.Next(s.Length)])]);
            candidate = $"{normalized}{suffix}";
        }
        while (existingUsernames.Contains(candidate)); 

        return candidate;
    }
}

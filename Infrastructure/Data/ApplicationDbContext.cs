using Application.Entities;
using Application.ValueObject;
using Infrastructure.Data.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;
/// <summary>
///  Defined all db context for application
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    ///  Data model about connection string for postgresql
    /// </summary>
    private readonly PostgresConnectionDataModel  _postgresConnectionString;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context,
        IConfiguration configuration)
        : base(context)
    {
        PostgresConnectionDataModel? postgresConnectionDataModel = configuration
            .GetSection("PostgresConnectionString").Get<PostgresConnectionDataModel>();
        ArgumentNullException.ThrowIfNull(postgresConnectionDataModel);
        _postgresConnectionString = postgresConnectionDataModel;
    }
    /// <summary>
    ///     Db set for comment
    /// </summary>
    public DbSet<Comment> Comments { get; set; }
    /// <summary>
    ///     Db set for post
    /// </summary>
    public DbSet<Post> Posts { get; set; }
    /// <summary>
    /// Db set for login provider
    /// </summary>
    public DbSet<LoginProvider> LoginProviders { get; set; }
    /// <summary>
    /// Db set for subject
    /// </summary>
    public DbSet<Subject> Subjects { get; set; }
    /// <summary>
    /// Db set for user
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    /// Db set for post subject
    /// </summary>
    public DbSet<PostSubject> PostSubjects { get; set; }
    /// <summary>
    /// Db set user follower
    /// </summary>
    public DbSet<UserFollower> UserFollowers { get; set; }
    /// <summary>
    ///     Db set for comment reaction
    /// </summary>
    public DbSet<ReactionEntity> ReactionEntities { get; set; }
    /// <summary>
    ///  Db set for notification template
    /// </summary>
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
    /// <summary>
    ///  Db set for notifications
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }
    /// <summary>
    ///  Db set for mail info
    /// </summary>
    public DbSet<MailInfo> MailInfos { get; set; }
    /// <summary>
    ///   Db set for arguments
    /// </summary>
    public DbSet<Arguments> Arguments { get; set; }
    /// <summary>
    ///     Override method in db context will connection postgresql with connection string has config
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_postgresConnectionString.Default);
        base.OnConfiguring(optionsBuilder);
    }
    /// <summary>
    ///     Override method design database with ef it will scan assembly has inherent
    ///     for type is IEntityTypeConfiguration add execute with config each db set
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);*/
        //  you can use references scan to create entity type configuration 
        //  but in here i want to manual scan
        modelBuilder.ApplyConfiguration(new CommentConfiguration())
            .ApplyConfiguration(new LoginProviderConfiguration())
            .ApplyConfiguration(new PostConfiguration())
            .ApplyConfiguration(new PostSubjectConfiguration())
            .ApplyConfiguration(new SubjectConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new ReactionEntityConfiguration())
            .ApplyConfiguration(new UserFollowerConfiguration())
            .ApplyConfiguration(new NotificationConfiguration())
            .ApplyConfiguration(new MailInfoConfiguration())
            .ApplyConfiguration(new ArgumentConfigurations())
            .ApplyConfiguration(new NotificationTemplateConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}


internal class PostgresConnectionDataModel
{
    /// <summary>
    ///  Main connection string for application
    /// </summary>
    public string Default { get; set; } = string.Empty;
}
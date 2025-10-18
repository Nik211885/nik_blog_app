using Application.Entities;
using Application.ValueObject;
using Infrastructure.Configurations;
using Infrastructure.Data.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
/// <summary>
///  Defined all db context for application
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    ///  Data model about connection string for postgresql
    /// </summary>
    private readonly PostgresConnectionString  _postgresConnectionString;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context,
        PostgresConnectionString postgresConnectionString)
        : base(context)
    {
        _postgresConnectionString = postgresConnectionString;
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
            .ApplyConfiguration(new UserFollowerConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
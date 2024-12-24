using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Models.Base;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Areas.Identity.Data;

public class ApplicationDBContext : IdentityDbContext<User>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<TopicAssessment> TopicAssessments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Subdistrict> Subdistricts { get; set; }
    public DbSet<Training> Trainings { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.Entity<TrainingCategory>().HasKey(tc => new { tc.TrainingId, tc.CategoryId });
        //builder.Entity<TrainingCategory>().
        //    HasOne(tc => tc.Training).WithMany(tc => tc.TrainingCategories).HasForeignKey(t => t.TrainingId);
        //builder.Entity<TrainingCategory>().
        //    HasOne(tc => tc.Category).WithMany(tc => tc.TrainingCategories).HasForeignKey(p => p.CategoryId);
        //base.OnModelCreating(builder);

        //builder.Entity<Enrollment>().
        //    HasOne(tc => tc.Training).WithMany(tc => tc.TrainingCategories).HasForeignKey(t => t.TrainingId);
        //builder.Entity<Enrollment>().
        //    HasOne(tc => tc.Category).WithMany(tc => tc.TrainingCategories).HasForeignKey(p => p.CategoryId);
        
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and overrIde the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

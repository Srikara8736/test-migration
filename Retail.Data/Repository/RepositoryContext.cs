using Microsoft.EntityFrameworkCore;
using Retail.Data.Entities.Common;
using Retail.Data.Entities.Customers;
using Retail.Data.Entities.FileSystem;
using Retail.Data.Entities.Projects;
using Retail.Data.Entities.Stores;
using Retail.Data.Entities.UserAccount;

namespace Retail.Data.Repository;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions options)
           : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from app settings
    }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<CodeMaster> CodeMasters { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<CustomerProject> CustomerProjects { get; set; }
    public DbSet<AreaType> AreaTypes { get; set; }
    public DbSet<AreaTypeGroup> AreaTypeGroups { get; set; }
    public DbSet<Category> Categories { get; set; }
   
    public DbSet<Space> Spaces { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreData> StoreDatas { get; set; }
    public DbSet<StoreDocument> StoreDocuments { get; set; }
    public DbSet<StoreCategoryAreaTypeGroup> CategoryAreaTypeGroups { get; set; }
    public DbSet<StoreImage> StoreImages { get; set; }
    public DbSet<StoreSpace> StoreSpaces { get; set; }
    public DbSet<StoreProject> StoreProjects { get; set; }
    public DbSet<StoreCategoryAreaTypeGroup> StoreCategoryAreaTypeGroups { get; set; }  




    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
        .HasOne(x => x.Customer)
        .WithMany()
        .HasForeignKey(x => x.CustomerId)
        .IsRequired(false);


        builder.Entity<Category>()
            .HasOne(x => x.AreaType)
            .WithMany()
            .HasForeignKey(x => x.AreaTypeId)
            .IsRequired(false);


        builder.Entity<StoreData>()
           .HasOne(x => x.Document)
           .WithMany()
           .HasForeignKey(x => x.DocumentId)
           .OnDelete(DeleteBehavior.ClientSetNull)
           .IsRequired(false);


        builder.Entity<StoreCategoryAreaTypeGroup>()
           .HasOne(x => x.Space)
           .WithMany()
           .HasForeignKey(x => x.SpaceId)
           .OnDelete(DeleteBehavior.ClientSetNull)
           .IsRequired(false);


        builder.Entity<StoreSpace>()
            .HasOne(x => x.Space)
            .WithMany()
            .HasForeignKey(x => x.SpaceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .IsRequired(false);

        builder.Entity<StoreData>()
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<StoreDocument>()
            .HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<Store>()
           .HasOne(x => x.Customer)
           .WithMany()
           .HasForeignKey(x => x.CustomerId)
           .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<StoreSpace>()
           .HasOne(x => x.Space)
           .WithMany()
           .HasForeignKey(x => x.SpaceId)
           .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Entity<StoreSpace>()
           .HasOne(x => x.StoreData)
           .WithMany()
           .HasForeignKey(x => x.StoreDataId)
           .OnDelete(DeleteBehavior.ClientSetNull);


        builder.Entity<Customer>()
         .HasOne(x => x.Image)
         .WithMany()
         .HasForeignKey(x => x.LogoImageId)
         .OnDelete(DeleteBehavior.ClientSetNull)
         .IsRequired(false);

        base.OnModelCreating(builder);
    }
}

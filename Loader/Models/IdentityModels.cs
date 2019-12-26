using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Loader.Hubs;

namespace Loader.Models
{

    public class AppUserRole : IdentityUserRole<int> { }
    public class AppUserClaim : IdentityUserClaim<int> { }
    public class AppUserLogin : IdentityUserLogin<int> { }

    public class Role : IdentityRole<int, AppUserRole>
    {
        public Role() { }
        public Role(string name) { Name = name; }

        //Extended Properties
        public int DGId { get;  set; }
        public int MTId { get; set; }


        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(RoleManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    userIdentity.AddClaim(new Claim("DesignationId", this.DGId));

        //    return userIdentity;
        //}

    }

    public class AppUserStore : UserStore<ApplicationUser, Role, int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public AppUserStore(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class AppRoleStore : RoleStore<Role, int, AppUserRole>
    {
        public AppRoleStore(ApplicationDbContext context) : base(context)
        { }
    } 
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public System.DateTime? ActiveUntil;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            EmployeeId = 0;
            IsActive = true;
            IsUnlimited = true;
            MTId = 0;
            return userIdentity;
        }

        public int MTId { get; set; }
        //public Nullable<int> DGId { get; set; }
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<DateTime> EffDate { get; set; }
        public Nullable<DateTime> TillDate { get; set; }
        public bool IsUnlimited { get; set; }
        public bool IsActive { get; set; }
        public Nullable<byte> Image { get; set; }
        public int UserDesignationId { get; set; }

    }
    public class ApplicationRole : IdentityRole<int, AppUserRole>
    {
        public Nullable<int> DGId { get; set; }
        public int MTId { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role,int, AppUserLogin, AppUserRole, AppUserClaim>
    {
        public ApplicationDbContext()
            : base("MainConnection")
        {
        }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<Datatype> DataType { get; set; }
        public DbSet<Param> Param { get; set; }
        public DbSet<ParamValue> ParamValue { get; set; }
        public DbSet<ParamScript> ParamScript { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<MenuTemplate> MenuTemplate { get; set; }
        public DbSet<MenuVsTemplate> MenuVsTemplate { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<BloodGroup> BloodGroup { get; set; }
        public DbSet<Religion> Religion { get; set; }
        public DbSet<Nationality> Nationality { get; set; }
        public DbSet<MaritialStatus> MaritialStatus { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<UserVSBranch> UserVSBranch { get; set; }
        public DbSet<LoginLogs> LoginLogs { get; set; }
        public DbSet<FiscalYear> FiscalYear { get; set; }
        public DbSet<UserConnection> UserConnection { get; set; }
        public DbSet<LicenseBranch> LicenseBranch { get; set; }



        public static ApplicationDbContext Create()
        {            
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            
            modelBuilder.Entity<AppUserLogin>().Map(c =>
            {
                c.ToTable("UserLogin","LG");
                c.Properties(p => new
                {
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });

            // Mapping for ApiRole
            modelBuilder.Entity<Role>().Map(c =>
            {
                c.ToTable("Role","LG");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                   p.DGId,
                    p.Name,
                    p.MTId
                });
            }).HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);

            modelBuilder.Entity<ApplicationUser>().Map(c =>
            {
                c.ToTable("User","LG");
                c.Property(p => p.Id).HasColumnName("UserId");
                c.Properties(p => new
                {
                    
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.UserName,
                    p.EmployeeId,
                    p.EffDate,
                    p.TillDate,
                    p.IsUnlimited,
                    p.IsActive,
                    p.Image,
                    p.MTId,
                    p.UserDesignationId
                    
                });
            }).HasKey(c => c.Id);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<ApplicationUser>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);

            modelBuilder.Entity<AppUserRole>().Map(c =>
            {
                c.ToTable("UserRole","LG");
                c.Properties(p => new
                {
                    
                    p.UserId,
                    p.RoleId
                });
            })
            .HasKey(c => new { c.UserId, c.RoleId });

            modelBuilder.Entity<AppUserClaim>().Map(c =>
            {
                c.ToTable("UserClaim","LG");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType
                });
            }).HasKey(c => c.Id);


            modelBuilder.Entity<Menu>().ToTable("MenuMain", "LG");
           // modelBuilder.Entity<LicenseBranch>().ToTable("LicenseBranch", "fin");

        }
    }
}
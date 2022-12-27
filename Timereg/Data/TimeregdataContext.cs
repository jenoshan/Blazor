using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Timereg.Models.Timeregdata;

namespace Timereg.Data
{
    public partial class TimeregdataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public TimeregdataContext(DbContextOptions<TimeregdataContext> options) : base(options)
        {
        }

        public TimeregdataContext()
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Timereg.Models.Timeregdata.VTimeused>().HasNoKey();
            builder.Entity<Timereg.Models.Timeregdata.VUserWithTime>().HasNoKey();
            builder.Entity<Timereg.Models.Timeregdata.VTimeusedPeruser>().HasNoKey();
            builder.Entity<Timereg.Models.Timeregdata.VUserProject>().HasNoKey();
            builder.Entity<Timereg.Models.Timeregdata.Invoiceline>().HasKey(vf => new { vf.invoiceid, vf.invoicelineid });
            builder.Entity<Timereg.Models.Timeregdata.Timeused>()
                  .HasOne(i => i.Project)
                  .WithMany(i => i.Timeuseds)
                  .HasForeignKey(i => i.projectid)
                  .HasPrincipalKey(i => i.id);
            builder.Entity<Timereg.Models.Timeregdata.Timeused>()
                  .HasOne(i => i.Employee)
                  .WithMany(i => i.Timeuseds)
                  .HasForeignKey(i => i.userid)
                  .HasPrincipalKey(i => i.userid);
            builder.Entity<Timereg.Models.Timeregdata.UserProject>()
                  .HasOne(i => i.Project)
                  .WithMany(i => i.UserProjects)
                  .HasForeignKey(i => i.projectid)
                  .HasPrincipalKey(i => i.id);
            builder.Entity<Timereg.Models.Timeregdata.UserProject>()
                  .HasOne(i => i.Employee)
                  .WithMany(i => i.UserProjects)
                  .HasForeignKey(i => i.userid)
                  .HasPrincipalKey(i => i.userid);

            builder.Entity<Timereg.Models.Timeregdata.Project>()
                .HasOne(i => i.company)
                .WithMany(i => i.Project)
                .HasForeignKey(i => i.companyid)
                .HasPrincipalKey(i => i.companyid);

            builder.Entity<Timereg.Models.Timeregdata.Invoice>()
             .HasOne(i => i.Company)
             .WithMany(i => i.Invoice)
             .HasForeignKey(i => i.companyid)
             .HasPrincipalKey(i => i.companyid);

            builder.Entity<Timereg.Models.Timeregdata.Project>()
                  .Property(p => p.validfrom)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Project>()
                  .Property(p => p.validto)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Timeused>()
                  .Property(p => p.workdate)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Timeused>()
                  .Property(p => p.timefrom)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Timeused>()
                  .Property(p => p.timeto)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.VTimeused>()
                  .Property(p => p.workdate)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.VTimeused>()
                  .Property(p => p.timefrom)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.VTimeused>()
                  .Property(p => p.timeto)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.VUserProject>()
                  .Property(p => p.validfrom)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.VUserProject>()
                  .Property(p => p.validto)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Invoice>()
                .Property(p => p.updateddate)
                .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Invoice>()
                  .Property(p => p.createddate)
                  .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Invoice>()
                 .Property(p => p.invoicedate)
                 .HasColumnType("datetime");

            builder.Entity<Timereg.Models.Timeregdata.Invoice>()
                 .Property(p => p.duedate)
                 .HasColumnType("datetime");

            this.OnModelBuilding(builder);
        }


        public DbSet<Timereg.Models.Timeregdata.Employee> Employees
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.Project> Projects
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.Timeused> Timeuseds
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.UserProject> UserProjects
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.VTimeused> VTimeuseds
        {
            get;
            set;
        }
        public DbSet<Timereg.Models.Timeregdata.VUserWithTime> VUserWithTimes
        {
            get;
            set;
        }
        public DbSet<Timereg.Models.Timeregdata.VTimeusedPeruser> VTimeusedPerusers
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.VUserProject> VUserProjects
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.Invoice> Invoice
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.Invoiceline> Invoiceline
        {
            get;
            set;
        }

        public DbSet<Timereg.Models.Timeregdata.Company> Company
        {
            get;
            set;
        }
    }
}

using EventApi.Domain;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EventApi.Data
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions options):
            base(options)
        {
        }

        public DbSet<EventItem> EventItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Format> Formats { get; set; }

        protected override void OnModelCreating
           (ModelBuilder builder)
        {
            builder.Entity<EventItem>(ConfigureEventItem);
            builder.Entity<Category>(ConfigureCategory);
            builder.Entity<Venue>(ConfigureVenue);
            builder.Entity<Organization>(ConfigureOrganization);
            builder.Entity<Logo>(ConfigureLogo);
            builder.Entity<Address>(ConfigureAddress);
            builder.Entity<Format>(ConfigureFormat);

           
        }

        private void ConfigureEventItem
            (EntityTypeBuilder<EventItem> builder)
        {
            builder.ToTable("Events");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("catalog_hilo")
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired();
            builder.Property(c => c.Summary)
                .IsRequired(false);
            builder.Property(c => c.Description)
                .IsRequired(false);
            builder.Property(c => c.Start)
                .IsRequired();
            builder.Property(c => c.End)
                .IsRequired();
            builder.Property(c => c.Created)
                .IsRequired();
            builder.Property(c => c.Changed)
                .IsRequired();
            builder.Property(c => c.Published)
                .IsRequired();
            builder.Property(c => c.Status)
                .IsRequired(false);
            builder.HasOne(c => c.Category)
               .WithMany()
               .HasForeignKey(c => c.CategoryId);
            builder.HasOne(c => c.Venue)
                   .WithMany()
                   .HasForeignKey(c => c.VenueId);
            builder.HasOne(c => c.Logo)
                   .WithMany()
                   .HasForeignKey(c => c.LogoId);
            builder.HasOne(c => c.Organization)
                   .WithMany()
                   .HasForeignKey(c => c.OrganizationId);
            builder.HasOne(c => c.Format)
                   .WithMany()
                   .HasForeignKey(c => c.FormatId);

        }
        private void ConfigureCategory
           (EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("category_hilo")
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.CategoryResourceUrl)
              .IsRequired();
        }
        private void ConfigureVenue
           (EntityTypeBuilder<Venue> builder)
        {
            builder.ToTable("Venue");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("venue_hilo")
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired();
            builder.Property(c => c.Capacity)
               .IsRequired();
            builder.Property(c => c.VenueResourceUrl)
              .IsRequired();
            builder.HasOne(c => c.Address)
                  .WithMany()
                  .HasForeignKey(c => c.AddressId);
        }
        private void ConfigureOrganization
           (EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organization");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("organization_hilo")
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureLogo
           (EntityTypeBuilder<Logo> builder)
        {
            builder.ToTable("Logo");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("logo_hilo")
                .IsRequired();
            builder.Property(c => c.LogoUrl)
                .IsRequired();
        }

        private void ConfigureAddress
           (EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("address_hilo")
                .IsRequired();
            builder.Property(c => c.Address_1)
               .IsRequired();
            builder.Property(c => c.Address_2)
               .IsRequired(false);
            builder.Property(c => c.City)
               .IsRequired();
            builder.Property(c => c.State)
               .IsRequired();
            builder.Property(c => c.Postal_Code)
               .IsRequired();
            builder.Property(c => c.Country)
                .IsRequired();
            builder.Property(c => c.Latitude)
                .IsRequired(false);
            builder.Property(c => c.Longitude)
                .IsRequired(false);

        }

        private void ConfigureFormat
           (EntityTypeBuilder<Format> builder)
        {
            builder.ToTable("Format");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("format_hilo")
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.FormatResourceUrl)
              .IsRequired();
        }
    }

}



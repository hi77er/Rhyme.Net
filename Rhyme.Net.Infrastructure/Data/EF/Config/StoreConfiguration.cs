using Rhyme.Net.Core.Domain.StoreAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhyme.Net.Infrastructure.Data.EF.Config;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
  public void Configure(EntityTypeBuilder<Store> builder)
  {
    builder.ToTable("Stores");

    builder.Property(x => x.Id)
      .ValueGeneratedOnAdd()
      .HasColumnName("Id")
      .IsRequired();

    builder.Property(m => m.Id)
      .ValueGeneratedOnAdd()
      .HasColumnName("Id")
      .IsRequired();

    builder.Property(m => m.MenuId)
      .HasColumnName("MenuId");

    builder.Property(m => m.Name)
      .HasColumnName("Name")
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.Property(m => m.WorkingHours)
      .HasColumnName("WorkingHours");

    builder.OwnsMany(m => m.Facilities, facilityBuilder =>
    {
      facilityBuilder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasColumnName("Id")
        .IsRequired();

      facilityBuilder.WithOwner().HasForeignKey("StoreId");

      facilityBuilder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

      facilityBuilder.Property(x => x.Description)
        .HasColumnName("Description")
        .HasMaxLength(DataSchemaConstants.DEFAULT_DESCRIPTION_LENGTH)
        .IsRequired(false);

      facilityBuilder.Property(x => x.Size)
        .HasColumnName("Size")
        .IsRequired();

      facilityBuilder.Property(x => x.Category)
        .HasColumnName("Category")
        .IsRequired();

      facilityBuilder.Property(x => x.Status)
        .HasConversion(
          x => x.Value,
          x => FacilityStatus.FromValue(x));
    });

    builder.OwnsMany(m => m.Channels, channelBuilder =>
    {
      channelBuilder.WithOwner();

      channelBuilder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

      channelBuilder.Property(x => x.Value)
        .HasConversion(
          x => x,
          x => Channel.FromValue(x));
    });
  }
}

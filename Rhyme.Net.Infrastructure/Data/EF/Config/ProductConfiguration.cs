using Rhyme.Net.Core.Domain.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhyme.Net.Infrastructure.Data.EF.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
  public void Configure(EntityTypeBuilder<Product> builder)
  {
    builder.ToTable("Products");

    builder.Property(x => x.Id)
      .ValueGeneratedOnAdd()
      .HasColumnName("Id")
      .IsRequired();

    builder.Property(m => m.Id)
      .ValueGeneratedOnAdd()
      .HasColumnName("Id")
      .IsRequired();

    builder.Property(m => m.Name)
      .HasColumnName("Name")
      .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
      .IsRequired();

    builder.Property(m => m.Description)
      .HasColumnName("Description")
      .HasMaxLength(DataSchemaConstants.DEFAULT_DESCRIPTION_LENGTH);

    builder.OwnsMany(m => m.Allergens, allergenBuilder =>
    {
      allergenBuilder.WithOwner();

      allergenBuilder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasColumnName("Id")
        .IsRequired();

      allergenBuilder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

      allergenBuilder.Property(x => x.ShortName)
        .HasColumnName("ShortName")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired(false);

      allergenBuilder.Property(x => x.Number)
        .HasColumnName("Number");

      allergenBuilder.Property(x => x.Status)
        .HasColumnName("Status");

      allergenBuilder.Property(x => x.SourcingFacilityId)
        .HasColumnName("SourcingFacilityId");

      allergenBuilder.Property(x => x.ShelfLife)
        .HasColumnName("ShelfLife");

      allergenBuilder.Property(x => x.ShelfLifeOnceOpened)
        .HasColumnName("ShelfLifeOnceOpened");

      allergenBuilder.Property(x => x.AllergensContainType)
        .HasConversion(
          x => x.Value,
          x => AllergensContainType.FromValue(x));

      allergenBuilder.Property(x => x.SensitivitiesContainType)
        .HasConversion(
          x => x.Value,
          x => SensitivitiesContainType.FromValue(x));

      allergenBuilder.OwnsMany(x => x.CompliesWith, compliesWithBuilder =>
      {
        compliesWithBuilder.WithOwner();

        compliesWithBuilder.Property(x => x.Name)
          .HasColumnName("Name")
          .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
          .IsRequired();

        compliesWithBuilder.Property(x => x.Value)
          .HasConversion(
            x => x,
            x => ComplyWith.FromValue(x));
      });

    });

    builder.OwnsMany(m => m.Nutritions, nutriBuilder =>
    {
      nutriBuilder.WithOwner();

      nutriBuilder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasColumnName("Id")
        .IsRequired();

      nutriBuilder.Property(x => x.Name)
        .HasColumnName("Name")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
        .IsRequired();

      nutriBuilder.Property(x => x.ShortName)
        .HasColumnName("ShortName")
        .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH);

      nutriBuilder.Property(x => x.Number)
        .HasColumnName("Number");

      nutriBuilder.Property(x => x.Status)
        .HasColumnName("Status");

      nutriBuilder.Property(x => x.Per100Grams)
        .HasColumnName("Per100Grams");

      nutriBuilder.Property(x => x.Per100Milliliters)
        .HasColumnName("Per100Milliliters");

      nutriBuilder.Property(x => x.PerServing)
        .HasColumnName("PerServing");

      nutriBuilder.Property(x => x.ServingSize)
        .HasColumnName("ServingSize");
    });
  }
}

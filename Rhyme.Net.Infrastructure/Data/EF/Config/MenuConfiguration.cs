using Rhyme.Net.Core.Domain.MenuAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhyme.Net.Infrastructure.Data.EF.Config;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
  public void Configure(EntityTypeBuilder<Menu> builder)
  {
    builder.ToTable("Menus");
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Id)
      .ValueGeneratedOnAdd()
      .HasColumnName("Id")
      .IsRequired();

    builder.Property(m => m.StoreId)
      .HasColumnName("StoreId")
      .IsRequired();

    builder.Property(m => m.Title)
      .HasColumnName("Title")
      .HasMaxLength(DataSchemaConstants.DEFAULT_TITLE_LENGTH)
      .IsRequired();

    builder.OwnsMany(m => m.Brochures, brochureBuilder =>
    {
      brochureBuilder.HasKey(x => x.Id);

      brochureBuilder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasColumnName("Id")
        .IsRequired();

      brochureBuilder.WithOwner().HasForeignKey("MenuId");

      brochureBuilder.OwnsMany(b => b.MenuItems, menuItemBuilder =>
      {
        menuItemBuilder.HasKey(x => x.Id);

        menuItemBuilder.Property(x => x.Id)
          .ValueGeneratedOnAdd()
          .HasColumnName("Id")
          .IsRequired();

        menuItemBuilder.WithOwner().HasForeignKey("BrochureId");

        menuItemBuilder.Ignore(mi => mi.Product);

        menuItemBuilder.OwnsOne(mv => mv.DefaultPricing, pricingBuilder =>
            {
              pricingBuilder.HasKey(x => x.Id);

              pricingBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

              pricingBuilder.Property(x => x.Status)
                .HasConversion(
                  x => x.Value,
                  x => PricingStatus.FromValue(x));
            });

        menuItemBuilder.OwnsMany(mi => mi.AddOns, addOnBuilder =>
        {
          addOnBuilder.HasKey(x => x.Id);

          addOnBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

          addOnBuilder.WithOwner();

          addOnBuilder.Property(x => x.MeasureType)
            .HasConversion(
              x => x.Value,
              x => AddOnMeasureType.FromValue(x));

          addOnBuilder.Ignore(ao => ao.Product);

          addOnBuilder.OwnsOne(mv => mv.AddOnPricing, pricingBuilder =>
            {
              pricingBuilder.HasKey(x => x.Id);

              pricingBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

              pricingBuilder.Property(x => x.Status)
                .HasConversion(
                  x => x.Value,
                  x => PricingStatus.FromValue(x));
            });
        });

        menuItemBuilder.OwnsMany(mi => mi.Modifiers, modifierBuilder =>
        {
          modifierBuilder.HasKey(x => x.Id);

          modifierBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

          modifierBuilder.WithOwner();

          modifierBuilder.Property(x => x.DefaultOption)
            .HasConversion(
              x => x.Value,
              x => ModifierOption.FromValue(x));

          modifierBuilder.Property(x => x.ModifierType)
            .HasConversion(
              x => x.Value,
              x => ModifierType.FromValue(x));

          modifierBuilder.OwnsMany(mo => mo.Variants, variantBuilder =>
          {
            variantBuilder.HasKey(x => x.Id);

            variantBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

            variantBuilder.WithOwner().HasForeignKey("MenuItemId");

            variantBuilder.Property(x => x.MenuItemId);

            variantBuilder.Property(x => x.ModifierType)
              .HasConversion(
                x => x.Value,
                x => ModifierType.FromValue(x));

            variantBuilder.Property(mv => mv.Option)
              .HasConversion(
                x => x.Value,
                x => ModifierOption.FromValue(x));

            variantBuilder.OwnsOne(mv => mv.VariantPricing, pricingBuilder =>
            {
              pricingBuilder.HasKey(x => x.Id);

              pricingBuilder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id")
                .IsRequired();

              pricingBuilder.Property(x => x.Status)
                .HasConversion(
                  x => x.Value,
                  x => PricingStatus.FromValue(x));
            });
          });
        });
      });
    });

    builder.Property(p => p.Title)
        .HasMaxLength(DataSchemaConstants.DEFAULT_TITLE_LENGTH)
        .IsRequired();
  }
}

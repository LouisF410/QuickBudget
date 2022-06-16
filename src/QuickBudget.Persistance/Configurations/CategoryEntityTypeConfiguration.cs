using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickBudget.Domain.Entities;

namespace QuickBudget.Persistance.Configurations
{
    internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(x => x.Id)
                .HasName("CategoryId");

            builder.HasMany(x => x.Budgets)
                .WithOne(x => x.Category);

            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.Category);
        }
    }
}

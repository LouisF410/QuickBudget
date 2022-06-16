using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickBudget.Domain.Entities;

namespace QuickBudget.Persistance.Configurations
{
    internal class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets");
            builder.HasKey(x => x.Id)
                .HasName("BudgetId");

            builder.Property(x => x.Amount)
                .HasPrecision(8, 2)
                .HasDefaultValue(0);

            builder.HasOne(x => x.BudgetPeriod)
                .WithMany(x => x.Budgets);
        }
    }
}

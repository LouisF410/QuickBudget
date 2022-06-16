using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickBudget.Domain.Entities;

namespace QuickBudget.Persistance.Configurations
{
    internal class BudgetPeriodEntityTypeConfiguration : IEntityTypeConfiguration<BudgetPeriod>
    {
        public void Configure(EntityTypeBuilder<BudgetPeriod> builder)
        {
            builder.ToTable("BudgetPeriods");
            builder.HasKey(x => x.Id)
                .HasName("BudgetPeriodId");

            builder.HasMany(x => x.Transactions)
                .WithMany(x => x.BudgetPeriods)
                .UsingEntity("BudgetPeriodTransactions");
        }
    }
}

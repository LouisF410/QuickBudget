using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickBudget.Domain.Entities;

namespace QuickBudget.Persistance.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(x => x.Id)
                .HasName("TransactionId");
            builder.Property(x => x.Amount).HasPrecision(8, 2);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Transactions);
        }
    }
}

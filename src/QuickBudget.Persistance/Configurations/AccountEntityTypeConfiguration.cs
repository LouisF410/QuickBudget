using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickBudget.Domain.Entities;

namespace QuickBudget.Persistance.Configurations
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(x => x.Id)
                .HasName("AccountId");
            builder.Property(x => x.AccountNumber).IsRequired();

            builder.OwnsOne(x => x.Bank, nameBuilder =>
            {
                nameBuilder.Property(x => x.BankId).HasColumnName("BankId").IsRequired();
                nameBuilder.Property(x => x.BankName).HasColumnName("BankName").IsRequired();
            });
        }
    }
}

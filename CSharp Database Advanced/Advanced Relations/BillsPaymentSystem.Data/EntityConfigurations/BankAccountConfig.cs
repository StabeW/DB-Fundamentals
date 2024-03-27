using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.HasKey(b => b.BankAccountId);

            builder
                .Property(b => b.Balance)
                .IsRequired();

            builder
                .Property(b => b.BankName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(b => b.SWIFT)
                .IsUnicode(false)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}

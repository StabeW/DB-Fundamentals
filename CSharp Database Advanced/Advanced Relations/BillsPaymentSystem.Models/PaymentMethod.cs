using BillsPaymentSystem.Data.Common.Contracts;
using BillsPaymentSystem.Models.Attribute;
using BillsPaymentSystem.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models
{
    public class PaymentMethod : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public PaymentMethodType Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Xor(nameof(BankAccountId))]
        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}

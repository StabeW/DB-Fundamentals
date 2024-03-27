using BillPaymentSystem.Models.Attributes;
using BillsPaymentSystem.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillsPaymentSystem.Models
{
    public class CreditCard : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int CreditCardId { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        [Column(TypeName = "Money")]
        public decimal Limit { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        [Column(TypeName = "Money")]
        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => Limit - MoneyOwed;

        [Expiration]
        public DateTime ExpirationData { get; set; }

        public PaymentMethod PaymentMethods { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}

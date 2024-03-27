using BillsPaymentSystem.Data.Common.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillsPaymentSystem.Models
{
    public class BankAccount : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int BankAccountId { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        [Column(TypeName = "Money")]
        public decimal Balance { get; set; }

        [MinLength(6), MaxLength(50)]
        [Required]
        public string BankName { get; set; }

        [MinLength(3), MaxLength(20)]
        [Required]
        [Unicode(false)]
        public string SWIFT { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}

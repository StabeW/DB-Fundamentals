using BillsPaymentSystem.Data.Common.Contracts;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models
{
    public class User : IAuditInfo, IDeletableEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
        [MinLength(10), MaxLength(80)]
        public string Email { get; set; }

        [Required]
        [MinLength(6), MaxLength(50)]
        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new HashSet<PaymentMethod>();
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}

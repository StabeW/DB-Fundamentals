using System.ComponentModel.DataAnnotations;
using BillsPaymentSystem.Data.Common.Contracts;

namespace BillsPaymentSystem.Data.Common
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
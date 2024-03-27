using System.ComponentModel.DataAnnotations;
using BillsPaymentSystem.App.Common.Contracts;

namespace BillsPaymentSystem.App.Common
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
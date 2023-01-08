using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Models
{
    public enum OrderStateEnum 
    {
        Pending,
        Processing,
        Completed,
        Declined,
        Cancelled,
        Refund
    }
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDate { get; set; }
        public string TransactionMetaData { get; set; }
    }
}

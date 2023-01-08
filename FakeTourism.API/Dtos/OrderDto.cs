using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> OrderItems { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
        public string TransactionMetaData { get; set; }
    }
}

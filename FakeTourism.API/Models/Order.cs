using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Stateless;

namespace FakeTourism.API.Models
{
    public enum OrderStateEnum 
    {
        Pending, //order generated
        Processing, //Order is under processing
        Completed, //Order successfully
        Declined, //order failed
        Cancelled, //order cancelled
        Refund //refund by user
    }

    public enum OrderStateTriggerEnum 
    {
        PlaceOrder, //process payment
        Approve, //payment success
        Reject, //payment failed
        Cancel, //cancel the payment
        Return //return the purchased goods
    }
    public class Order
    {
        public Order() 
        {
            StateMachineInit();
        }

        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<LineItem> OrderItems { get; set; }
        public OrderStateEnum State { get; set; }
        public DateTime CreateDate { get; set; }
        public string TransactionMetaData { get; set; }
        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;

        public void PaymentProcessing() 
        {
            _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
        }

        public void PaymentApproved() 
        {
            _machine.Fire(OrderStateTriggerEnum.Approve);
        }

        public void PaymentRejected() 
        {
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

        private void StateMachineInit() 
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>
                (() => State, s => State = s);
            
            _machine.Configure(OrderStateEnum.Pending)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }
    }
}

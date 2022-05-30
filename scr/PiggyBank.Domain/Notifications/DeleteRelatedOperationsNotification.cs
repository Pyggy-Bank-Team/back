using MediatR;

namespace PiggyBank.Domain.Notifications
{
    public class DeleteRelatedOperationsNotification : INotification
    {
        public int AccountId { get; set; }
    }
}
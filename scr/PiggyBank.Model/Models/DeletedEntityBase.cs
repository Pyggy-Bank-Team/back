
namespace PiggyBank.Model.Models
{
    public abstract class DeletedEntityBase : EntityModifiedBase
    {
        public bool IsDeleted { get; set; }

        public bool IsArchived { get; set; }
    }
}

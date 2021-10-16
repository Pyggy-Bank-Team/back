using Microsoft.AspNetCore.Identity;
using PiggyBank.Common.Interfaces.Models;

namespace Identity.Model.Models
{
    public class ApplicationUser : IdentityUser, IBaseModel
    {
        public string CurrencyBase { get; set; }
        public long? ChatId { get; set; }
    }
}

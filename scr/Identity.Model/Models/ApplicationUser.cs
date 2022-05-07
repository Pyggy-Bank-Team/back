using Microsoft.AspNetCore.Identity;

namespace Identity.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CurrencyBase { get; set; }
        public long? ChatId { get; set; }
    }
}

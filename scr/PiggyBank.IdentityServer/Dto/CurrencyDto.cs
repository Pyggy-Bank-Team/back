using System.ComponentModel.DataAnnotations;

namespace PiggyBank.IdentityServer.Dto
{
    public class CurrencyDto
    {
        [Required]
        public string PreviousCurrency { get; set; }

        [Required]
        public string NewCurrency { get; set; }
    }
}

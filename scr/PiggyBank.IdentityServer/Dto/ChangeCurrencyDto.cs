using System.ComponentModel.DataAnnotations;

namespace PiggyBank.IdentityServer.Dto
{
    public class ChangeCurrencyDto
    {
        [Required]
        public string PreviousCurrency { get; set; }

        [Required]
        public string NewCurrency { get; set; }
    }
}

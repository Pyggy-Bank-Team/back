using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class TransferOperationRequestBase
    {
        [Range(1, int.MaxValue)]
        public int From { get; set; }

        [Range(1, int.MaxValue)]
        public int To { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}
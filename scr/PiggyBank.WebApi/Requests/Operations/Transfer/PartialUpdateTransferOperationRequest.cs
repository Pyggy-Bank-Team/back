using System;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class PartialUpdateTransferOperationRequest
    {
        [Range(1, int.MaxValue)]
        public int? From { get; set; }

        [Range(1, int.MaxValue)]
        public int? To { get; set; }

        public decimal? Amount { get; set; }

        public string Comment { get; set; }

        public DateTime? OperationDate { get; set; }
    }
}
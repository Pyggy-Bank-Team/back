﻿using System.ComponentModel.DataAnnotations;

namespace PiggyBank.WebApi.Requests.Operations.Transfer
{
    public class PartialUpdateTransferOperationRequest
    {
        [Range(1, 2)]
        public int? From { get; set; }

        [Range(1, 2)]
        public int? To { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        public string Comment { get; set; }
    }
}
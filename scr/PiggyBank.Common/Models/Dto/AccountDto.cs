﻿using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Models.Dto
{
    public class AccountDto : BaseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public AccountType Type { get; set; }
        
        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsArchived { get; set; }
    }
}

using System;

namespace PiggyBank.Model.Models.Entities
{
    public class AppSettings : EntityModifiedBase
    {
        public Guid UserId { get; set; }
        public string BaseCurrency { get; set; }
        public string Locale { get; set; }
        public long? TelegramChatId { get; set; }
    }
}
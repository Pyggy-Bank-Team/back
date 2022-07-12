using System;

namespace Common.Results.Models.Dto
{
    public class AppSettingsDto
    {
        public Guid UserId { get; set; }
        public string BaseCurrency { get; set; }
        public string Locale { get; set; }
        public long? TelegramChatId { get; set; }
    }
}
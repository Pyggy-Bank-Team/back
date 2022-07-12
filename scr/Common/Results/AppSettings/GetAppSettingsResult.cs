using Common.Results.Models.Dto;

namespace Common.Results.AppSettings
{
    public class GetAppSettingsResult : BaseResult
    {
        public AppSettingsDto Data { get; set; }
    }
}
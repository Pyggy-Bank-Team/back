using System;
using Common.Results.AppSettings;
using MediatR;

namespace Common.Queries
{
    public class GetAppSettingsQuery : IRequest<GetAppSettingsResult>
    {
        public Guid UserId { get; set; }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Reports;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Requests.Reports;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
            => _service = service;

        [HttpPost, Route("Chart/byCategories")]
        public Task<ChartByCategoryDto[]> GetChartByCategory(GetChartByCategoriesRequest byCategoriesRequest, CancellationToken token)
        {
            var command = new GetChartCommand
            {
                From = byCategoriesRequest.From ?? DateTime.MinValue,
                To = byCategoriesRequest.To ?? DateTime.Now,
                Type = byCategoriesRequest.Type == CategoryType.Undefined ? CategoryType.Expense : byCategoriesRequest.Type,
                UserId = User.GetUserId()
            };

            return _service.GetChartByCategories(command, token);
        }

        [HttpPost, Route("Chart/byExpensePerDays")]
        public Task<ChartByExpensePerDayDto[]> ChartByExpensePerDays(GetChartByExpenseRequest byCategoriesRequest, CancellationToken token)
        {
            var command = new GetChartCommand
            {
                From = byCategoriesRequest.From ?? DateTime.MinValue,
                To = byCategoriesRequest.To ?? DateTime.Now,
                UserId = User.GetUserId()
            };

            return _service.ChartByExpensePerDays(command, token);
        }
    }
}
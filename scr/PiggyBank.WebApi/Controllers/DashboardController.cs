using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Common.Interfaces;
using PiggyBank.Common.Models.Dto.Dashboard;
using PiggyBank.WebApi.Extensions;
using PiggyBank.WebApi.Requests.Dashboard;

namespace PiggyBank.WebApi.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
            => _service = service;

        [HttpPost, Route("Chart/byCategories")]
        public Task<ChartByCategoryDto[]> GetChartByCategory(GetChartByCategoriesRequest request, CancellationToken token)
        {
            var command = new GetChartCommand
            {
                From = request.From ?? DateTime.MinValue,
                To = request.To ?? DateTime.Now,
                UserId = User.GetUserId()
            };

            return _service.GetChartByCategories(command, token);
        }
    }
}
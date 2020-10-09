using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Commands.Dashboard;
using PiggyBank.Model;

namespace PiggyBank.Domain.Queries.Dashboard
{
    public class GetChartByCategoriesQuery : BaseQuery<GetChartCommand>
    {
        private readonly GetChartCommand _command;
        public GetChartByCategoriesQuery(PiggyContext context, GetChartCommand command) : base(context)
        {
            _command = command;
        }

        public override async Task<GetChartCommand> Invoke(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
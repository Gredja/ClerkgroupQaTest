using Microsoft.AspNetCore.Mvc;
using Service;

namespace QATest.Controllers
{
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly DB db;
        private readonly ILogger<BalancesController> logger;

        public BalancesController(DB db,
            ILogger<BalancesController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        [HttpGet("{account}")]
        public async Task<decimal?> Get(int account) => (await db.GetAccountBalance(account))?.Amount;

        [HttpPost("{account}/{amount}")]
        public Task Deposit(int account, decimal amount) => db.DepositToAccount(account, amount);

        [HttpDelete("{account}/{amount}")]
        public Task Get(int account, decimal amount) => db.WithdrawFromAccount(account, amount);
    }
}
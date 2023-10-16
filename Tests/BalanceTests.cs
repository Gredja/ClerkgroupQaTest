using Microsoft.AspNetCore.Mvc.Testing;
using Service;
using Xunit;

namespace Tests
{
    public class BalanceTests
    {
        private MongoDbServiceUnitTestMock _mongoDbServiceUnitTestMock;

        public BalanceTests()
        {
            _mongoDbServiceUnitTestMock = new MongoDbServiceUnitTestMock();
            _mongoDbServiceUnitTestMock.Initialize();
        }

        [Fact]
        public async Task CheckTransferFromOneAccountToAnotherTest()
        {
            var balances = _mongoDbServiceUnitTestMock.GetItems();
            var recipient = balances.First();
            var donor = balances.Last();
            var moneyToTransfer = 10;

            var service = new ServiceHost();
            var client = service.CreateClient();

            Assert.Equal(recipient.Amount, await GetBalanceAsync(client, recipient.Account));
            Assert.Equal(donor.Amount, await GetBalanceAsync(client, donor.Account));

            await client.DeleteAsync($"{donor.Account}/{moneyToTransfer}");
            Assert.Equal(donor.Amount - moneyToTransfer, await GetBalanceAsync(client, donor.Account));

            await client.PostAsync($"{recipient.Account}/{moneyToTransfer}", null);
            Assert.Equal(recipient.Amount + moneyToTransfer, await GetBalanceAsync(client, recipient.Account));
        }

        private async Task<decimal?> GetBalanceAsync(HttpClient client, int account)
        {
            var response = await client.GetAsync(account.ToString());
            var content = await response.Content.ReadAsStringAsync();

            if (decimal.TryParse(content, out var value))
            {
                return value;
            }

            return null;
        }

        private class ServiceHost : WebApplicationFactory<Startup>
        {
        }
    }
}

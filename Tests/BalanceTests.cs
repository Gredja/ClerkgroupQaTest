using Microsoft.AspNetCore.Mvc.Testing;
using Service;
using Xunit;

namespace Tests
{
    public class BalanceTests
    {
        [Fact]
        public async Task CheckTransferFromOneAccountToAnotherTest()
        {
            var service = new ServiceHost();
            var client = service.CreateClient();

            Assert.Equal(0, await Balance(client, 1));

            await client.PostAsync("1/10", null);
            await client.PostAsync("2/30", null);

            Assert.Equal(10, await Balance(client, 1));
            Assert.Equal(30, await Balance(client, 2));

            await client.DeleteAsync("2/5");
            await client.PostAsync("1/5", null);

            Assert.Equal(15, await Balance(client, 1));
            Assert.Equal(25, await Balance(client, 2));
        }


        private async Task<decimal?> Balance(HttpClient client, int account)
        {
            var response = await client.GetAsync(account.ToString());
            var content = await response.Content.ReadAsStringAsync();

            if (decimal.TryParse(content, out var value))
                return value;

            return null;
        }

        private class ServiceHost : WebApplicationFactory<Startup>
        {
        }
    }
}

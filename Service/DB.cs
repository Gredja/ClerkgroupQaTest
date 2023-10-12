using MongoDB.Driver;
using Service.Entities;

namespace Service
{
    /// <summary>
    /// Тип для выполнения зпросов к базе данных.
    /// </summary>
    /// <remarks>
    /// Данный тип не может работать так как не имеет подключения к реальной базе данных.
    /// </remarks>
    public class DB
    {
        MongoClient client;
        ILogger<DB> logger;

        public DB(MongoClient client,
            ILogger<DB> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        private IMongoCollection<Balance> Accounts => client
            .GetDatabase("Accounts")
            .GetCollection<Balance>("Balance");


        /// <summary>
        /// Получение остатка на счёте.
        /// </summary>
        /// <param name="account">Номер счета</param>
        /// <returns>Остаток на счёте</returns>
        public Task<Balance> GetAccountBalance(int account) => Accounts
            .Find(x => x.Account == account)
            .SingleAsync();

        /// <summary>
        /// Пополнение остатка на счёте на заданную сумму.
        /// </summary>
        /// <param name="account">Номер счета</param>
        /// <param name="amount">Сумма пополнения</param>
        /// <returns></returns>
        public Task DepositToAccount(int account, decimal amount) => Accounts
            .FindOneAndUpdateAsync(x => x.Account == account,
                Builders<Balance>.Update.Inc(x => x.Amount, amount));

        /// <summary>
        /// Уменьшение остатка на счёте на заданную сумму.
        /// </summary>
        /// <param name="account">Номер счета</param>
        /// <param name="amount">Сумма снятия</param>
        /// <returns></returns>
        public Task WithdrawFromAccount(int account, decimal amount) => Accounts
            .FindOneAndUpdateAsync(x => x.Account == account,
                Builders<Balance>.Update.Inc(x => x.Amount, -amount));
    }
}

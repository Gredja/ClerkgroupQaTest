using MongoDB.Driver;
using Moq;
using Service.Entities;

namespace Tests
{
    public class MongoDbServiceUnitTestMock
    {
        private readonly Mock<MongoClient> _mongoClientMock;
        private readonly Mock<IMongoDatabase> _mongoDbMock;
        private readonly Mock<IMongoCollection<Balance>> _balancesCollectionMock;
        private List<Balance> _balancesList;
        private readonly Mock<IAsyncCursor<Balance>> _balanceCursorMock;
        private readonly Mock<MongoClientSettings> _settingsMock;

        public MongoDbServiceUnitTestMock()
        {
            _settingsMock = new Mock<MongoClientSettings>();
            _mongoClientMock = new Mock<MongoClient>();
            _balancesCollectionMock = new Mock<IMongoCollection<Balance>>();
            _mongoDbMock = new Mock<IMongoDatabase>();
            _balanceCursorMock = new Mock<IAsyncCursor<Balance>>();

            _balancesList = GetItems();
        }

        public List<Balance> GetItems()
        {
            var recipientBalance = new Balance
            {
                Account = 1,
                Amount = 100
            };

            var donorBalance = new Balance
            {
                Account = 2,
                Amount = 200
            };

            return new List<Balance>
            {
                donorBalance,
                recipientBalance
            };
        }

        public void Initialize()
        {
            InitializeMongoBalanceCollection();
            InitializeMongoDb();
        }

        private void InitializeMongoDb()
        {
            _mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(_mongoDbMock.Object);
            _mongoDbMock.Setup(x => x.GetCollection<Balance>(It.IsAny<string>(), default))
                .Returns(_balancesCollectionMock.Object);

            // Real local BD
            _mongoClientMock.Object.DropDatabase("Accounts");
            var balances = _mongoClientMock.Object.GetDatabase("Accounts").GetCollection<Balance>("Balance");
            balances.InsertMany(GetItems());
        }

        private void InitializeMongoBalanceCollection()
        {
            _balanceCursorMock.Setup(x => x.Current).Returns(_balancesList);
            _balanceCursorMock.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true)
                .Returns(false);
            _balanceCursorMock.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true)).Returns(Task.FromResult(false));
            _balancesCollectionMock
                .Setup(x => x.AggregateAsync(It.IsAny<PipelineDefinition<Balance, Balance>>(),
                    It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_balanceCursorMock.Object);
        }
    }
}

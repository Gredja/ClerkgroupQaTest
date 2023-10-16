using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Service.Entities
{
    /// <summary>
    /// Информация об остатке на счёте.
    /// </summary>
    public class Balance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Номер счёта.
        /// </summary>
        public int Account { get; set; }

        /// <summary>
        /// Текущий остаток на счёте.
        /// </summary>
        public decimal Amount { get; set; }
    }
}

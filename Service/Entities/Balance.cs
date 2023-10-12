namespace Service.Entities
{
    /// <summary>
    /// Информация об остатке на счёте.
    /// </summary>
    public class Balance
    {
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

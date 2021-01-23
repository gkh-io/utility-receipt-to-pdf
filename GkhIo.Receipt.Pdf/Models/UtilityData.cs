namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Информация по коммунальной услуге за расчетный период
    /// </summary>
    public sealed class UtilityData
    {
        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Объём оказанной услуги
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Название единицы измерения
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Тариф. Рублей за единицу измерения
        /// </summary>
        public decimal Tariff { get; set; }

        /// <summary>
        /// Начислено по тарифу
        /// </summary>
        public decimal ChargesByTariff { get; set; }
        /// <summary>
        /// Размер повышающего коэффициента
        /// </summary>
        public decimal? IncreaseCoefficient { get; set; }
        /// <summary>
        /// Размер платы, расчитанной с учётом повышающего коэффициента
        /// </summary>
        public decimal? IncreasePayment { get; set; }
        /// <summary>
        /// Перерасчёт
        /// </summary>
        public decimal? Recalculation { get; set; }
        /// <summary>
        /// Итого
        /// </summary>
        public decimal Total { get; set; }
    }
}
namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Строка справочной информации
    /// </summary>
    public class ReferenceInformationUnit
    {
        /// <summary>
        /// Название услуги
        /// </summary>
        public string UtilityName { get; set; }
        /// <summary>
        /// Индивидуальный или общедомовой прибор учета
        /// </summary>
        public bool PersonalMeter { get; set; }
        /// <summary>
        /// Номер прибора учёта
        /// </summary>
        public string MeterNumber { get; set; }
        /// <summary>
        /// Предыдушее показание
        /// </summary>
        public decimal PreviousValue { get; set; }
        /// <summary>
        /// Текущее показание
        /// </summary>
        public decimal CurrentValue { get; set; }
        /// <summary>
        /// Объем потребления
        /// </summary>
        public decimal Volume { get; set; }
        /// <summary>
        /// Норма потребления
        /// </summary>
        public decimal ConsumptionRate { get; set; }
        /// <summary>
        /// Объем потребления в помещениях дома
        /// </summary>
        public decimal VolumeInHouseAreas { get; set; }
        /// <summary>
        /// Объем потребления на общедомовые нужды
        /// </summary>
        public decimal VolumeCommonHouseUse { get; set; }
    }
}
namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Группа коммунальных услуг
    /// </summary>
    public sealed class UtilityGroup
    {
        /// <summary>
        /// Название группы, например,
        /// "Начисления за содержание жилого помещения"
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Данные по коммунальным услугам из этой группы
        /// </summary>
        public UtilityData[] Utilities { get; set; }
    }
}
namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Данные счётчика
    /// </summary>
    public sealed class Meter
    {
        /// <summary>
        /// Тип счётчика
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Номер счётчика
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Предыдущие показания счётчика
        /// </summary>
        public decimal PreviousValue { get; set; }
        /// <summary>
        /// Дата до которой необходимо провести следующую поверку
        /// </summary>
        public NodaTime.LocalDate Date { get; set; }
    }
}
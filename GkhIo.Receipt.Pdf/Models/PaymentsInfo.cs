namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Информация о платежах
    /// </summary>
    public sealed class PaymentsInfo
    {
        /// <summary>
        /// Начислено за период
        /// </summary>
        public decimal ForPeriod { get; set; }

        /// <summary>
        /// Остаток на начало периода (+/-)
        /// </summary>
        public decimal RestForBeggingOfPeriod { get; set; }

        /// <summary>
        /// Поступления за период
        /// </summary>
        public decimal IncomingInPeriod { get; set; }

        /// <summary>
        /// Дата последнего платежа
        /// </summary>
        public NodaTime.LocalDate? LastPayment { get; set; }

        /// <summary>
        /// Итого к оплате
        /// </summary>
        public decimal TotalPayment { get; set; }

        /// <summary>
        /// Учтены все платежи до указанной даты
        /// </summary>
        public NodaTime.LocalDate PaymentsIncludedUpToDate { get; set; }

        /// <summary>
        /// Текст, расположенный справа от штрихкодов
        /// </summary>
        public string[] RightText { get; set; }

        /// <summary>
        /// Текст, расположенный внизу первой страницы
        /// </summary>
        public string[] BottomText { get; set; }
    }
}
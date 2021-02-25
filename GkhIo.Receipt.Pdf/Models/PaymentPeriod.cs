using System.Globalization;

namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Платежный период
    /// </summary>
    public class PaymentPeriod
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public string GetMonthString() =>
            CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.MonthNames[Month - 1];
        
    }
}
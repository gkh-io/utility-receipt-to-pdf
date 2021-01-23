namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Получатель платежа
    /// </summary>
    public sealed class PaymentReciever
    {
        /// <summary>
        /// Полное название компании получателя платежа,
        /// например, ООО Управляющая компания "Бим Бом"
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Платёжный счет получателя платежа
        /// </summary>
        public string PaymentAccount { get; set; } 

        /// <summary>
        /// Название банка в котором находится счет, например,
        /// "ПАО ПромСвязьБанк"
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Корреспондетский счет получателя платежа
        /// </summary>
        public string CorrespondenceAccount { get; set; }

        /// <summary>
        /// БИК банка получателя платежа
        /// </summary>
        public string BIC { get; set; }

        /// <summary>
        /// ИНН получателя платежа
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// КПП получателя платежа
        /// </summary>
        public string KPP { get; set; }


    }
}
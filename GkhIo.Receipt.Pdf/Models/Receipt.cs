namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Квитанция
    /// </summary>
    public sealed class Receipt
    {
        /// <summary>
        /// Адрес на который выставлена квитанция
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Лицевой счёт плательщика
        /// </summary>
        public string PersonalAccount { get; set; }

        /// <summary>
        /// Данные по счётчикам
        /// </summary>
        public Counter[] Counters { get; set; }

        /// <summary>
        /// ФИО плательщика
        /// </summary>
        public Person Payer { get; set; }
        
        /// <summary>
        /// Номер счёта квитанции
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Платёжный период
        /// </summary>
        public PaymentPeriod PaymentPeriod { get; set; }

        /// <summary>
        /// Оплатить до
        /// </summary>
        public NodaTime.LocalDate PayUntil { get; set; }

        /// <summary>
        /// Информация об объекте оплаты (квартире и доме)
        /// </summary>
        public FlatInfo FlatInfo { get; set; }

        /// <summary>
        /// Начисления за содержание жилого помещения
        /// </summary>
        public UtilityGroup[] UtilityGroups { get; set; }

        /// <summary>
        /// Справочная информация
        /// </summary>
        public ReferenceInformationUnit[] ReferenceInformation { get; set; }
        /// <summary>
        /// QR, соответствующий квитанции
        /// </summary>
        public string QR { get; set; }
        /// <summary>
        /// Дополнительные баркоды квитанции
        /// </summary>
        public string[] Barcodes { get; set; }
    }
}
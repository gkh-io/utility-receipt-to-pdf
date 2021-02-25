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
        public Meter[] Meters { get; set; }

        /// <summary>
        /// ФИО плательщика
        /// </summary>
        public Person Payer { get; set; }
        /// <summary>
        /// Организация получатель платежа
        /// </summary>
        public PaymentReceiver PaymentReceiver { get; set; }
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
        public NodaTime.LocalDate PayUpTo { get; set; }
        /// <summary>
        /// День до которого были учтены платежи за предыдущий расчётный период
        /// </summary>
        public int PaidDay { get; set; }
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
        /// Штрихкод месячного начисления
        /// </summary>
        public string BarcodeForMonth { get; set; }
        /// <summary>
        /// Штрихкод суммарного начисления
        /// </summary>
        public string BarcodeForTotal { get; set; }
        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public decimal Sum { get; set; }
        /// <summary>
        /// Дополнительная информация по платежу через QR
        /// </summary>
        public string QRAdditionalInfo { get; set; }
        /// <summary>
        /// Предыдущие расчёты, учтённые в квитанции
        /// </summary>
        public PaymentsInfo Payments { get; set; }
        /// <summary>
        /// Текст, расположенный справа от штрихкодов
        /// </summary>
        public string[] RightNotes { get; set; }

        /// <summary>
        /// Текст, расположенный внизу первой страницы
        /// </summary>
        public string[] BottomNotes { get; set; }
    }
}
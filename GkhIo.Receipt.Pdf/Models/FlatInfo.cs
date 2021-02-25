namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Информация о квартире и доме, которые оплачиваются
    /// </summary>
    public sealed class FlatInfo
    {
        /// <summary>
        /// Тип квартиры, например,
        /// частная
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Общая площадь квартиры
        /// </summary>
        public decimal CommonArea { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public decimal LivingArea { get; set; }

        /// <summary>
        /// Отапливаемая площадь
        /// </summary>
        public decimal HeatedArea { get; set; }

        /// <summary>
        /// Зарегистрировано
        /// </summary>
        public int PersonsRegistered { get; set; }
        /// <summary>
        /// Общая площадь дома
        /// </summary>
        public decimal HouseArea { get; set; }

        /// <summary>
        /// площадь помещений дома (жилых и не жилых)
        /// </summary>
        public decimal PremisesArea { get; set; }

        /// <summary>
        /// Площадь поещений общего пользования
        /// </summary>
        public decimal AreaOfCommonAreas { get; set; }
    }
}
namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Структурированный адрес, для вывода на квитанции
    /// </summary>
    public sealed class Address
    {
        /// <summary>
        /// полное название населенного пункта,
        /// включая его тип, например г. Реутов
        /// </summary>
        public string CityFull { get; set; }
        /// <summary>
        /// полное название улицы, включая её тип,
        /// например, Юбилейный пр-кт
        /// </summary>
        public string StreetFull { get; set; }
        /// <summary>
        /// полный номер дома, включая его тип,
        /// например, дом 16
        /// </summary>
        public string HouseFull { get; set; }
        /// <summary>
        /// полное название квартиры, включая её тип,
        /// например, кв. 123
        /// </summary>
        public string FlatFull { get; set; }
    }
}
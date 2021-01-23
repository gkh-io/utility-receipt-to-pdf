namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    /// Работа с адресом
    /// </summary>
    public interface IAddressFormatter
    {
        /// <summary>
        /// Форматировать адрес для квитанции
        /// </summary>
        /// <returns></returns>
        string FormatAddressForReceipt(Models.Address address);
    }
}
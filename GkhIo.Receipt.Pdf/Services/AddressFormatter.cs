using System;
using System.Text;
using GkhIo.Receipt.Pdf.Abstract;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Служба для работы с адресом
    /// </summary>
    public class AddressFormatter : IAddressFormatter
    {
        readonly StringBuilder _builder = new StringBuilder();

        /// <inheritdoc />
        public string FormatAddressForReceipt(Models.Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            _builder.Clear();            
            
            AppendAddressPart(address.CityFull);
            AppendAddressPart(address.StreetFull);
            AppendAddressPart(address.HouseFull);
            AppendAddressPart(address.FlatFull);

            return _builder.ToString();
        }

        /// <summary>
        /// Добавляем часть адреса к адресной строке
        /// </summary>
        /// <param name="part"></param>
        private void AppendAddressPart(string part)
        {
            if (part == null) return;

            if (_builder.Length > 0)
            {
                _builder.Append(", ");
            }

            _builder.Append(part);
        }
    }
}
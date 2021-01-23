using System;
using System.Text;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Форматирование ФИО в полную форму
    /// </summary>
    public class PersonFullFormFormatter: IPersonFullFormFormatter
    {
        readonly StringBuilder _builder = new StringBuilder();
        /// <inheritdoc />
        public string ToFullForm(Person person)
        {
            if(person == null)
                throw new ArgumentNullException(nameof(person));

            AppendPart(person.LastName);
            AppendPart(person.FirstName);
            AppendPart(person.MiddleName);

            return _builder.ToString();
        }

        private void AppendPart(string part)
        {
            if (string.IsNullOrEmpty(part)) 
                return;

            if (_builder.Length > 0)
            {
                _builder.Append(" ");
            }

            _builder.Append(part);
        }
    }
}
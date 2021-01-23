using System;
using System.Linq;
using System.Security;
using System.Text;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Форматирование ФИО в краткую форму
    /// </summary>
    public sealed class PersonShortFormFormatter: IPersonShortFormFormatter
    {
        readonly StringBuilder _builder = new StringBuilder();

        /// <inheritdoc />
        public string ToShortForm(Person person)
        {
            if(person == null)
                throw new ArgumentNullException(nameof(person));

            AppendLastName(person);
            AppendFirstName(person);
            AppendMiddleName(person);

            return _builder.ToString();
        }

        private void AppendMiddleName(Person person)
        {
            if (!HasMiddleName(person)) return;

            _builder.Append(person.MiddleName.First());
            _builder.Append(".");
        }

        private void AppendFirstName(Person person)
        {
            if (!HasFirstName(person)) return;

            _builder.Append(person.FirstName.First());
            _builder.Append(".");
        }

        private void AppendLastName(Person person)
        {
            _builder.Append(person.LastName);
            if (_builder.Length > 0 &&
                (HasFirstName(person)
                 || HasMiddleName(person)))
            {
                _builder.Append(" ");
            }
        }

        /// <summary>
        /// Задано ли отчество
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private static bool HasMiddleName(Person person) => !string.IsNullOrEmpty(person.MiddleName);

        /// <summary>
        /// Задано ли имя
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        private static bool HasFirstName(Person person) => !string.IsNullOrEmpty(person.FirstName);
    }
}
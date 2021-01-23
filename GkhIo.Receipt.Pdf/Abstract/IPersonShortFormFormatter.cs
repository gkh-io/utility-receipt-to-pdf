using GkhIo.Receipt.Pdf.Models;

namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    /// Форматирование ФИО
    /// </summary>
    public interface IPersonShortFormFormatter
    {
        /// <summary>
        /// В краткую форму, например,
        /// Бобриков В.А.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        string ToShortForm(Person person);
    }
}
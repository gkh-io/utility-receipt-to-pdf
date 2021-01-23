using GkhIo.Receipt.Pdf.Models;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IPersonFullFormFormatter
    {
        /// <summary>
        /// В полную форму, например,
        /// Бобриков Владимир Алексеевич
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        string ToFullForm(Person person);
    }
}
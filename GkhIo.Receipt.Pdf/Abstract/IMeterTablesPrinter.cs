using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;

namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    ///     Печать таблиц со счётчиками
    /// </summary>
    public interface IMeterTablesPrinter
    {
        /// <summary>
        /// Добавить в pdf документ блок содержащий информацию по счетчикам жителя
        /// </summary>
        /// <param name="meters">счётчики</param>
        /// <param name="payer">житель</param>
        /// <param name="pdf">pdf документ</param>
        void Print(Meter[] meters, Person payer, Document pdf);
    }
}
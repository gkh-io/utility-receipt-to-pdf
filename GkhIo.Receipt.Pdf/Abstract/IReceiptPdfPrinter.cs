using System.Threading;
using System.Threading.Tasks;
using GkhIo.Receipt.Pdf.Models;

namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    /// Формирует PDF документ для квитанции
    /// </summary>
    public interface IReceiptPdfPrinter
    {
        /// <summary>
        /// Распечатать квитанцию
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>файл с pdf документом</returns>
        Task<LoadedFile> Print(Models.Receipt receipt, CancellationToken cancellationToken = default);
    }
}
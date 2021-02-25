using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IFlatInfoPrinter
    {
        PdfPTable Print(FlatInfo flatInfo, int payUntil);
    }
}
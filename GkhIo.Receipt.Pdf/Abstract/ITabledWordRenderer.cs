using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface ITabledWordRenderer
    {
        PdfPTable Render(string personalAccount);
    }
}
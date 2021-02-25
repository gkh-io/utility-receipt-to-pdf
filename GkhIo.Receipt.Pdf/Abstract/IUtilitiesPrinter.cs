using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IUtilitiesPrinter
    {
        PdfPTable Print(UtilityGroup[] utilityGroups, PaymentPeriod paymentPeriod);
    }
}
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IPaymentInfoPrinter
    {
        PdfPTable Print(PaymentReceiver receiver, Address address, Person payer, string personalAccount);
    }
}
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public interface IDetailsPaymentInfoPrinter
    {
        PdfPTable Print(PaymentReceiver receiver, Address address, Person payer, string personalAccount,
            FlatInfo flatInfo, int payUntil);
    }
}
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;
using NodaTime;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IReceiptPersonPaymentsPrinter
    {
        PdfPTable Print(string receiptNumber, string personalAccount, PaymentPeriod paymentPeriod,
            PaymentsInfo payments, LocalDate payUpTo);
    }
}
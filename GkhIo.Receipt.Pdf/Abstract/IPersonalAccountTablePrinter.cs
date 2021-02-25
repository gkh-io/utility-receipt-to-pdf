using iTextSharp.text;

namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    /// Вывод таблички с лицевым счётом
    /// </summary>
    public interface IPersonalAccountTablePrinter
    {
        void Print(string personalAccount, Document pdf, int horizontalAlignment);
    }
}
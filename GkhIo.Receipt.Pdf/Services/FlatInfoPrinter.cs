using System.Globalization;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class FlatInfoPrinter : IFlatInfoPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private PdfPTable _table;

        public FlatInfoPrinter(CommonPresentationSettings commonPresentationSettings)
        {
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Print(FlatInfo flatInfo, int payUntil)
        {
            _table = new PdfPTable(new[] {2f, 1f});

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            AddCell("Тип квартиры");
            AddBoldCell(flatInfo.Type);
            AddCell("Общая пл./Жилая пл./Отапл. пл.:");
            AddBoldCell($"{flatInfo.CommonArea}/{flatInfo.LivingArea}/{flatInfo.HeatedArea}");
            AddCell("Зарегистрировано/проживает:");
            AddBoldCell(flatInfo.PersonsRegistered.ToString());
            AddCell("Общая площадь дома:");
            AddBoldCell(flatInfo.HouseArea.ToString("#,0.00", nfi));
            AddCell("Площадь помещений (жилых и нежилых)");
            AddBoldCell(flatInfo.PremisesArea.ToString("#,0.00", nfi));
            AddCell("Площадь мест общего пользования");
            AddBoldCell(flatInfo.AreaOfCommonAreas.ToString("#,0.00", nfi));

            _table.AddCell(new PdfPCell(new Phrase($"Учтены все оплаты, поступившие до {payUntil} числа расчётного месяца", _commonPresentationSettings.SmallFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Colspan = 2,
                BorderWidth = _commonPresentationSettings.WithoutBorder
            });
            return _table;
        }

        private void AddCell(string text)
        {
            _table.AddCell(new PdfPCell(new Phrase(text, _commonPresentationSettings.SmallFont))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BorderWidth = _commonPresentationSettings.ThinBorder
            });
        }
        private void AddBoldCell(string text)
        {
            _table.AddCell(new PdfPCell(new Phrase(text, _commonPresentationSettings.SmallBoldFont))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                BorderWidth = _commonPresentationSettings.ThinBorder
            });
        }
    }
}
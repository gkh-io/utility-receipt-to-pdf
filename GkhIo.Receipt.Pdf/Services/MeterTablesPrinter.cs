using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    ///     Печать таблиц со счётчиками
    /// </summary>
    public sealed class MeterTablesPrinter : IMeterTablesPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private readonly IPersonShortFormFormatter _personShortFormFormatter;
        private Font Font => _commonPresentationSettings.SmallFont;
        private Font _fontForPerson;

        public MeterTablesPrinter(CommonPresentationSettings commonPresentationSettings
            , IPersonShortFormFormatter personShortFormFormatter)
        {
            _commonPresentationSettings = commonPresentationSettings;
            _personShortFormFormatter = personShortFormFormatter;
        }

        /// <summary>
        ///     Добавить в pdf документ блок содержащий информацию по счетчикам жителя
        /// </summary>
        /// <param name="meters">счётчики</param>
        /// <param name="payer">житель</param>
        /// <param name="pdf">pdf документ</param>
        public void Print(Meter[] meters, Person payer, Document pdf)
        {
            // таблички со счётчиками
            if (!meters.Any()) return;
            _fontForPerson = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLDITALIC);


            var layoutTable = new PdfPTable(new[] {0.495f, 0.01f, 0.495f})
            {
                SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingBetweenBlocks,
                WidthPercentage = 100
            };

            var countersInFirstColumn = meters.Length - meters.Length / 2;
            var firstMeterssGroup = meters.Take(countersInFirstColumn).ToArray();
            var secondMetersGroup = meters.Skip(countersInFirstColumn).ToArray();

            var leftCell = new PdfPCell(CreateTable(firstMeterssGroup)) {BorderWidth = 0};
            layoutTable.AddCell(leftCell);
            if (secondMetersGroup.Any())
            {
                var emptyCell = new PdfPCell {BorderWidth = 0};
                layoutTable.AddCell(emptyCell);
                var rightCell = new PdfPCell(CreateTable(secondMetersGroup)) {BorderWidth = 0};
                layoutTable.AddCell(rightCell);
            }

            pdf.Add(layoutTable);

            var signParagraph = CreateSignString(payer);

            pdf.Add( signParagraph);
        }

        private Paragraph CreateSignString(Person payer)
        {
            var signParagraph = new Paragraph(
                new Phrase("Дата снятия показаний ____________________ Данные подтверждаю ____________________ ФИО ",
                    Font))
            {
                Alignment = Element.ALIGN_CENTER
            };
            signParagraph.Add(new Phrase(_personShortFormFormatter.ToShortForm(payer).ToUpperInvariant(),
                _fontForPerson));
            return signParagraph;
        }

        private PdfPTable CreateTable(IEnumerable<Meter> metersGroup)
        {
            var table = new PdfPTable(new[] {80f, 109f, 85f, 80f, 80f})
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                SpacingBefore = 0
            };
            var headerText = new[]
            {
                "Тип", "Номер прибора", "Предыдущее показание", "Текущее показание", "Дата след. поверки"
            };

            foreach (var text in headerText) AddCell(table, text);

            foreach (var meter in metersGroup)
            {
                AddCell(table, meter.Type);
                AddCell(table, meter.Number);
                AddCell(table, meter.PreviousValue.ToString(CultureInfo.InvariantCulture));
                AddCell(table, "");
                AddCell(table, meter.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
            }

            return table;
        }

        private void AddCell(PdfPTable table, string text)
        {
            table.AddCell(new PdfPCell
            {
                Phrase = new Phrase(text, Font),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                UseAscender = true //для корректного вертикального выравнивания в ячейке
            });
        }
    }
}
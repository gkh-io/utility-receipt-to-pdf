using GkhIo.Receipt.Pdf.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    ///     Вывод таблички с лицевым счётом
    /// </summary>
    public sealed class PersonalAccountTablePrinter : IPersonalAccountTablePrinter
    {
        private const int LabelIndentationLeft = 25;
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private Font _fontLabel;
        private Document _pdf;
        private readonly ITabledWordRenderer _tabledWordRenderer;
        
        public PersonalAccountTablePrinter(CommonPresentationSettings commonPresentationSettings
            , ITabledWordRenderer tabledWordRenderer)
        {
            _commonPresentationSettings = commonPresentationSettings;
            _tabledWordRenderer = tabledWordRenderer;
        }

        public void Print(string personalAccount, Document pdf, int horizontalAlignment)
        {
            _pdf = pdf;
            
            _fontLabel = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);

            var paragraph = new Paragraph("Лицевой счёт", _fontLabel)
            {
                Alignment = Element.ALIGN_LEFT,
                SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingBetweenBlocks,
                IndentationLeft = LabelIndentationLeft,
                SpacingAfter = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
            };
            _pdf.Add(paragraph);
            _pdf.Add(_tabledWordRenderer.Render(personalAccount));
        }
    }
}
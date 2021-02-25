using GkhIo.Receipt.Pdf.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class TabledWordRenderer : ITabledWordRenderer
    {
        private const int PersonalAccountColumnWidth = 12;
        private Font _fontPersonalAccount;
        private PdfPTable _personalAccountTable;
        private readonly CommonPresentationSettings _commonPresentationSettings;

        public TabledWordRenderer(CommonPresentationSettings commonPresentationSettings)
        {
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Render(string personalAccount)
        {
            _fontPersonalAccount = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10, Font.BOLD);
            CreateTable(personalAccount.Length);
            AddCells(personalAccount);
            return _personalAccountTable;
        }

        private void AddCells(string personalAccount)
        {
            var first = true;
            foreach (var symbol in personalAccount)
            {
                CreateCellForSymbol(symbol, first);
                first = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private void CreateTable(int length)
        {
            _personalAccountTable = new PdfPTable(length)
            {
                LockedWidth = true,
                HorizontalAlignment = Element.ALIGN_LEFT,
                TotalWidth = length * PersonalAccountColumnWidth
            };
        }

        private void CreateCellForSymbol(char symbol, bool first)
        {
            var cell = new PdfPCell(new Phrase(symbol.ToString(), _fontPersonalAccount))
            {
                BorderWidthTop = _commonPresentationSettings.ThinBorder,
                BorderWidthBottom = _commonPresentationSettings.ThinBorder,
                BorderWidthLeft =
                    first ? _commonPresentationSettings.ThinBorder : _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.ThinBorder,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                UseAscender = true //для корректного вертикального выравнивания цифры в ячейке
            };

            _personalAccountTable.AddCell(cell);
        }
    }
}
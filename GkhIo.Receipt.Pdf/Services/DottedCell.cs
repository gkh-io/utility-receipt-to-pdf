using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public class DottedCell : IPdfPCellEvent
    {
        private readonly int _border;

        public DottedCell(int border)
        {
            _border = border;
        }

        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];
            canvas.SaveState();
            canvas.SetLineDash(10,3);
            if ((_border & Rectangle.TOP_BORDER) == Rectangle.TOP_BORDER)
            {
                canvas.MoveTo(position.Right, position.Top);
                canvas.LineTo(position.Left, position.Top);
            }

            if ((_border & Rectangle.BOTTOM_BORDER) == Rectangle.BOTTOM_BORDER)
            {
                canvas.MoveTo(position.Right, position.Bottom);
                canvas.LineTo(position.Left, position.Bottom);
            }

            if ((_border & Rectangle.RIGHT_BORDER) == Rectangle.RIGHT_BORDER)
            {
                canvas.MoveTo(position.Right, position.Top);
                canvas.LineTo(position.Right, position.Bottom);
            }

            if ((_border & Rectangle.LEFT_BORDER) == Rectangle.LEFT_BORDER)
            {
                canvas.MoveTo(position.Left, position.Top);
                canvas.LineTo(position.Left, position.Bottom);
            }

            canvas.Stroke();
            canvas.RestoreState();
        }
    }
}
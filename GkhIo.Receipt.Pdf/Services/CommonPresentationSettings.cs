using System;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Общие настройки стилей для документа
    /// </summary>
    public sealed class CommonPresentationSettings
    {
        /// <summary>
        /// Отсутствие границы
        /// </summary>
        public float WithoutBorder => 0;
        /// <summary>
        /// Тонкая граница
        /// </summary>
        public float ThinBorder => 0.5f;
        /// <summary>
        /// Толстая граница
        /// </summary>
        public float HeavyBorder => 1f;
        /// <summary>
        /// Расстояние между блоками документа
        /// </summary>
        public int DefaultVerticalSpacingBetweenBlocks => 20;
        /// <summary>
        /// Расстояние между элементами документа внутри блока
        /// </summary>
        public int DefaultVerticalSpacingInsideBlocks => 3;

        /// <summary>
        /// Отступ надписи "Извещение" от верха таблицы
        /// </summary>
        public int LabelTopSpacing => 6;

        public Font SmallFont;
        public Font SmallBoldFont;
        public Font SmallBoldItalicFont;
        public Font ExtraSmallFont;
        public Font SuperExtraSmallFont;

        public Font MiddleFont;

        public CommonPresentationSettings()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FontFactory.RegisterDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

            SmallFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
            ExtraSmallFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 6, Font.NORMAL);
            SuperExtraSmallFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 4, Font.NORMAL);
            SmallBoldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD);
            SmallBoldItalicFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLDITALIC);
            MiddleFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
        }
    }
}
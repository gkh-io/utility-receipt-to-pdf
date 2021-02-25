using System.IO;

namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// Объект для работы с файлом
    /// </summary>
    public class LoadedFile
    {
        /// <summary>
        /// Поток на чтение файла
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; set; }
    }
}
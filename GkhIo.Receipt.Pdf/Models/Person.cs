namespace GkhIo.Receipt.Pdf.Models
{
    /// <summary>
    /// ФИО
    /// </summary>
    public sealed class Person
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }
    }
}
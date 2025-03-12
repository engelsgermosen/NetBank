using NetBank.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NetBank.Core.Application.ViewModels.Transaction
{
    public class SaveTransactionViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [Range(1,int.MaxValue,ErrorMessage ="El monto debe ser mayor a 0")]
        [Required(ErrorMessage ="El monto es obligatorio")]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public TransactionType TransactionType { get; set; }

        [Range(78000000,int.MaxValue,ErrorMessage ="Elige una opction valida")]
        public int? OriginProductId { get; set; }

        [Range(78000000, int.MaxValue, ErrorMessage = "Elige una opction valida")]
        public int? DestinationProductId { get; set; }

        public string? UserId { get; set; }

        public bool HasError { get; set; } = false;
        public string? Error {  get; set; }
    }
}

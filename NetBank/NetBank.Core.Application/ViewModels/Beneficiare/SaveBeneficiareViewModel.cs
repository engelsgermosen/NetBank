
using System.ComponentModel.DataAnnotations;

namespace NetBank.Core.Application.ViewModels.Beneficiare
{
    public class SaveBeneficiareViewModel
    {
        public string? Name { get; set; }

        public string? LastName { get; set; }

        [Range(780000000,int.MaxValue, ErrorMessage="Indica un numero de cuenta valido")]
        public int AccountNumber { get; set; }

        public string? UserId { get; set; }
    }
}

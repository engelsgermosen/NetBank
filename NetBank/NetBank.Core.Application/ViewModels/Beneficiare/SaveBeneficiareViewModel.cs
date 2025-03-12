using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBank.Core.Application.ViewModels.Beneficiare
{
    public class SaveBeneficiareViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        [Range(780000000,int.MaxValue, ErrorMessage="Indica un numero de cuenta valido")]
        public int AccountNumber { get; set; }

        public string? UserId { get; set; }
    }
}

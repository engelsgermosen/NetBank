using System;
using System.Collections.Generic;
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

        public int AccountNumber { get; set; }

        public string? UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBank.Core.Domain.Enums
{
    public enum TransactionType
    {
        PaymentExpress = 1,
        PaymentWithCreditCard = 2,
        PaymentLoan = 3,
        PaymentBeneficiarie = 4,
        CashAdvance = 5,
        Transfers = 6,
    }
}

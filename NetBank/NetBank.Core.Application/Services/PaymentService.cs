using AutoMapper;
using NetBank.Core.Application.Interfaces.Repositories;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel,PaymentViewModel,Payment>, IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper) : base(paymentRepository, mapper) 
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

    }
}

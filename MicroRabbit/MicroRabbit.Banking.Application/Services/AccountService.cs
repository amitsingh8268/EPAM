using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository; 
        private readonly IEventBus _eventBus;

        public AccountService(IAccountRepository accountRepository, IEventBus eventBus)
        {
            this._accountRepository = accountRepository;
            _eventBus= eventBus;
        }
        public IEnumerable<Account> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public void Transfer(AccountrTransfer accountrTransfer)
        {
            var createTransferCommand = new CreateTransferCommand(
                 accountrTransfer.FromAccount,
                 accountrTransfer.ToAccount,
                 accountrTransfer.TransferAmount
            );
            _eventBus.SendCommand(createTransferCommand);
        }
    }
}

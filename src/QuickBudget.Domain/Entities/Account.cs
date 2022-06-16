using QuickBudget.Domain.Entities.Base;
using QuickBudget.Domain.ValueObjects;

namespace QuickBudget.Domain.Entities
{
    public record Account : Entity
    {
        protected Account()
        {
            Bank = new Bank();
            Transactions = new HashSet<Transaction>();
        }

        public Account(int bankId, string bankName, string accountNumber, string description) : this()
        {
            Bank.BankId = bankId;
            Bank.BankName = bankName;
            AccountNumber = accountNumber;
            Description = description;
        }

        public Bank Bank { get; private set; }
        public string AccountNumber { get; private set; }
        public string Description { get; private set; }
        public double Balance { get; private set; }

        public int ApplicationUserId { get; private set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}

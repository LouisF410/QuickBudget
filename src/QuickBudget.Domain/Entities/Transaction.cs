namespace QuickBudget.Domain.Entities
{
    public record Transaction
    {
        protected Transaction()
        {
            BudgetPeriods = new HashSet<BudgetPeriod>();
        }

        public Transaction(int accountId, int categoryId, string description, DateTimeOffset transactionDate, decimal amount) : this()
        {
            AccountId = accountId;
            CategoryId = categoryId;
            Description = description;
            TransactionDate = transactionDate;
            Amount = amount;
        }

        public int Id { get; private set; }
        public int AccountId { get; private set; }
        public int CategoryId { get; private set; }

        public string Description { get; private set; }
        public DateTimeOffset TransactionDate { get; private set; }
        public decimal Amount { get; private set; }

        public Account Account { get; private set; }
        public Category Category { get; private set; }

        public ICollection<BudgetPeriod> BudgetPeriods { get; }
    }
}

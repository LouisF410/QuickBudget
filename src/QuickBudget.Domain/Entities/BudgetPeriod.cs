using QuickBudget.Domain.Entities.Base;

namespace QuickBudget.Domain.Entities
{
    public record BudgetPeriod : Entity
    {
        protected BudgetPeriod()
        {
            Budgets = new HashSet<Budget>();
            Transactions = new HashSet<Transaction>();
        }

        public BudgetPeriod(DateTime startDate, DateTime endDate): this()
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public string Period => StartDate.ToString("yyyyMM");
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public ICollection<Budget> Budgets { get; }
        public ICollection<Transaction> Transactions { get; }
    }
}

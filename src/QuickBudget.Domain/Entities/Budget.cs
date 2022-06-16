using QuickBudget.Domain.Entities.Base;

namespace QuickBudget.Domain.Entities
{
    public record Budget : Entity
    {
        public int BudgetPeriodId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }

        public Category Category { get; set; }

        public BudgetPeriod BudgetPeriod { get; set; }
    }
}

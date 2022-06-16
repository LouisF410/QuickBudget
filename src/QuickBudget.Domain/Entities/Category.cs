namespace QuickBudget.Domain.Entities
{
    public record Category
    {
        protected Category()
        {
            Budgets = new HashSet<Budget>();
            Transactions = new HashSet<Transaction>();
        }

        public Category(string name) : this()
        {
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }

        public ICollection<Budget> Budgets { get; }
        public ICollection<Transaction> Transactions { get; }
    }
}

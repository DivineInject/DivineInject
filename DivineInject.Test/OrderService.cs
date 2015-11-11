namespace DivineInject.Test
{
    public class OrderService : IOrderService
    {
        public OrderService(IDatabaseProvider databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }

        public IDatabaseProvider DatabaseProvider { get; private set; }
    }
}
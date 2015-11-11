namespace DivineInject.Test
{
    public class User : IUser
    {
        public User(string name, IDatabaseProvider databaseProvider)
        {
            Name = name;
            DatabaseProvider = databaseProvider;
        }

        public interface IFactory
        {
            IUser Create(string name);
        }

        public string Name { get; private set; }
        public IDatabaseProvider DatabaseProvider { get; private set; }
    }
}
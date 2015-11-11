namespace DivineInject.Test
{
    public class TestClassWithConstructorWithDependency
    {
        public TestClassWithConstructorWithDependency(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
    }
}
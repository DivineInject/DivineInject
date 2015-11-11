namespace DivineInject.Test
{
    public class TestClassWithAnUninjectableConstructor
    {
        public TestClassWithAnUninjectableConstructor(string name)
        {
        }

        public TestClassWithAnUninjectableConstructor(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
    }
}
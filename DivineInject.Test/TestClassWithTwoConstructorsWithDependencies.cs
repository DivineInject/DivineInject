namespace DivineInject.Test
{
    public class TestClassWithTwoConstructorsWithDependencies
    {
        public TestClassWithTwoConstructorsWithDependencies(ITestDependency dependency, ITestSecondDependency secondDependency)
        {
            Dependency = dependency;
            SecondDependency = secondDependency;
        }

        public TestClassWithTwoConstructorsWithDependencies(ITestDependency dependency)
        {
            Dependency = dependency;
        }

        public ITestDependency Dependency { get; private set; }
        public ITestSecondDependency SecondDependency { get; private set; }
    }
}
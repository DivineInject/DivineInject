namespace DivineInject.Test
{
    public class TestClassThatCannotBeInjected
    {
        public TestClassThatCannotBeInjected(string name)
        {
        }

        public TestClassThatCannotBeInjected(ITestSecondDependency dependency)
        {
        }
    }
}
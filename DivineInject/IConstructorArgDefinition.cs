using System.Reflection.Emit;

namespace DivineInject
{
    public interface IConstructorArgDefinition
    {
        IConstructorArg Define(TypeBuilder tb);
    }
}

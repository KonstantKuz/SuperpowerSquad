using JetBrains.Annotations;

namespace Feofun.Modifiers
{
    public interface IModifiableParameterOwner
    {
        [NotNull] IModifiableParameter GetParameter(string name);
        void AddParameter(IModifiableParameter parameter);
    }
}
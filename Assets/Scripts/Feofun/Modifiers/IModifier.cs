namespace Feofun.Modifiers
{
    public interface IModifier
    {
        public void Apply(IModifiableParameterOwner parameterOwner);
    }
}
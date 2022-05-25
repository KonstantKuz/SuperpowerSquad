namespace Feofun.UI.Components
{
    public interface IUiInitializable<in TParam>
    {
        void Init(TParam param);
    }
}
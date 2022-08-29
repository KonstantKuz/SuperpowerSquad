namespace Survivors.ABTest.Providers
{
    public interface IABTestProvider
    {
        string CurrentVariantId { get; }
        bool IsVariantId(string variantId);
    }
}
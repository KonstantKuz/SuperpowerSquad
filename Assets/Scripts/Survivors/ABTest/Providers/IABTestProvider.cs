namespace Survivors.ABTest.Providers
{
    public interface IABTestProvider
    {
        bool IsVariantId(string variantId);
    }
}
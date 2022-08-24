namespace Survivors.ABTest
{
    public interface IABTestProvider
    {
        bool IsVariantId(string variantId);
    }
}
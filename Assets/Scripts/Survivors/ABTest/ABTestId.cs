namespace Survivors.ABTest
{
    public enum ABTestId
    {
        Control,
        WithDisasters
        
    }

    public static class ABTestIdExtension
    {
        public static string ToCamelCase(this ABTestId abTestId)
        {
            var abTestIdStr = abTestId.ToString();
            return char.ToLower(abTestIdStr[0]) + abTestIdStr.Substring(1);
        }
    }
    
}
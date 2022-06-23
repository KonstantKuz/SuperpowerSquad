using System.Collections.Generic;

namespace Survivors.Analytics
{
    public interface IEventParamProvider
    {
        
        
        Dictionary<string, object> GetParams(IEnumerable<string> paramNames);
    }
}
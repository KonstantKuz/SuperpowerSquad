using System.Collections.Generic;
using Survivors.UI.Screen;

namespace Feofun.UI.Screen
{
    public class SwitchingParam
    {
        private readonly Dictionary<string, object[]> _screenParams = new Dictionary<string, object[]>();
       
        private bool _async;
        public bool Async => _async;
        public static SwitchingParam Create() => new SwitchingParam();
        public SwitchingParam(bool async = false)
        {
            _async = async;
        }
        public SwitchingParam SetParamForScreen(ScreenId screenId, params object[] initParams)
        { 
            return SetParamForScreen(screenId.ToString(), initParams);
        }  
        public SwitchingParam SetParamForScreen(string screenName, params object[] initParams)
        {
            _screenParams[screenName] = initParams;
            return this;
        } 
        public SwitchingParam SwitchAsync()
        {
            _async = true;
            return this;
        }
        public object[] GetParamsForScreen(string screenName) => _screenParams.ContainsKey(screenName) ? _screenParams[screenName] : new object[] { };
    }
}
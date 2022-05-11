using System;
using Zenject;

namespace Feofun.App
{
    public class AppContext
    {
        private static DiContainer _container;
        
        public static DiContainer Container
        {
            set
            {
                if (_container != null) {
                    throw new Exception("Container already set");
                }
                _container = value;
            }
            get => _container;
        }
        
        public static void Clear()
        {
            _container = null;
        }
    }
}
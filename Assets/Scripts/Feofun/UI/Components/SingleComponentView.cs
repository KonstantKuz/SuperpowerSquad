using UnityEngine;

namespace Feofun.UI.Components
{
    public class SingleComponentView<T> : MonoBehaviour
    {
        private T _component;
        
        protected T Component
        {
            get
            {
                if (_component == null)
                {
                    _component = GetComponent<T>();
                }

                return _component;
            }
        }
    }
}
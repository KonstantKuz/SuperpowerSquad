using UnityEngine;

namespace Survivors.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        public void PlayDeath()
        {
            Destroy(gameObject);
        }
    }
}

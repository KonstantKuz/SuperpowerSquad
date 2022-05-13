using System;
using Survivors.Units.Component.Health;
using UnityEngine;

namespace Survivors.Units.Component.Death
{
    public class EnemyDeath : MonoBehaviour
    {
        public void Death()
        {
            Destroy(gameObject);
        }
    }
}

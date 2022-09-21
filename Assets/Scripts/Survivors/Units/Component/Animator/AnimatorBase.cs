using UnityEngine;

namespace Survivors.Units.Component.Animator
{
    public abstract class AnimatorBase : MonoBehaviour
    {
        public abstract void Play(string stateName);
        public abstract void SetBool(string param, bool value);
    }
}
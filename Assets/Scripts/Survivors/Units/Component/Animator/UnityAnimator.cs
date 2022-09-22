using UnityEngine;

namespace Survivors.Units.Component.Animator
{
    [RequireComponent(typeof(UnityEngine.Animator))]
    public class UnityAnimator : AnimatorBase
    {
        private UnityEngine.Animator _animator;

        private UnityEngine.Animator Animator => _animator ??= GetComponent<UnityEngine.Animator>();

        public override void Play(string stateName) => Animator.Play(stateName);

        public override void SetBool(string param, bool value) => Animator.SetBool(param, value);
    }
}
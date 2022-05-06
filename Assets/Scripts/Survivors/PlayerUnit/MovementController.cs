using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Survivors.PlayerUnit
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour
    {
        private NavMeshAgent _agent;

        [Inject] 
        private Joystick _joystick;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            _agent.destination = transform.position + new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        }
    }
}
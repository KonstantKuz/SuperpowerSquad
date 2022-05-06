using UnityEngine;

namespace Survivors.Camera
{
    public class CameraFollowBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _distanceToObject;
        
        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main.GetComponent<Transform>();
            cameraTransform.position = transform.position - _distanceToObject * cameraTransform.forward;
        }
    }
}
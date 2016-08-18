using UnityEngine;

namespace NigelGott.Terra
{
    public class CameraController : MonoBehaviour {

        public GameObject target;

        private Vector3 cameraOffset;

        void Start () {
            cameraOffset = transform.position - target.transform.position;
        }
	
        void LateUpdate ()
        {
            var rigidBody = target.GetComponent<Rigidbody>();
            var normalizedVelocityOrForwards = rigidBody.velocity.magnitude > 0 ? rigidBody.velocity.normalized : Vector3.forward;
            transform.position = target.transform.position + cameraOffset;
//            transform.LookAt(target.transform);

        }
    }
}

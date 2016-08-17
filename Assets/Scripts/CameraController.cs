using UnityEngine;

namespace NigelGott.Terra
{
    public class CameraController : MonoBehaviour {

        public GameObject target;

        private Vector3 cameraOffset;

        void Start () {
            cameraOffset = transform.position - target.transform.position;
        }
	
        void LateUpdate () {
            transform.position = target.transform.position + cameraOffset;
        }
    }
}

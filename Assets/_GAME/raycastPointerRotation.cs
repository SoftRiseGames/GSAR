using UnityEngine;
namespace BarthaSzabolcs.IsometricAiming
{
    public class raycastpointerRotation : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        public Geser_Movement rayDuration;

        private Camera mainCamera;

        private void Start()
        {
            rayDuration = GameObject.Find("GESAR").GetComponent<Geser_Movement>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            Aim();
        }

        private void Aim()
        {
            var (success, position) = GetMousePosition();
            if (success)
            {
                var direction = position - transform.position;
                
                direction.y = 0;
                transform.forward = direction;
            }
        }

        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask) && rayDuration.charactermovement)
            {
                return (success: true, position: hitInfo.point);
            }
            else
            {
                return (success: false, position: Vector3.zero);
            }
        }
    }
}

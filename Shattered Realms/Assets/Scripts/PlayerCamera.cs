using UnityEngine;

namespace MD
{
    public class PlayerCamera : MonoBehaviour
    {
        private PlayerManager player; // THIS IS WHAT THE CAMERA WANTS TO FOLLOW

        [Header("Speeds")]
        [SerializeField] float followSmoothSpeed = 0.12f;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        private void Awake()
        {
            player = InputManager.instance.player;
        }
        // WHY LATE UPDATE?
        // THE PLAYER MOVES IN "UPDATE" THIS ALLOWS THE CAMERA TO READ THE FINAL POSITION AT EACH FRAME, PREVENTING "UPDATE-ORDER" RELATED STUTTER
        private void LateUpdate()
        {
            HandleCameraActions();
        }
        private void HandleCameraActions()
        {
            HandleCameraMovement();
            // RotateCamera(); // POSSIBLE FOR FOR CAMERA ROTATION LIKE GRIM DAWN
        }

        private void HandleCameraMovement()
        {
            if (player == null)
                return;

            Vector3 targetPosition = Vector3.SmoothDamp(transform.position , player.transform.position, ref cameraFollowVelocity, followSmoothSpeed);
            transform.position = targetPosition;
        }
    }
}
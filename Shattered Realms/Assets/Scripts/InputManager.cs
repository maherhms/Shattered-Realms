using UnityEngine;
using UnityEngine.InputSystem;

namespace MD
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        private PlayerControls playerControls;

        [Header("Inputs")]
        [SerializeField] private bool leftClick = false;

        [Header("Movement position")]
        [SerializeField] Vector3 movementPosition;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (playerControls == null)
                playerControls = new PlayerControls();

            // MOUSE INPUT
            playerControls.Player.LeftClick.performed += i => leftClick = true;

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        private void Update()
        {
            HandleInputActions();
        }
        // THIS WILL BE CALLED ON EACH FRAME
        private void HandleInputActions()
        {
            HandleLeftClickAction();
        }
        private void HandleLeftClickAction()
        {
            if (leftClick)
            {
                leftClick = false;
                // IF THERE IS A MONSTER ATTACK IT
                // IF THERE IS AN INTERACTABLE, INTERACT WITH IT
                // IF THERE IS NOTHING, ATTEMPT TO MOVE TO THE POSITION

                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    movementPosition = hit.point;
            }
        }
        // DRAW A SPHERE TO SEE WHERE WE CLICKED
        private void OnDrawGizmos()
        {
            // IF WE HAVENT CLICKED ANYWHERE YET AND THE POSITION IS 0,0,0 DO NOTHING
            if (movementPosition == Vector3.zero)
                return;
            // DRAW A SPHERE WITH 0.2F RADIUS WHERE WE CLICKED
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(movementPosition, 0.2f);
        }
    }
}
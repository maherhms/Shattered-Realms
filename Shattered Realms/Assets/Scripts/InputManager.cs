using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace MD
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        private PlayerControls playerControls;
        public PlayerManager player;

        [Header("Inputs")]
        [SerializeField] private bool leftClick = false;

        [Header("Movement position")]
        [SerializeField] Vector3 movementPosition;

        [Header("WASD Input")]
        [SerializeField] bool enableWASDMovement = false;
        [SerializeField] Vector2 movementInput;

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

        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChanged;
            gameObject.SetActive(false);
        }

        private void OnSceneChanged(Scene currentScene, Scene newScene)
        {
            Scene activateScene = SceneManager.GetActiveScene();

            // DISABLE THE PLAYER INPUT MANAGER WHEN ON MENU SCENE, ENABLE IN EVERY OTHER SCENE
            if (activateScene.buildIndex == WorldSaveGameManager.instance.menuSceneIndex)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
                playerControls = new PlayerControls();

            // MOUSE INPUT
            playerControls.Player.LeftClick.performed += i => leftClick = true;
            // WASD INPUT
            playerControls.Player.WASD.performed += i => movementInput = i.ReadValue<Vector2>();

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
            HandleWASDMovement();
        }
        private void HandleLeftClickAction()
        {
            if (leftClick)
            {
                leftClick = false;

                if (player == null)
                    return;

                // IF THERE IS A MONSTER ATTACK IT
                // IF THERE IS AN INTERACTABLE, INTERACT WITH IT
                // IF THERE IS NOTHING, ATTEMPT TO MOVE TO THE POSITION

                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    movementPosition = hit.point;

                player.playerLocomotionManager.MovePlayerToPosition(movementPosition);
            }
        }
        private void HandleWASDMovement()
        {
            if (!enableWASDMovement)
                return;

            // IF NO MOVEMENT IS DETECTED VIA WASD KEYS, ASSIGN YOUR POSITION AS THE TARGET POSITION AND RETURN
            if (movementInput.magnitude < 0.001f)
            {
                player.playerLocomotionManager.MovePlayerToPosition(player.transform.position);
                return;
            }

            // GET THE CAMERA"S FACING DIRECTION BUT TAKE INTO ACCOUNT THE ISOMETRIC VIEW
            Vector3 camerasForwardDirection = Camera.main.transform.forward;
            camerasForwardDirection.y = 0;
            camerasForwardDirection.Normalize();

            Vector3 camerasRightDirection = Camera.main.transform.right;
            camerasRightDirection.y = 0;
            camerasRightDirection.Normalize();

            // CREATE DIRECTION TO MOVE BASED ON CAMERA"S DIRECTION AND YOUR WAS KEY INPUT
            Vector3 moveDirection = (camerasForwardDirection * movementInput.y) + ( camerasRightDirection * movementInput.x);
            player.playerLocomotionManager.MovePlayerToPosition(player.transform.position +  moveDirection);
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
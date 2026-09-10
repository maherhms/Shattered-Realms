using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MD
{
    public class PlayerManager : NetworkBehaviour
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animator = GetComponent<Animator>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            
        }
        private void Update()
        {
            if (IsOwner)
            {
                playerLocomotionManager.HandleMovement();
                // IF I AM OWNER OF THIS CHARACTER SEND POSITION AND ROATION TO NETWORK VARIBLES
                playerNetworkManager.networkPosition.Value = transform.position;
                playerNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                // IF I AM NOT OWNER OF THIS CHARACTER GET POSITION AND ROTATION FROM NETWORK VARIABLES
                transform.position = 
                    Vector3.SmoothDamp(transform.position, playerNetworkManager.networkPosition.Value, 
                    ref playerNetworkManager.networkPositionVelocity, playerNetworkManager.networkSmoothTime);

                transform.rotation = 
                    Quaternion.Slerp(transform.rotation, playerNetworkManager.networkRotation.Value, Time.deltaTime / playerNetworkManager.networkSmoothTime);
            }
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // WHEN THIS OBJECT SPAWNS, IF IT IS OUR PLAYER ( AND NOT AN OTHER PLAYER SPAWNING IN YOUR WORLD ) ASSIGN THIS PLAYER OBJECT TO THE INPUT MANAGER
            if (IsOwner)
            {
                InputManager.instance.player = this;
            }

            playerNetworkManager.isMoving.OnValueChanged += playerNetworkManager.OnIsMovingChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            playerNetworkManager.isMoving.OnValueChanged -= playerNetworkManager.OnIsMovingChanged;
        }
    }
}
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MD
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
        }
        protected override void Update()
        {
            if (IsOwner)
                playerLocomotionManager.HandleMovement();

            SyncNetworkPositionAndRotation();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // WHEN THIS OBJECT SPAWNS, IF IT IS OUR PLAYER ( AND NOT AN OTHER PLAYER SPAWNING IN YOUR WORLD ) ASSIGN THIS PLAYER OBJECT TO THE INPUT MANAGER
            if (IsOwner)
                InputManager.instance.player = this;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
        }
    }
}
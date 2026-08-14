using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MD
{
    public class PlayerManager : NetworkBehaviour
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // WHEN THIS OBJECT SPAWNS, IF IT IS OUR PLAYER ( AND NOT AN OTHER PLAYER SPAWNING IN YOUR WORLD ) ASSIGN THIS PLAYER OBJECT TO THE INPUT MANAGER
            if (IsOwner)
            {
                InputManager.instance.player = this;
            }
        }
    }
}
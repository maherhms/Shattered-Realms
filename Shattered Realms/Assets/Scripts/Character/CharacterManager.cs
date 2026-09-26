using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MD
{
    public class CharacterManager : NetworkBehaviour
    {
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkManager;

        protected virtual void Awake()
        {
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            animator = GetComponent<Animator>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }
        protected virtual void Update()
        {

        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            characterNetworkManager.isMoving.OnValueChanged += characterNetworkManager.OnIsMovingChanged;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            characterNetworkManager.isMoving.OnValueChanged -= characterNetworkManager.OnIsMovingChanged;
        }
        protected virtual void SyncNetworkPositionAndRotation()
        {
            if (IsOwner)
            {
                // IF I AM OWNER OF THIS CHARACTER SEND POSITION AND ROATION TO NETWORK VARIBLES
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                // IF I AM NOT OWNER OF THIS CHARACTER GET POSITION AND ROTATION FROM NETWORK VARIABLES
                transform.position =
                    Vector3.SmoothDamp(transform.position, characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVelocity, characterNetworkManager.networkSmoothTime);

                transform.rotation =
                    Quaternion.Slerp(transform.rotation, characterNetworkManager.networkRotation.Value, Time.deltaTime / characterNetworkManager.networkSmoothTime);
            }
        }
    }
}
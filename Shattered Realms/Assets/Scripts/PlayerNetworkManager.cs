using Unity.Netcode;
using UnityEngine;

namespace MD
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        PlayerManager player;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = 
            new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = 
            new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public float networkSmoothTime = 0.1f;
        public Vector3 networkPositionVelocity;

        [Header("Network Flags")]
        public NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false , NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnIsMovingChanged(bool oldStatus, bool newStatus)
        {
            player.animator.SetBool("isMoving", isMoving.Value);
        }
    }
}
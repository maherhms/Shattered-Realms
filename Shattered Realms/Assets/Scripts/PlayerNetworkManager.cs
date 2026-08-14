using Unity.Netcode;
using UnityEngine;

namespace MD
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        PlayerManager player;

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
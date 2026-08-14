using UnityEngine;

namespace MD
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        PlayerManager player;
        PlayerAnimationManager playerAnimationManager;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
        }
        public void MovePlayerToPosition(Vector3 newPosition)
        {
            player.navMeshAgent.SetDestination(newPosition);
            GetPlayerIsMoving();
        }
        public void GetPlayerIsMoving()
        {
            if (player.navMeshAgent.remainingDistance > 0)
            {
                playerAnimationManager.SetPlayerIsMoving(true);
            }
            else
            {
                playerAnimationManager.SetPlayerIsMoving(false);
            }
        }
    }
}
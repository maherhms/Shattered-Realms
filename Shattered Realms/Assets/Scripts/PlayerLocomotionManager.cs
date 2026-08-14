using UnityEngine;

namespace MD
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        public void MovePlayerToPosition(Vector3 newPosition)
        {
            player.navMeshAgent.SetDestination(newPosition);
        }
    }
}
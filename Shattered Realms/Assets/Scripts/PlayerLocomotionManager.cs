using System.Collections;
using UnityEngine;

namespace MD
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        PlayerManager player;

        private Coroutine hasReachedDestinationCoroutine;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        public void HandleMovement()
        {

        }
        public void MovePlayerToPosition(Vector3 newPosition)
        {
            player.navMeshAgent.SetDestination(newPosition);

            if(hasReachedDestinationCoroutine != null )
                StopCoroutine(hasReachedDestinationCoroutine);

            player.playerNetworkManager.isMoving.Value = true;
            hasReachedDestinationCoroutine = StartCoroutine(HasReachedDestinationCoroutine());
        }
        private IEnumerator HasReachedDestinationCoroutine()
        {
            // WHILE THE PATH OF NAVMESH AGENT IS STILL BEING CALCULATED , WAIT
            while (player.navMeshAgent.pathPending)
            {
                yield return null;
            }

            while (player.navMeshAgent.remainingDistance > player.navMeshAgent.stoppingDistance)
            {
                yield return null;
            }

            player.playerNetworkManager.isMoving.Value = false;
        }
    }
}
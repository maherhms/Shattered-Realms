using System.Collections;
using UnityEngine;

namespace MD
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        PlayerManager player;

        private Coroutine hasReachedDestinationCoroutine;

        [SerializeField] float rotationSpeed = 360f;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        public void HandleMovement()
        {
            synchCharacterWithAgent();
        }
        public void MovePlayerToPosition(Vector3 newPosition)
        {
            player.navMeshAgent.SetDestination(newPosition);

            if(hasReachedDestinationCoroutine != null )
                StopCoroutine(hasReachedDestinationCoroutine);

            player.playerNetworkManager.isMoving.Value = true;
            hasReachedDestinationCoroutine = StartCoroutine(HasReachedDestinationCoroutine());
        }
        // COROUTINE TO SET ISMOVING TO FALSE WHEN REACHED DESTINATION
        private IEnumerator HasReachedDestinationCoroutine()
        {
            // WHILE THE PATH OF NAVMESH AGENT IS STILL BEING CALCULATED , WAIT DONT SET ISMOVING TO FALSE
            while (player.navMeshAgent.pathPending)
            {
                yield return null;
            }
            // WHILE THE PATH OF NAVMESH AGENT STILL DIDNT REACH STOPPING POINT , WAIT DONT SET ISMOVING TO FALSE
            while (player.navMeshAgent.remainingDistance > player.navMeshAgent.stoppingDistance)
            {
                yield return null;
            }
            // ONCE IT IS NOLONGER MOVING OR CALCULATING PATH WE SET TO FALSE
            player.playerNetworkManager.isMoving.Value = false;
        }
        // USE THIS IF YOU DESIRE TO MOVE PLAYER WITH ROOT MOTION ( ANIMATION MOVEMENT )
        // MOVE NAVMESH AGENT WITH THE ROOT MOTION
        // ROTATE IF THE DIRECTION ROTATION IS TOO SHARP
        private void synchCharacterWithAgent()
        {
            if (player.playerNetworkManager.isMoving.Value)
            {
                //player.transform.rotation = player.navMeshAgent.transform.rotation;
                Vector3 desiredDestinationDirection = player.navMeshAgent.steeringTarget - player.transform.position;

                if (desiredDestinationDirection.sqrMagnitude > 0.001f)
                {
                    desiredDestinationDirection.y = 0f;
                    Quaternion targetRotation = Quaternion.LookRotation(desiredDestinationDirection);

                    transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }

            player.navMeshAgent.transform.localPosition = Vector3.zero;
            player.navMeshAgent.transform.localRotation = Quaternion.identity;
        }
    }
}
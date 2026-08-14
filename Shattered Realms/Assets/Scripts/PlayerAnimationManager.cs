using UnityEngine;

namespace MD
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        Animator playerAnimator;

        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
        }
        public void SetPlayerIsMoving(bool isMoving)
        {
            playerAnimator.SetBool("IsMoving", isMoving);
        }
    }
}
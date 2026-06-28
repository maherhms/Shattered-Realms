using UnityEngine;

namespace MD
{
    public class PlayerManager : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
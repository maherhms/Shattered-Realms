using UnityEngine;
using UnityEngine.SceneManagement;

namespace MD
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [Header("World Index")]
        public int menuSceneIndex = 0;
        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(worldSceneIndex);
        }
    }
}
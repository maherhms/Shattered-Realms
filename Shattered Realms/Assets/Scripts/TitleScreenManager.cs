using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MD
{
    public class TitleScreenManager : MonoBehaviour
    {
        [Header("Press Start Menu")]
        [SerializeField] CanvasGroup pressStartMenuCanvasGroup;
        private Coroutine fadeOutPressStartMenuCoroutine;
        [SerializeField] float fadeTime = 1.0f;

        [Header("Character Menu")]
        [SerializeField] CanvasGroup characterMenuCanvasGroup;
        private Coroutine fadeInCharacterMenuCoroutine;
        [SerializeField] GameObject[] characterMenuButtons;

        // PRESS START MENU
        public void StartGame()
        {
            NetworkManager.Singleton.StartHost();
            FadeOutPressStartMenu();
            FadeInCharacterMenu();
        }

        private void FadeOutPressStartMenu()
        {
            if(fadeOutPressStartMenuCoroutine != null)
                StopCoroutine(fadeOutPressStartMenuCoroutine);

            fadeOutPressStartMenuCoroutine = StartCoroutine(FadeOutPressStartMenuCoroutine(fadeTime));
        }

        private IEnumerator FadeOutPressStartMenuCoroutine(float durationOfFade)
        {
            float timeElapsed = 0f;
            float startingAlpha = pressStartMenuCanvasGroup.alpha;

            while (timeElapsed < durationOfFade)
            {
                pressStartMenuCanvasGroup.alpha = Mathf.Lerp(startingAlpha, 0 , timeElapsed / durationOfFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        // CHARACTER MENU
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.StartNewGame();
        }
        private void FadeInCharacterMenu()
        {
            if (fadeInCharacterMenuCoroutine != null)
                StopCoroutine(fadeInCharacterMenuCoroutine);

            fadeInCharacterMenuCoroutine = StartCoroutine(FadeInCharacterMenuCoroutine(fadeTime));

            for (int i = 0; i < characterMenuButtons.Length; i++)
            {
                if (characterMenuButtons[i] == null)
                    continue;

                characterMenuButtons[i].SetActive(true);
            }
        }

        private IEnumerator FadeInCharacterMenuCoroutine(float durationOfFade)
        {
            float timeElapsed = 0f;
            float startingAlpha = 0;

            while (timeElapsed < durationOfFade)
            {
                characterMenuCanvasGroup.alpha = Mathf.Lerp(startingAlpha, 1, timeElapsed / durationOfFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
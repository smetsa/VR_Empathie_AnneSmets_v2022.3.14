using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public Button startButton;
    public GameObject secondUI;
    public static SceneTransitionManager singleton;
    private bool startButtonPressed = false;

    private void Awake()
    {
        if (singleton && singleton != this)
            Destroy(singleton);

        singleton = this;
    }
    private void Start()
    {
        // Finde den Button und füge ihm eine Funktion hinzu, die aufgerufen wird, wenn er geklickt wird
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        // Setze den Zustand des Start-Buttons auf "gedrückt"
        startButtonPressed = true;
    }

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene("OutsideScene");
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }
    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {

        fadeScreen.FadeOut();
        // Neue Szene starten
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}

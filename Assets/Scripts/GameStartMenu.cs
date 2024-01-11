using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public GameObject startButton;
    public List<Button> levelButtons;

    private Button selectedLevel;

    void Start()
    {
        startButton.SetActive(false); // Start-Button zu Beginn deaktivieren

        foreach (var levelButton in levelButtons)
        {
            levelButton.onClick.AddListener(() => SelectLevel(levelButton));
        }

        startButton.GetComponent<Button>().onClick.AddListener(StartSelectedLevel);
    }

    void SelectLevel(Button levelButton)
    {
        if (selectedLevel != null)
        {
            selectedLevel.interactable = true; // Aktiviere den vorher ausgewählten Level-Button
        }

        selectedLevel = levelButton; // Setze das ausgewählte Level
        selectedLevel.interactable = false; // Deaktiviere den ausgewählten Level-Button
        startButton.SetActive(true); // Start-Button aktivieren
        DisableAllLevelButtons();
        int level = int.Parse(selectedLevel.name.Substring(selectedLevel.name.Length - 1)); // Extrahiere die Levelnummer aus dem Namen
    }

    void StartSelectedLevel()
    {
        if (selectedLevel != null)
        {
            int level = int.Parse(selectedLevel.name.Substring(selectedLevel.name.Length - 1)); // Extrahiere die Levelnummer aus dem Namen
            StartLevel(level);
        }
    }

    public void StartLevel(int level)
    {
        SceneTransitionManager.singleton.GoToSceneAsync(level);
        Debug.Log("Start Level " + level);
    }
    void DisableAllLevelButtons()
    {
        foreach (var levelButton in levelButtons)
        {
            levelButton.gameObject.SetActive(false); // Deaktiviere den Level-Button
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;

    [Header("Level Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        // Hook events
        level1Button.onClick.AddListener(() => StartLevel(1));
        level2Button.onClick.AddListener(() => StartLevel(2));
        level3Button.onClick.AddListener(() => StartLevel(3));

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }


    }

    public void StartLevel(int level)
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(level);








    }

    public void HideAll()
    {
        mainMenu.SetActive(false);




    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);

    }
}


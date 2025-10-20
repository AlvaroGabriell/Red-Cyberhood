using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [Header("References")]
    private GameObject player;
    public GameObject MainMenuBackground, MainMenu, SettingsMenu, PauseMenu, DefeatMenu, VictoryMenu, CreditsMenu;

    private Stack<GameObject> menuStack = new();

    [Header("Settings")]
    public Button audioSubmenuButton;
    public Button controlsSubmenuButton;
    private Button currentSelectedSubmenuButton;
    public GameObject AudioSubmenu, ControlsSubmenu;

    void Awake()
    {
        // Inicializa o singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject GetPlayer()
    {
        if (player.IsUnityNull()) player = GameObject.FindGameObjectWithTag("Player");
        return player;
    }

    // Use this function whenever you need to open a menu
    // It ensures that only one menu is active at a time, closing the current menu before opening the new one
    // Maintaining a stack of menus to manage navigation between them.
    public void OpenMenu(GameObject menu)
    {
        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(false);
        }

        menu.SetActive(true);
        menuStack.Push(menu);
    }

    // Use this function to close the current menu and reactivate the previous one, if any.
    public void CloseCurrentMenu()
    {
        if (menuStack.Count > 0)
        {
            menuStack.Pop().SetActive(false);
        }
        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(true);
        }
    }
    public void CloseAllMenus()
    {
        while(menuStack.Count > 0)
        {
            menuStack.Pop().SetActive(false);
        }
    }

    public void OnPlay()
    {
        MainMenuBackground.SetActive(false);
        CloseCurrentMenu();
        GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Enable();
        MusicManager.Instance.PlayMusic(true, "GameplayFuture");
    }
    public void OnSettings()
    {
        ShowSubmenu(audioSubmenuButton);
        OpenMenu(SettingsMenu);
        foreach (Slider slider in SettingsMenu.GetComponentsInChildren<Slider>())
        {
            if (slider.name == "MusicSlider") MusicManager.Instance.AttachSlider(slider);
            if (slider.name == "SFXSlider") SFXManager.Instance.AttachSlider(slider);
        }
    }
    public void OnPause()
    {
        OpenMenu(PauseMenu);
        GameController.Instance.PauseGame();
        GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
    }
    public void OnContinue()
    {
        CloseCurrentMenu();
        GameController.Instance.ResumeGame();
        GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Enable();
    }
    public void OnCredits()
    {
        OpenMenu(CreditsMenu);
    }
    public void OnExit()
    {
        Application.Quit();
    }
    public void OnRestart()
    {
        //TODO: fazer restart
    }
    public void OnMainMenu()
    {
        GameController.Instance.ResumeGame();
        StartCoroutine(ReloadScene());
    }
    public void OnBack()
    {
        CloseCurrentMenu();
    }
    public void HandleEscape()
    {
        if (menuStack.Contains(MainMenu)) return;
        else if (SettingsMenu.activeSelf) OnBack();
        else if (PauseMenu.activeSelf) OnContinue();
        else OnPause();
    }

    private IEnumerator ReloadScene()
    {
        yield return SceneManager.UnloadSceneAsync("Principal");

        var load = SceneManager.LoadSceneAsync("Principal", LoadSceneMode.Additive);
        yield return load;

        CloseAllMenus();
        GameController.Instance.StartGame();
    }
    
    // ----------------- Settings Menu -----------------
    public void ShowSubmenu(Button submenuButton)
    {
        if (currentSelectedSubmenuButton != null)
        {
            ColorBlock resetCB = currentSelectedSubmenuButton.colors;
            resetCB.normalColor = new Color(1, 1, 1, 0.01f); 
            resetCB.highlightedColor = new Color(0.62f, 0.62f, 0.62f, 0.17f); 
            resetCB.selectedColor = new Color(0.62f, 0.62f, 0.62f, 0.43f); 
            currentSelectedSubmenuButton.colors = resetCB;
        }

        currentSelectedSubmenuButton = submenuButton;
        ColorBlock c = currentSelectedSubmenuButton.colors;
        Color selectedColor = new(0.62f, 0.62f, 0.62f, 0.43f);
        c.normalColor = selectedColor;
        c.highlightedColor = selectedColor;
        c.pressedColor = selectedColor;
        c.selectedColor = selectedColor;
        currentSelectedSubmenuButton.colors = c;

        if (submenuButton == audioSubmenuButton)
        {
            ControlsSubmenu.SetActive(false);
            AudioSubmenu.SetActive(true); 
        }
        else if (submenuButton == controlsSubmenuButton)
        {
            AudioSubmenu.SetActive(false);
            ControlsSubmenu.SetActive(true);
        }
    }
}

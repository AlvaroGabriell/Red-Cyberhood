using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CinematicLibrary))]
[RequireComponent(typeof(PlayerInput))]
public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [Header("References")]
    private GameObject player;
    private PlayerInput input;
    public GameObject MainMenuBackground, CinematicMenu, MainMenu, SettingsMenu, PauseMenu, DefeatMenu, VictoryMenu, CreditsMenu, TutorialMenu;

    private Stack<GameObject> menuStack = new();

    [Header("Settings")]
    public Button audioSubmenuButton;
    public Button controlsSubmenuButton;
    private Button currentSelectedSubmenuButton;
    public GameObject AudioSubmenu, ControlsSubmenu;

    [Header("Cinematic")]
    private bool isCinematicPlaying = false;
    private Coroutine actualCinematicCoroutine = null;
    private string actualCinematicName;
    private static CinematicLibrary cinematicLibrary;

    
    private bool tutorialShowed = false, isRestart = false;
    public bool hideTutorial = false;

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
        cinematicLibrary = GetComponent<CinematicLibrary>();
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
    }

    void Update()
    {
        if (hideTutorial && !tutorialShowed)
        {
            tutorialShowed = true;
            StartCoroutine(HideTutorial());
        }
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
        CloseCurrentMenu();
        actualCinematicCoroutine = StartCoroutine(ShowCinematic("StartCinematic", success =>
        {
            if (success) ReallyStartGame();
        }));
    }
    public void OnSettings()
    {
        ShowSubmenu(audioSubmenuButton);
        OpenMenu(SettingsMenu);
        foreach (Slider slider in SettingsMenu.GetComponentsInChildren<Slider>())
        {
            if (slider.gameObject.name == "MusicSlider") MusicManager.Instance.AttachSlider(slider);
            if (slider.gameObject.name == "SFXSlider") SFXManager.Instance.AttachSlider(slider);
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
        GameController.Instance.ResumeGame();
        StartCoroutine(RestartGame());
    }
    public void OnMainMenu()
    {
        GameController.Instance.ResumeGame();
        StartCoroutine(ReloadPrincipalScene());
    }
    public void OnBack()
    {
        CloseCurrentMenu();
    }
    
    public void OnEscapeButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (menuStack.Contains(MainMenu)) return;
            else if (actualCinematicName == "StartCinematic") return;
            else if (SettingsMenu.activeSelf) OnBack();
            else if (PauseMenu.activeSelf) OnContinue();
            else OnPause();
        }
    }

    public void OnSkipCinematic(InputAction.CallbackContext context)
    {
        if(context.performed && isCinematicPlaying)
        {
            if (actualCinematicCoroutine != null)
            {
                StopCoroutine(actualCinematicCoroutine);
                StopCinematic();
                ReallyStartGame();
            }
        }
    }

    private IEnumerator ReloadPrincipalScene()
    {
        MusicManager.Instance.StopAllAudioSources();
        yield return SceneManager.UnloadSceneAsync("Principal");

        var load = SceneManager.LoadSceneAsync("Principal", LoadSceneMode.Additive);
        yield return load;

        CloseAllMenus();
        hideTutorial = false; tutorialShowed = false;
        TutorialMenu.SetActive(false);
        isRestart = false;
        GameController.Instance.StartGame();
    }

    private IEnumerator RestartGame()
    {
        yield return StartCoroutine(ReloadPrincipalScene());
        hideTutorial = true; tutorialShowed = true;
        isRestart = true;
        CloseCurrentMenu();
        ReallyStartGame();
        yield break;
    }

    private IEnumerator HideTutorial()
    {
        yield return new WaitForSeconds(5);

        TutorialMenu.SetActive(false);
    }

    private void ReallyStartGame()
    {
        MainMenuBackground.SetActive(false);
        if(!isRestart) TutorialMenu.SetActive(true);
        GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Enable();
        MusicManager.Instance.PlayGameplayMusic();
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

    // ----------------- Cinematic -----------------
    private IEnumerator ShowCinematic(string cinematicName, Action<bool> onComplete)
    {
        if (isCinematicPlaying)
        {
            Debug.Log("There's already a cinematic playing");
            onComplete.Invoke(false);
            yield break;
        }
        Cinematic cinematic = cinematicLibrary.GetCinematic(cinematicName);
        if (cinematic == null)
        {
            Debug.Log("Cinematic not found");
            onComplete.Invoke(false);
            yield break;
        }

        isCinematicPlaying = true;
        actualCinematicName = cinematicName;

        CinematicMenu.SetActive(true);

        Image[] componentImages = CinematicMenu.GetComponentsInChildren<Image>();
        Image currentImage = componentImages[0];
        Image nextImage = componentImages[1];

        currentImage.color = new Color(1, 1, 1, 1);
        nextImage.color = new Color(1, 1, 1, 0);


        for (int i = 0; i < cinematic.cinematicFrames.Length; i++)
        {
            var frame = cinematic.cinematicFrames[i];
            nextImage.sprite = frame.sprite;

            if (!cinematic.cinematicFrames[i].fadeInFromPreviousImage)
            {
                currentImage.sprite = frame.sprite;
                yield return new WaitForSeconds(frame.timeInScreen);
                continue;
            }

            float fadeDuration = Mathf.Min(1f, frame.timeInScreen * frame.fadeIntesity); // Fade = 30% of the timeInScreen (intensity of the fade, basically)
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                nextImage.color = new Color(1, 1, 1, t);
                yield return null;
            }

            (nextImage, currentImage) = (currentImage, nextImage);

            float remaining = Mathf.Max(0, frame.timeInScreen - fadeDuration);
            yield return new WaitForSeconds(remaining);
        }

        StopCinematic();
        onComplete.Invoke(true);
    }
    
    private void StopCinematic()
    {
        CinematicMenu.SetActive(false);
        isCinematicPlaying = false;
        actualCinematicName = null;
        actualCinematicCoroutine = null;
    }
}

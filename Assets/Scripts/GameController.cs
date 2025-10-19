using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    private GameObject player;
    public bool isPaused = false;

    void Awake()
    {
        // Inicializa o singleton
        if (Instance == null)
        {
            Instance = this;
            if(!IsSceneLoaded("Principal")) SceneManager.LoadScene("Principal", LoadSceneMode.Additive);
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }
    
    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        UIController.Instance.MainMenuBackground.SetActive(true);
        UIController.Instance.OpenMenu(UIController.Instance.MainMenu);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pausa o jogo
        SFXManager.Instance.PauseSource(true);
        isPaused = true; // Atualiza o estado de pausa
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Retoma o jogo
        SFXManager.Instance.PauseSource(false);
        isPaused = false; // Atualiza o estado de pausa
    }
}

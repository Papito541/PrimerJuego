using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Pause pause;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        pause = FindFirstObjectByType<Pause>();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PerderVida(int vidap)
    {   
        pause.DesactivarVida(vidap);
    }

    public void Perder()
    {
        pause.Lose();
    }
}

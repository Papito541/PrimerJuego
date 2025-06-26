using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject panelPausa;
    public bool pausado = false;
    public Button botonpause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pausado)
            {
                reanudar();
            }
            else
            {
                pausar();
            }
        }
    }

    public void reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1;
        pausado = false;
    }
    public void pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0;
        pausado = true;
    }

    public void resetG()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        pausado = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        pausado = false;
    }
}

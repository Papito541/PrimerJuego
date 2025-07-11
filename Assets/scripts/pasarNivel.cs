using UnityEngine;
using UnityEngine.SceneManagement;

public class pasarNivel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Debug.Log("¡Jugador entró al trigger!");
            jugador playerScript = collision.GetComponent<jugador>();
            if (playerScript != null)
            {
                playerScript.DesactivarDobleSalto();
            }
            DesbloquearNext();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void DesbloquearNext()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex",SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
            Debug.Log("¡Se completo!");
        }
    }
}

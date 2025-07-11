using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaFalsa : MonoBehaviour
{

    public static int indice;
    [SerializeField] private string nombreNivelTrampa = "NivelTrampa"; // o usa el índice si prefieres

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            indice = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(nombreNivelTrampa);
        }
    }
}

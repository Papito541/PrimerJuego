using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaFalsa : MonoBehaviour
{

    [SerializeField] private string nombreNivelTrampa = "NivelTrampa"; // o usa el índice si prefieres

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.nivelAnterior = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(nombreNivelTrampa);
        }
    }
}

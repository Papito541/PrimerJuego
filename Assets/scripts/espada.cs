using UnityEngine;

public class espada : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Notifica al jugador que recogió la espada
            collision.GetComponent<jugador>()?.ActivarEspada();

            // Destruye este objeto (pickup de la espada)
            Destroy(gameObject);
        }
    }
}

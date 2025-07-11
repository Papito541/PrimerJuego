using UnityEngine;

public class PowerUpDobleSalto : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.name); // <-- esto
        if (collision.CompareTag("Player"))
        {
            jugador playerScript = collision.GetComponent<jugador>();
            if (playerScript != null)
            {
                Debug.Log("Jugador recogió el doble salto."); // <-- esto
                playerScript.ActivarDobleSalto();
                Destroy(gameObject);
            }
        }
    }
}

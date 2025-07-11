using UnityEngine;

public class PowerUpDobleSalto : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisi�n detectada con: " + collision.name); // <-- esto
        if (collision.CompareTag("Player"))
        {
            jugador playerScript = collision.GetComponent<jugador>();
            if (playerScript != null)
            {
                Debug.Log("Jugador recogi� el doble salto."); // <-- esto
                playerScript.ActivarDobleSalto();
                Destroy(gameObject);
            }
        }
    }
}

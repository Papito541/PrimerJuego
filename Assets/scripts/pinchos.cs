using UnityEngine;

public class pinchos : MonoBehaviour
{
    private void Start()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (GameObject enemigo in enemigos)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemigo.GetComponent<Collider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDamage = new Vector2(collision.gameObject.transform.position.x, 0);
            jugador playerScript = collision.gameObject.GetComponent<jugador>();
            playerScript.recibiendoDamage(direccionDamage, 1);
        }
    }
}

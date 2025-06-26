using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public float detectionRadius = 5f;
    public float attackRadius = 1.8f;
    public float speed = 10f;
    public bool enMovimiento;
    private bool recibeDamage;
    public float Fuerza_rebote = 6f;
    private bool atacando = false;
    private jugador playerScript;
    public int vida = 3;
    public bool muerto;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        playerScript = player.GetComponent<jugador>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool jugadorVivo = (playerScript != null && !playerScript.muerto);

        animator.SetBool("jugadorM", jugadorVivo); // NUEVO
        animator.SetBool("EnMovimiento", enMovimiento);
        animator.SetBool("RecibeDamage", recibeDamage);

        if (jugadorVivo && !muerto)
        {
            Movimiento();
        }
        else
        {
            enMovimiento = false;
            movement = Vector2.zero;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector2 direccionDamage = new Vector2(collision.gameObject.transform.position.x, 0);
    //        jugador playerScript = collision.gameObject.GetComponent<jugador>();
    //        playerScript.recibiendoDamage(direccionDamage, 1);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada") && !muerto && !atacando && !recibeDamage)
        {
            Vector2 direccionDamage = new Vector2(transform.position.x, 0);
            recibiendoDamage(direccionDamage, 1);
        }
    }

    public void Movimiento()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRadius && !atacando && !recibeDamage)
        {
            StartCoroutine(SecuenciaDeAtaque());
        }

        if (playerScript.muerto || atacando || recibeDamage) return;

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            // Flip sprite
            if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);

            movement = new Vector2(direction.x, 0);
            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        if (!recibeDamage)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    public void recibiendoDamage(Vector2 direccion, int cantDamage)
    {
        if (muerto || atacando || recibeDamage) return;
            
        recibeDamage = true;
            vida -= cantDamage;
            if (vida <= 0)
            {
                muerto = true;
                enMovimiento = false;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
                animator.SetTrigger("GolpeM");
                Destroy(gameObject, 1.5f);
                return;
            }
                Vector2 rebote = (transform.position - new Vector3(direccion.x, transform.position.y)).normalized;
                rb.AddForce(new Vector2(rebote.x, 0.2f) * Fuerza_rebote, ForceMode2D.Impulse);
    }

    public void desactivaDamge()
    {
        recibeDamage = false;
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator SecuenciaDeAtaque()
    {
        // Salir si el jugador est� muerto antes de empezar el ataque
        if (playerScript == null || playerScript.muerto)
        {
            yield break;
        }

        atacando = true;
        enMovimiento = false;
        movement = Vector2.zero;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;


        animator.SetBool("EnMovimiento", false);

        animator.SetTrigger("PreparandoAtaque");
        yield return new WaitForSeconds(0.6f);

        if (playerScript != null && !playerScript.muerto)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            animator.SetTrigger("Atacando");
            if (distanceToPlayer <= attackRadius)
            {
                playerScript.recibiendoDamage(transform.position, 1);
            }
        }

        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        atacando = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
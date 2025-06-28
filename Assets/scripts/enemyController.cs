using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public float detectionRadius = 5f;
    public float attackRadius = 1.2f;
    public float alturaDeteccion = 1.5f;
    public float alturaAtaque = 1.5f;
    public float speed = 10f;
    public bool enMovimiento;
    private bool recibeDamage;
    public float Fuerza_rebote = 6f;
    private bool atacando = false;
    private jugador playerScript;
    public int vida = 3;
    public bool muerto;

    public Transform puntoA;
    public Transform puntoB;
    public float tiempoEsperaEnPunto = 2f;

    private Transform puntoActual;
    private bool esperando = false;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        playerScript = player.GetComponent<jugador>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        puntoActual = puntoA;
    }

    // Update is called once per frame
    void Update()
    {
        bool jugadorVivo = (playerScript != null && !playerScript.muerto);

        animator.SetBool("jugadorM", jugadorVivo); 
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
        float distanceX = Mathf.Abs(player.position.x - transform.position.x);
        float distanceY = Mathf.Abs(player.position.y - transform.position.y);

        if (distanceX < attackRadius && distanceY < alturaAtaque && !atacando && !recibeDamage)
        {
            StartCoroutine(SecuenciaDeAtaque());
        }

        if (playerScript.muerto || atacando || recibeDamage) return;

        if (distanceX < detectionRadius && distanceY < 1.5f)
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
            if (!esperando)
            {
                float distancia = Vector2.Distance(transform.position, puntoActual.position);

                if (distancia < 0.2f)
                {
                    StartCoroutine(EsperarYRotar());
                }
                else
                {
                    Vector2 direccion = (puntoActual.position - transform.position).normalized;
                    movement = new Vector2(direccion.x, 0);
                    enMovimiento = true;
                    if (direccion.x != 0)
                        transform.localScale = new Vector3(Mathf.Sign(direccion.x), 1, 1);
                }

                if (!recibeDamage)
                    rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
            }
            else
            {
                movement = Vector2.zero;
                enMovimiento = false;
            }
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

    private IEnumerator EsperarYRotar()
    {
        esperando = true;
        movement = Vector2.zero;
        enMovimiento = false;

        yield return new WaitForSeconds(tiempoEsperaEnPunto);

        // Cambiar al siguiente punto
        puntoActual = (puntoActual == puntoA) ? puntoB : puntoA;
        esperando = false;
    }


    private void OnDrawGizmos()
    {
        // Rectángulo de detección (amarillo)
        Gizmos.color = Color.yellow;
        float ancho = detectionRadius * 2f;
        Gizmos.DrawWireCube(transform.position, new Vector3(ancho, alturaDeteccion, 0));

        // Rectángulo de ataque (rojo)
        Gizmos.color = Color.red;
        float anchoAtaque = attackRadius * 2f;
        Gizmos.DrawWireCube(transform.position, new Vector3(anchoAtaque, alturaAtaque, 0));
    }
}
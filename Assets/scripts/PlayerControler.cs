using UnityEngine;

public class jugador : MonoBehaviour
{
    public int vida = 3;
    public float velocidad = 20f;
    public float Fuerza_salto = 10f;
    public float Fuerza_rebote = 6f;
    public float LongitudRayCast = 0.5f;
    public LayerMask Suelo;
    private bool en_Suelo;
    private bool recibeDamage;
    private bool estabaEnSuelo = false;
    private Rigidbody2D rb;
    private bool tieneEspada = false;
    private bool atacando;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float maxComboDelay = 0.5f;
    private bool siguienteComboSolicitado = false;
    public bool muerto;

    public Animator animator;
    public GameObject correr_particulas;
    public GameObject espadaGO;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        espadaGO.SetActive(true);
    }

    void Update()
    {
        if (!muerto)
        {
            Movimiento();

            Salto();

            if (tieneEspada && !recibeDamage && en_Suelo)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (!atacando && (comboTimer > 0 || comboStep == 0))
                    {
                        comboStep++;
                        if (comboStep <= 3)
                        {
                            comboTimer = maxComboDelay;
                            EjecutarCombo();
                        }
                    }
                    else if (atacando)
                    {
                        siguienteComboSolicitado = true;
                    }
                }
            }

            if (comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0)
                {
                    comboStep = 0;
                }
            }
        }

        animator.SetBool("Atacando", atacando);
        animator.SetBool("RecibeDamage", recibeDamage);
        estabaEnSuelo = en_Suelo;
    }

    public void Movimiento()
    {
        float inputX = Input.GetAxis("Horizontal");

        if (!recibeDamage)
        {
            Vector2 nuevaVelocidad = rb.linearVelocity;
            if (!atacando)
                nuevaVelocidad.x = inputX * velocidad;
            else
                nuevaVelocidad.x = 0;
            rb.linearVelocity = nuevaVelocidad;
        }

        bool estaCayendo = !en_Suelo && rb.linearVelocity.y < -0.01f;
        if (estaCayendo || recibeDamage)
        {
            animator.SetFloat("movimiento", 0f); // evita correr en el aire o si está recibiendo daño
        }
        else
        {
            animator.SetFloat("movimiento", Mathf.Abs(rb.linearVelocity.x));
        }
        if (!atacando)
        {
            if (inputX < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (inputX > 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Salto()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LongitudRayCast, Suelo);
        en_Suelo = hit.collider != null;

        if (en_Suelo && Input.GetKeyDown(KeyCode.Space) && !recibeDamage && !atacando)
        {
            rb.AddForce(new Vector2(0f, Fuerza_salto), ForceMode2D.Impulse);
        }

        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && en_Suelo && !recibeDamage)
        {
            if (!correr_particulas.activeInHierarchy)
                correr_particulas.SetActive(true);
        }
        else
        {
            if (correr_particulas.activeInHierarchy)
                correr_particulas.SetActive(false);
        }

        animator.SetBool("En_Suelo", en_Suelo);
        float velocidadY = Mathf.Abs(rb.linearVelocity.y) < 0.001f ? 0f : rb.linearVelocity.y;
        animator.SetFloat("VelocidadY", velocidadY);

        bool estaCayendo = rb.linearVelocity.y < -0.01f && !en_Suelo;
        animator.SetBool("EstaCayendo", estaCayendo);

        if (!estabaEnSuelo && en_Suelo)
        {
            animator.SetTrigger("Aterrizaje");
            animator.SetBool("EstaCayendo", false);
        }
    }

    public void recibiendoDamage(Vector2 direccion, int cantDamage)
    {
        if (!recibeDamage)
        {
            recibeDamage = true;
            vida -= cantDamage;
            if (vida <= 0)
            {
                muerto = true;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Detiene el movimiento horizontal
                animator.SetTrigger("GolpeM");
            }
            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0.2f).normalized;
                rb.AddForce(rebote * Fuerza_rebote, ForceMode2D.Impulse);
            }
        }
    }

    public void desactivaDamge()
    {
        recibeDamage = false;
        rb.linearVelocity = Vector2.zero;

        // Forzar animación de caída si sigue en el aire
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, LongitudRayCast, Suelo);
        en_Suelo = hit.collider != null;

        if (!en_Suelo)
        {
            animator.SetBool("EstaCayendo", true);
            animator.SetBool("En_Suelo", false);
        }
    }

    public void ActivarEspada()
    {
        if (tieneEspada) return;

        tieneEspada = true;
        animator.SetBool("TienesEspada", true);
    }

    public void DesactivaAtacar()
    {
        atacando = false;

        if (siguienteComboSolicitado && comboStep < 3)
        {
            comboStep++;
            comboTimer = maxComboDelay;
            siguienteComboSolicitado = false;
            EjecutarCombo();
        }
        else if (comboStep >= 3 || comboTimer <= 0)
        {
            comboStep = 0;
            comboTimer = 0f;
            siguienteComboSolicitado = false;
        }
    }

    void EjecutarCombo()
    {
        atacando = true;
        animator.ResetTrigger("Ataque1");
        animator.ResetTrigger("Ataque2");
        animator.ResetTrigger("Ataque3");

        switch (comboStep)
        {
            case 1:
                animator.SetTrigger("Ataque1");
                break;
            case 2:
                animator.SetTrigger("Ataque2");
                break;
            case 3:
                animator.SetTrigger("Ataque3");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Vector2.down * LongitudRayCast);
    }
}

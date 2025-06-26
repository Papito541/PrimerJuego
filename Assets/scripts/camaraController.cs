using UnityEngine;

public class camaraController : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.025f;
    public Vector3 desplazamiento;

    void Start()
    {
        if (objetivo != null)
        {
            desplazamiento = transform.position - objetivo.position;
        }
    }


    private void LateUpdate()
    {
        Vector3 PosicionDeseada = objetivo.position + desplazamiento;

        // Mantener la cámara siempre en su Z original (por ejemplo, -10 para 2D)
        PosicionDeseada.z = transform.position.z;

        Vector3 PosicionSuavizada = Vector3.Lerp(transform.position, PosicionDeseada, velocidadCamara);
        transform.position = PosicionSuavizada;
    }

}

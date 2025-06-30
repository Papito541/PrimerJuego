using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textoTimer; // Asigna desde el Inspector
    private float tiempoTranscurrido = 0f;
    private bool activo = true;
    
    void Update()
    {
        if (!activo) return;

        tiempoTranscurrido += Time.deltaTime;

        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60f);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60f);

        textoTimer.text = minutos.ToString("0") + ":" + segundos.ToString("00");
    }

    public void Pausar()
    {
        activo = false;
    }

    public void Reanudar()
    {
        activo = true;
    }

    public void Reiniciar()
    {
        tiempoTranscurrido = 0f;
        activo = true;
    }

    public float GetTiempoTotal()
    {
        return tiempoTranscurrido;
    }
}

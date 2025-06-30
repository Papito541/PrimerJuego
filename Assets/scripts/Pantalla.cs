using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pantalla : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDropDown;
    Resolution[] resoluciones;

    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        RevisarResolucion();

    }

    public void ActiveFULLS(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    //
    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropDown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);


            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }

        }

        resolucionesDropDown.AddOptions(opciones);
        //resolucionesDropDown.value = resolucionActual;
        //resolucionesDropDown.RefreshShownValue();

        int guardado = PlayerPrefs.GetInt("numeroResolucion", resolucionActual);
        resolucionesDropDown.value = guardado;
        resolucionesDropDown.RefreshShownValue();
        ////
        //resolucionesDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
        //
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        //
        resoluciones = Screen.resolutions;
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);
        //


        Resolution resolution = resoluciones[indiceResolucion];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    //
}
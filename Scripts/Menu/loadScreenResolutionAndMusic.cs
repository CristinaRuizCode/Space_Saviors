using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadScreenResolutionAndMusic : MonoBehaviour
{
    private void Start()
    {
        //Cargamos resolucion de pantalla
        string resolution = PlayerPrefs.GetString("ScreenResolution");
        string[] allParameters = resolution.Split(',');

        if (!resolution.Equals(""))
        {
            Screen.SetResolution(int.Parse(allParameters[0]), int.Parse(allParameters[1]), bool.Parse(allParameters[2]));
        }
        else
        {
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
        }

        //cargamos volumen
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().loadVol();
    }
}

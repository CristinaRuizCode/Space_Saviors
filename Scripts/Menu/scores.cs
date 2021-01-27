using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scores : MonoBehaviour
{
    //----------- VARIABLES PRIVADAS -------------
    // 1. nombres               -> nombre de los jugadores con las mejores puntuaciones
    // 2. totalPuntuaciones     -> total de puntuaciones maximas que maneja el juego
    string nombres;
    int totalPuntuaciones = 5;

    public void Start()
    {
        nombres = "";
        string nameAndPoints = "";
        string[] words;
        //obtenemos los nombres de playerprefs de las puntuaciones que va a tener el juego
        for (int i = 1; i <= totalPuntuaciones; i++)
        {
            nombres += "#" + i + "      "; //posicion de esa puntuacion
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(i.ToString()))) //Posicion -> nombrePlayer,puntuacion
            {
                nameAndPoints = PlayerPrefs.GetString(i.ToString());
                words = nameAndPoints.Split(',');
                if (Int32.Parse(words[1]) == 0)
                {
                    nombres += "-----------";
                }
                else
                {
                    nombres += words[0] + "  " + words[1];
                }
            }
            else
            {
                nombres += "-----------";
            }
            if (i < totalPuntuaciones)
            {
                nombres += "\n\n"; //para evitar un salto de linea en la ultima puntuacion a mostrar
            }
        }
        //ponemos los nombres de los jugadores en el text
        transform.GetComponent<Text>().text = nombres;
    }
}

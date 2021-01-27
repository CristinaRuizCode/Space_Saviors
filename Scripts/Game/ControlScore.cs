using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScore : MonoBehaviour
{
    //-------- VARIABLES PRIVADAS -------
    // 1. score         -> Text donde se muestra al puntuacion del jugador
    // 2. multiplicador -> Multiplicador en base a la dificultad elegida. A mas dificultad mas puntos por destruir naves
    [SerializeField]Text score;
    int multiplicador;

    public void Awake()
    {
        if (PlayerPrefs.GetString("Difficulty").Equals("Hard"))
        {
            multiplicador = 3;
        }else if (PlayerPrefs.GetString("Difficulty").Equals("Medium"))
        {
            multiplicador = 2;
        }
        else
        {
            multiplicador = 1;
        }
    }

    //funcion que actualiza la puntuacion
    public void updateScore(int points)
    {
        int actualScore = int.Parse(score.text);
        actualScore += points * multiplicador;
        score.text = actualScore.ToString();
    }
}

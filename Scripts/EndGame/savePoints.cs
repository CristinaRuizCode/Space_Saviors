using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class savePoints : MonoBehaviour
{
    //--------- VARIABLES PRIVADAS ----------
    // 1. totalPuntuaciones     -> total de puntuaciones que almacena el juego
    // 2. pointsText            -> el texto donde mostramos las puntuaciones
    // 3. newScore              -> Texto que se activa si conseguimos entrar en las mejores puntuaciones
    int totalPuntuaciones = 5;
    [SerializeField] Text pointsText, newScore;

    private void Start()
    {
        //En PlayerPrefs las puntuaciones se guardan tal que asi: setString(posicionDeLaPuntuacion, nombreJugador+","+ puntuacion)
        int points = PlayerPrefs.GetInt("Puntuacion");
        pointsText.text = points.ToString();
        string playerName = PlayerPrefs.GetString("PlayerName");
        List<KeyValuePair<String, String>> items = new List<KeyValuePair<String, String>>();

        string nameAndPoints = "";
        string[] words;
        int auxiliarPoints = points;
        string auxiliarName = playerName;

        for (int i = 1; i <= totalPuntuaciones; i++)
        {
            nameAndPoints = PlayerPrefs.GetString(i.ToString());

            if (!string.IsNullOrEmpty(nameAndPoints))
            {
                words = nameAndPoints.Split(',');

                if (auxiliarPoints >= Int32.Parse(words[1]))
                {
                    items.Add(new KeyValuePair<String, String>(auxiliarName, auxiliarPoints.ToString()));
                    auxiliarName = words[0];
                    auxiliarPoints = Int32.Parse(words[1]);
                    newScore.gameObject.SetActive(true);
                }
                else
                {
                    items.Add(new KeyValuePair<String, String>(words[0], words[1]));
                }
            }
            else 
            {
                if (auxiliarName.Equals(playerName))
                {
                    items.Add(new KeyValuePair<String, String>(playerName, points.ToString()));
                    auxiliarName = "-----------";
                    auxiliarPoints = 0;
                }
                else
                {
                    items.Add(new KeyValuePair<String, String>(auxiliarName, auxiliarPoints.ToString()));
                    auxiliarName = "-----------";
                    auxiliarPoints = 0;
                }
            }

        }

        for (int i = 1; i <= totalPuntuaciones; i++)
        {
            PlayerPrefs.SetString(i.ToString(),items[i - 1].Key + "," + items[i-1].Value );
        }

        PlayerPrefs.Save();
    }
}

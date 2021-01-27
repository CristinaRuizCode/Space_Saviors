using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDifficulty : MonoBehaviour
{
    //------------ VARIABLES PRIVADAS ----------
    // 1. enemiesLife, enemiesSpeed , enemiesAmount     -> Estadiscticas de los enemigos
    // 2. diffiulty                                     -> Dropdown del selector de dificultad 
    [SerializeField]Slider enemiesLife, enemiesSpeed, enemiesAmount;
    [SerializeField] Dropdown difficulty;

    private void Awake()
    {
        enemiesLife.value = 1;
        enemiesSpeed.value = 0.8f;
        enemiesAmount.value = 3;
    }

    //funcion que actualiza las estadisticas de los enemigos en funcion de la dificultad seleccionada
    public void changeDifficulty()
    {
        if (difficulty.options[difficulty.value].text.Equals("Easy"))
        {
            enemiesLife.value = 1;
            enemiesSpeed.value = 0.8f;
            enemiesAmount.value = 3;
        }else if (difficulty.options[difficulty.value].text.Equals("Medium"))
        {
            enemiesLife.value = 2;
            enemiesSpeed.value = 1f;
            enemiesAmount.value = 2; 
        }
        else
        {
            enemiesLife.value = 3;
            enemiesSpeed.value = 1.2f;
            enemiesAmount.value = 0;
        }
    }
}

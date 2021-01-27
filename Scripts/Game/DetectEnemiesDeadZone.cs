using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemiesDeadZone : MonoBehaviour
{
    //funcion que detecta sin un enemigo a llegado a la parte inferior de la pantalla, si es así, se acaba el juego
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            if (collision.transform.parent.name.Equals("Enemies"))
            {
                StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FinishGame>().LoseGame());
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FinishGame>().Finish();
            }
        }

    }
}

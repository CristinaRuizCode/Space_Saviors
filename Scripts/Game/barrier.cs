using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrier : MonoBehaviour
{
    //--------- VARIABLES PRIVADAS -----------
    // hp   -> Vidas de un barrier
    [SerializeField]int hp=5;

    //Quitar una vida a un barrier
    public void quitarVida()
    {
        hp--;
        if (hp == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Barriers/barrier_" + hp);
        }
               
    }

}

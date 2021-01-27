using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeDirectionMoveEnemies : MonoBehaviour
{
    //------------- VARIABLES PUBLICAS ----------
    // 1. direction -> direccion de movimiento de los enemigos
    public int direction; 


    //duncion qur cuando detecta que un enemigo ha llegado al límite de la zona de enemigos, cambia la dirección hacia el lado contrario
    void OnTriggerEnter2D(Collider2D collision)
    {
        //enemigo raso o superior
        if (collision.gameObject.tag.Equals("Enemy_Raso") || collision.gameObject.tag.Equals("Enemy_Superior"))
        {
            GameObject.FindGameObjectWithTag("Enemies").GetComponent<ControlEnemiesShips>().dir = direction;
        }
        else
        {
            //boss
            GameObject.FindGameObjectWithTag("Enemies").GetComponent<ControlBoss>().dir = direction;
        }
       
    }
}

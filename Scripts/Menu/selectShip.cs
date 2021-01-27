using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectShip : MonoBehaviour
{
    // ------------ VARIABLES PRIVADAS -----------------
    // 1. childActive       -> hijo selecionado
    // 2. next, previous    -> referencias a los botones next y previous
    // 3. aux               -> color auxiliar para la visibilidad de los sprites
    private int childActive;
    private GameObject next, previous, nameShip, statistics;
    private Color aux;

    //Mostramos el primer hijo, desactivamos el boton previo y si hay mas hijos activamos el boton next
    public void Awake()
    {
        next = GameObject.FindGameObjectWithTag("NextShip");
        previous = GameObject.FindGameObjectWithTag("PreviousShip");
        nameShip = GameObject.FindGameObjectWithTag("NameShip");
        statistics = GameObject.FindGameObjectWithTag("Statistics");
        childActive = 0;

        changeName(transform.GetChild(childActive).name);
        updateStats(childActive);
        cambiarVisibilidad(childActive,255);

        previous.SetActive(false);

        if (transform.childCount < 2)
        {
            next.SetActive(false);
        }
    }

    //funcion que cambia el color alpha de un sprite
    //Recibe:   pos   -> posicion del hijo
    //          color -> valor de alpha
    void cambiarVisibilidad(int pos, int color)
    {
        aux = transform.GetChild(pos).GetComponent<Image>().color;
        aux.a = color;
        transform.GetChild(pos).GetComponent<Image>().color = aux;
    }

    void changeName(string n)
    {
        nameShip.GetComponent<Text>().text = n;
    }

    //activamos la siguiente nave, si no hay mas naves desactivamos el boton next y activamos el previous
    public void nextShip()
    {
        cambiarVisibilidad(childActive,0);
        childActive++;
        cambiarVisibilidad(childActive,255);
        changeName(transform.GetChild(childActive).name);
        updateStats(childActive);

        if (transform.childCount - 1 == childActive)
        {
            next.SetActive(false);
        }

        if (!previous.gameObject.activeSelf)
        {
           previous.SetActive(true);
        }
    }

    //activamos la nave anterior, si no hay mas naves anteriores desactivamos el boton previous y activamos el next
    public void previousShip()
    {
        cambiarVisibilidad(childActive,0);
        childActive--;
        cambiarVisibilidad(childActive,255);
        changeName(transform.GetChild(childActive).name);
        updateStats(childActive);

        if (childActive == 0)
        {
            previous.SetActive(false);
        }

        if (!next.gameObject.activeSelf)
        {
            next.SetActive(true);
        }
    }


    //Segund la nave seleccionada tendremos diferentes estadisticas
    void updateStats(int childActive)
    {
        GameObject ship = Resources.Load<GameObject>("Ships/" + transform.GetChild(childActive).name);
        statistics.transform.GetChild(0).GetComponent<Slider>().value = ship.GetComponent<PlayerShip>().hp;
        statistics.transform.GetChild(1).GetComponent<Slider>().value = ship.GetComponent<PlayerShip>().shields;
        statistics.transform.GetChild(2).GetComponent<Slider>().value = ship.GetComponent<PlayerShip>().energy;
        statistics.transform.GetChild(3).GetComponent<Slider>().value = ship.GetComponent<PlayerShip>().speedMove;
        statistics.transform.GetChild(4).GetComponent<Slider>().value = ship.GetComponent<PlayerShip>().shootTime;
    }
}

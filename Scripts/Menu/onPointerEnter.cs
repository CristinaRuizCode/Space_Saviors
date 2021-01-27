using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onPointerEnter : MonoBehaviour
{
    //--- VARIABLES PRIVADAS ---
    // 1. MenuSelected  -> Selectores del menu
    // 2. selected      -> Transform de los selectores
    // 3. sizeButton    -> tam de los botones
    // 4. sound         -> SOnido al interactuar con los botones dle menu
    // 4. offset        -> Offset para las naves situadas a los laterales de los botones
    GameObject MenuSelected;
    Transform selected;
    Vector2 sizeButton;
    AudioSource sound;
    [SerializeField] float offset = 50, size = 20;

    public void Awake()
    {
        MenuSelected = GameObject.FindGameObjectWithTag("MenuSelected");
        if (MenuSelected)
        {
            selected = MenuSelected.transform;
        }
        sound = GameObject.FindGameObjectWithTag("AudioEffect").GetComponent<AudioSource>();
    }

    public void Start()
    {
        visibilidad(false);
    }

    //activamos las naves, hacemos el boton grande y posicionamos las naves a los laterales dle boton
    public void onEnter()
    {
        visibilidad(true);
        makeBigger();
        makeSound();
        selected.GetChild(0).localPosition = new Vector3(transform.localPosition.x + GetComponent<RectTransform>().rect.size.x / 2 + offset, transform.localPosition.y, - 30);
        selected.GetChild(1).localPosition = new Vector3(transform.localPosition.x - GetComponent<RectTransform>().rect.size.x / 2 - offset, transform.localPosition.y, - 30);
    }

    public void makeSound()
    {
        sound.Play();
    }

    //disminuimos el tam dle boton y ponemos desactivados las naves
    public void onExit()
    {
        makeSmaller();
        visibilidad(false);
    }

    //funcion para activar o desactivar un objeto
    public void visibilidad(bool visibilidad)
    {
        if (MenuSelected)
        {
            selected.gameObject.SetActive(visibilidad);
        }
    }

    //funcion para hacer mas grande el boton
    public void makeBigger()
    {
        sizeButton = transform.GetComponent<Image>().rectTransform.sizeDelta;
        transform.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(sizeButton.x + size, sizeButton.y + size);
    }

    //funcion para hacer mas pequenio el boton
    public void makeSmaller()
    {
        sizeButton = transform.GetComponent<Image>().rectTransform.sizeDelta;
        transform.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(sizeButton.x - size, sizeButton.y - size);
    }
}

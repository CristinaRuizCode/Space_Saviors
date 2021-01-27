using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class visibility : MonoBehaviour
{
    //FUNCIONES PARA CAMBIAR LA VISIBILIDAD DE UN PANEL, BOTON , IMAGEN O TEXTO DE MANERA PROGRESIVA
    Image[] arrayImagen;
    Text[] arrayText;

    private void OnEnable()
    {
        arrayText = GetComponentsInChildren<Text>(true);
        arrayImagen = GetComponentsInChildren<Image>(true);
        changeVisibility(new Color());
    }

    private void Update()
    {
        changeVisibility(transform.GetComponent<Image>().color);
    }

    void changeVisibility(Color colo)
    {
        Color c;
        arrayImagen = GetComponentsInChildren<Image>(true);
        foreach (Image im in arrayImagen)
        {
            c = im.color;
            c.a = colo.a;
            im.color = c;
        }

        foreach (Text t in arrayText)
        {
            c = t.color;
            c.a = colo.a;
            t.color = c;
        }
    }
}

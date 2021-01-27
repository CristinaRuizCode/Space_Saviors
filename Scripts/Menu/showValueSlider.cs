using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showValueSlider : MonoBehaviour
{
    //Texto que se muestra al lateral del slider
    [SerializeField]Text t;

    private void Start()
    {
        updateValueText();
    }

    //funcion que actualiza el texto en funcion de lo que este en el slider
    public void updateValueText()
    {
        if (transform.GetComponent<Slider>().wholeNumbers)
        {
            t.text = transform.GetComponent<Slider>().value.ToString();
        }
        else
        {
            t.text = transform.GetComponent<Slider>().value.ToString("0.00");
        }
    }
}

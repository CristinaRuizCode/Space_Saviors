using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveTextCredits : MonoBehaviour
{
    //--- VARIABLES PRIVADAS ---
    // 1. speedText -> velocidad de movimiento del texto
    // 2. posiInitial   -> posicion incial de los creditos
    // 3. timeButton    -> tiempo que se ha mantenido pulsado el boton
    // 4. positionParent    -> posicion del panel de los creditos
    [SerializeField]float speedText = 0.02f;
    float posiInitial, timeButton = 0, positionParent;
    GameObject sec1, sec2, sec3;

    private void OnEnable()
    {
        posiInitial = transform.position.y;
        sec1 = transform.parent.parent.GetChild(1).GetChild(0).gameObject;
        sec2 = transform.parent.parent.GetChild(1).GetChild(1).gameObject;
        sec3 = transform.parent.parent.GetChild(1).GetChild(2).gameObject;
        positionParent = transform.parent.position.y;
        resetButtonsTime();
    }

    private void Update()
    {
        detectESC();
    }

    void FixedUpdate()
    {
        if (transform.position.y < positionParent)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speedText, transform.position.z);
        }
        else
        {
            StartCoroutine(closePanel(1));
        }
    }

    IEnumerator closePanel(float sec)
    {
        yield return new WaitForSeconds(sec);
        transform.parent.parent.gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        transform.position = new Vector3(transform.position.x, posiInitial, transform.position.z);
    }

    //funcion que resetea los sprites
    public void resetButtonsTime()
    {
        sec1.SetActive(false);
        sec2.SetActive(false);
        sec3.SetActive(false);
    }

    //funcion para detectar el boton ESC
    public void detectESC()
    {
        if (Input.GetAxis("Cancel") != 0)
        {
            timeButton += Time.deltaTime;

            startEscape((int)timeButton);
        }
        else
        {
            resetButtonsTime();
            timeButton = 0;
        }
    }

    //Sprites que se activan al mantener el boton esc pulsado
    public void startEscape(int time)
    {
        switch (time)
        {
            case 0:
                sec1.SetActive(true);
                break;
            case 1:
                sec2.SetActive(true);
                break;
            case 2:
                sec3.SetActive(true);
                break;
            case 3:
                transform.parent.parent.gameObject.SetActive(false);
                break;
        }
    }
}

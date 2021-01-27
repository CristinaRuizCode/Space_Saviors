using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startMenu : MonoBehaviour
{

    //---------- VARIABLES PRIVADAS ---------
    // 1. go                                                -> gameManager
    // 2. transformAux                                      -> trasnform auxiliar
    // 3. vGeneral, vEfectos, vMusica                       -> volumen de los audiosource
    // 4. uiSliderGeneral, uiSliderEffects, uiSliderMusica  -> slider de los diversos volumenes
    GameManager go;
    Transform transformAux;
    float vGeneral, vEfectos, vMusica;
    Slider uiSliderGeneral, uiSliderEffects, uiSliderMusica;

    //Establemos velocidades de las estrellas y configuramos funciones que necesiten gamemanager
    void Start()
    {
        go = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
        transform.GetChild(10).GetChild(7).GetComponent<Button>().onClick.AddListener(startGame);
        transformAux = transform.GetChild(9);
        transformAux.gameObject.SetActive(true);

        transformAux.GetChild(2).GetComponent<Dropdown>().onValueChanged.AddListener(changeRes);
        transformAux.GetChild(3).GetComponent<Toggle>().onValueChanged.AddListener(changeFullScreen);
        transformAux.GetChild(4).GetComponent<Slider>().onValueChanged.AddListener(changeVolumenGeneral);
        transformAux.GetChild(5).GetComponent<Slider>().onValueChanged.AddListener(changeVolumenEffects);
        transformAux.GetChild(6).GetComponent<Slider>().onValueChanged.AddListener(changeVolumenMusic);

        transformAux.gameObject.SetActive(false);

        //Animacion de estrellas background
        transform.GetChild(1).GetChild(0).GetComponent<Animator>().SetFloat("speed", 0.5f);
        transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetFloat("speed", 0.7f);
        transform.GetChild(1).GetChild(2).GetComponent<Animator>().SetFloat("speed", 0.6f);


        // ------------ CARGAMOS EL VOLUMEN DE LA MUSICA Y EFECTOS --------------
        uiSliderGeneral = transformAux.GetChild(4).GetComponent<Slider>();

        if (!PlayerPrefs.GetString("VolumenGeneral").Equals(""))
        {
            vGeneral = float.Parse(PlayerPrefs.GetString("VolumenGeneral"));
        }
        else
        {
            vGeneral = 0.5f;
        }

        uiSliderGeneral.value = vGeneral;

        uiSliderEffects = transformAux.GetChild(5).GetComponent<Slider>();
        if (!PlayerPrefs.GetString("VolumenEfectos").Equals(""))
        {
            vEfectos = float.Parse(PlayerPrefs.GetString("VolumenEfectos"));
        }
        else
        {
            vEfectos = 0.5f;
        }

        uiSliderEffects.value = vEfectos;

        uiSliderMusica = transformAux.GetChild(6).GetComponent<Slider>();
        if (!PlayerPrefs.GetString("VolumenMusica").Equals(""))
        {
            vMusica = float.Parse(PlayerPrefs.GetString("VolumenMusica"));
        }
        else
        {
            vMusica = 0.5f;
        }
        uiSliderMusica.value = vMusica;

    }
    private void startGame()
    {
        go.saveDifficultyAndPlayerName();
        go.LoadScene("Game");

    }

    private void changeRes(int c)
    {
        Dropdown uiDropdown = transformAux.GetChild(2).GetComponent<Dropdown>();
        go.changeRes(uiDropdown);

    }

    private void changeFullScreen(bool c)
    {
        Toggle uiToggle = transformAux.GetChild(3).GetComponent<Toggle>();
        go.changeFullScreen(uiToggle);

    }

    private void changeVolumenGeneral(float c)
    {
        uiSliderGeneral.value = c;
        go.changeVol(uiSliderGeneral);
    }

    private void changeVolumenEffects(float c)
    {
        uiSliderEffects.value = c;
        go.changeVol(uiSliderEffects);

    }

    private void changeVolumenMusic(float c)
    {
        uiSliderMusica.value = c;
        go.changeVol(uiSliderMusica);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //------- VARIABLES PRIVADAS ------
    // 1. waitChangeR       -> Tiempo de espera para mantener resolucion
    // 2. Fullscreen        -> Bool para controlar la pantalla completa
    // 3. rWidth, rHeight   -> valores de resolucion de pantalla
    [SerializeField] int waitChangeR = 15;
    bool FullScreen;
    int rWidth, rHeight;


    private void Awake()
    {
        int numGameManagers = FindObjectsOfType<GameManager>().Length;
        if (numGameManagers != 1)
        {
            Destroy(this.gameObject);
        }else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // --------------- FUNCIONES PARA CAMBIO DE RESOLUCION DE PANTALLA -----------------

    //funcion para obtener la resolucion actual
    void getResolutionScreen()
    {
        FullScreen = Screen.fullScreen;
        rWidth = Screen.width;
        rHeight = Screen.height;
    }

    //funcion para cambiar la resolucion
    public void changeRes(Dropdown dp)
    {
        string[] mySplit = dp.options[dp.value].text.Split(' ');

        getResolutionScreen();
        Screen.SetResolution(int.Parse(mySplit[0]), int.Parse(mySplit[2]), Screen.fullScreen);
        StartCoroutine(waitResolution());
    }
    
    //funcion para cambiar fullscreen
    public void changeFullScreen(Toggle t)
    {
        getResolutionScreen();
        Screen.SetResolution(Screen.width, Screen.height, t.isOn);
        StartCoroutine(waitResolution());
    }

    //funcion que mantiene la resolucion que se ha escogido
    public void mantener()
    {
        PlayerPrefs.SetString("ScreenResolution", Screen.width + "," + Screen.height + "," + Screen.fullScreen);
        PlayerPrefs.Save();
        visibility();
    }

    //funcion que vuelve a la resolucion/fullscreen anterior 
    public void restaurar()
    {
        Screen.SetResolution(rWidth, rHeight, FullScreen);
        PlayerPrefs.SetString("ScreenResolution", rWidth + "," + rHeight + "," + FullScreen);
        PlayerPrefs.Save();
        GameObject.FindGameObjectWithTag("ToggleFullScreen").GetComponent<Toggle>().isOn = FullScreen;
        visibility();
    }

    //funcion para desactivar el panel de confirmacion de cambio de resolucion
    void visibility()
    {
        GameObject.FindGameObjectWithTag("panelResolution").SetActive(false);
        StopAllCoroutines();
    }

    //funcion que espera X segundos antes de volver a la resolucion anterior
    IEnumerator waitResolution()
    {
        int tiempo = waitChangeR;
        Text time = GameObject.FindGameObjectWithTag("time").GetComponent<Text>();

        while (tiempo >= 0)
        {
            time.text = tiempo.ToString("0");
            yield return new WaitForSeconds(1);
            tiempo--;
        }

        restaurar();
    }


    //--------------- FUNCION PARA CAMBIO DE VOLUMEN -----------------------------
    public void changeVol(Slider sd)
    {
        //Buscamos todos los audiosource
       AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

        switch (sd.name)
        {
            case "SliderVolumenGeneral":
                AudioListener.volume = sd.value;
                PlayerPrefs.SetString("VolumenGeneral", sd.value.ToString());
                break;

            case "SliderVolumenEfectos":

                foreach (AudioSource audioS in allAudioSources)
                {
                    if (audioS.tag.Equals("AudioEffect"))
                    {
                        audioS.volume = sd.value;
                    }
                }
                PlayerPrefs.SetString("VolumenEfectos", sd.value.ToString());
                break;
            case "SliderVolumenMusica":
                foreach (AudioSource audioS in allAudioSources)
                {
                    if (audioS.tag.Equals("AudioMusic"))
                    {
                        audioS.volume = sd.value;
                    }
                }
                PlayerPrefs.SetString("VolumenMusica", sd.value.ToString());
                break;
        }
    }

    //funcion que inicia el sonido a lo guardado en playerPrefs
    public void loadVol()
    {
        //Cargamos el volumen de la musica, sonidos y efectos, sino por defecto lo ponemos a la mitad
        float vGeneral, vEfectos, vMusica;

        if (!PlayerPrefs.GetString("VolumenGeneral").Equals(""))
        {
            vGeneral = float.Parse(PlayerPrefs.GetString("VolumenGeneral"));
        }
        else
        {
            vGeneral = 0.5f;
        }

        if (!PlayerPrefs.GetString("VolumenEfectos").Equals(""))
        {
            vEfectos = float.Parse(PlayerPrefs.GetString("VolumenEfectos"));
        }
        else
        {
            vEfectos = 0.5f;
        }

        if (!PlayerPrefs.GetString("VolumenMusica").Equals(""))
        {
            vMusica = float.Parse(PlayerPrefs.GetString("VolumenMusica"));
        }
        else
        {
            vMusica = 0.5f;
        }

        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            if (audioS.tag.Equals("AudioEffect"))
            {
                audioS.volume = vEfectos;
            }
            else if (audioS.tag.Equals("AudioMusic"))
            {
                audioS.volume = vMusica;
            }
            else
            {
                audioS.volume = vGeneral;
            }
        }
    }

    // --------------------- FUNCION PARA SALIR DEL JUEGO -------------------

    public void exitGame()
    {
        Application.Quit();
    }

    //---------------------- GUARDAR DIFICULTAD -----------------------------

        //funcion que guarda la dificultad, el nombre del jugador y la nave escogida
    public void saveDifficultyAndPlayerName()
    {
        GameObject panelShip = GameObject.FindGameObjectWithTag("PanelSelectShip");
        Transform panelDifficulty = panelShip.transform.GetChild(3);

        PlayerPrefs.SetString("Difficulty", panelDifficulty.GetChild(12).GetComponent<Dropdown>().options[panelDifficulty.GetChild(12).GetComponent<Dropdown>().value].text);
        PlayerPrefs.SetFloat("Enemies_Life", panelDifficulty.GetChild(0).GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("Enemies_Speed", panelDifficulty.GetChild(1).GetComponent<Slider>().value);  
        PlayerPrefs.SetFloat("Enemies_Amount", panelDifficulty.GetChild(2).GetComponent<Slider>().value);
        
        PlayerPrefs.SetString("PlayerName", panelShip.transform.GetChild(panelShip.transform.childCount-1).GetChild(panelShip.transform.GetChild(panelShip.transform.childCount - 1).childCount-1).GetComponent<Text>().text);
        PlayerPrefs.SetString("ShipName", panelShip.transform.GetChild(1).GetComponent<Text>().text);                          
        
        PlayerPrefs.Save();
    }

    //------------------------- CAMBIAR ESCENAS ------------------
    
    //funcion para mostrar las puntuaciones cuando se vuelva al menu principal
    public void scoresScene(string scene)
    {
        SceneManager.LoadScene(scene);
       StartCoroutine(scores());
    }

    IEnumerator scores()
    {
        while (!GameObject.FindGameObjectWithTag("ScoreButton"))
        {
            yield return null;
        }
        

        if (GameObject.FindGameObjectWithTag("ScoreButton"))
        {
            GameObject.FindGameObjectWithTag("ScoreButton").GetComponent<Button>().onClick.Invoke();
        }

    }

    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}

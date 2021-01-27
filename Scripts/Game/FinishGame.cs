using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour
{
    // -------------------- VARIABLES PRIVADAS ---------------
    // 1. soundBackground       -> AudioSource de la musica de fondo
    [SerializeField] AudioSource soundBackground;

    //funcion que sale de la escena del juego y guarda la puntuacion obtenida
    public void Finish()
    {
        PlayerPrefs.SetInt("Puntuacion", int.Parse(GameObject.FindGameObjectWithTag("HUD").transform.GetChild(5).GetComponent<Text>().text));
        GameObject.FindObjectOfType<GameManager>().LoadScene("EndGame");
    }

    //funcion que para a los enemigos y muestra la derrota
    public IEnumerator LoseGame()
    {
        //busco los enemigos y los paro
        GameObject enemies = GameObject.FindGameObjectWithTag("Enemies");
        enemies.GetComponent<ControlBoss>().enabled = false;
        enemies.GetComponent<ControlEnemiesShips>().enabled = false;
        enemies.SetActive(false);

        GameObject textWin = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(6).gameObject;
        textWin.GetComponent<Text>().text = "YOU LOOSE";

        soundBackground.clip = Resources.Load<AudioClip>("Audios/loose");
        soundBackground.Play();
        yield return new WaitForSeconds(soundBackground.clip.length);
        Finish();
    }

    //funcion que muestra la victoria
    public IEnumerator  WinGame()
    {
        GameObject textWin = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(6).gameObject;
        textWin.GetComponent<Text>().text = "YOU WIN";
        textWin.GetComponent<Animator>().enabled = true;

        soundBackground.clip = Resources.Load<AudioClip>("Audios/win");
        soundBackground.Play();
        yield return new WaitForSeconds(soundBackground.clip.length);
        Finish();
    }
}

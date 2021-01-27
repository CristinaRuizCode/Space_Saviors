using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitGame : MonoBehaviour
{
    public void exit()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().scoresScene("Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class playerName : MonoBehaviour
{
    //Boton para comenzar la partida. Solo activable cuando se escriba el nombre del jugador
    [SerializeField] Button start;

    private void Awake()
    {
        start.GetComponent<onPointerEnter>().GetComponent<EventTrigger>().enabled = false;
        start.interactable = false;
    }

    public void onNameChange()
    { 
        //actualizamos label
        GetComponent<InputField>().ForceLabelUpdate();
        bool b = transform.GetChild(transform.childCount - 1).GetComponent<Text>().text.Length > 0;
        start.interactable = b;
        start.GetComponent<onPointerEnter>().GetComponent<EventTrigger>().enabled = b;
    }
}

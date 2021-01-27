using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed;
    public void Start()
    {

        GetComponent<Animator>().SetFloat("speed", speed);
    }
}

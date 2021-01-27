using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    //------- VARIABLES PRIVADAS -------
    // 1. timeDead  -> Tiempo de vida de la bala
    // 2. velocity  -> velocidad a la que se desplaza la bala
    // 3. shoShot   -> variable que indica quien ha disparado la bala
    // 4. explosion -> Audiosource de la explosion de una bala
    [SerializeField] float timeDead;
    [SerializeField] float velocity;
    string whoShot;
    AudioSource sound;

    //-------- VARIABLES PUBLICAS ------
    // 1. ShootModes        -> Modos de disparo de la bala
    // 2. directionBullet   -> direccion de la bala (1 o -1)
    [HideInInspector] public enum ShootModes { simple, doble, triple, boomerang };
    [HideInInspector] public int directionBullet;

    public Bullet()
    {
        timeDead = 5;
        velocity = 1; 
        whoShot = "";
    }

    public void FixedUpdate()
    {
        transform.Translate(Vector3.up * directionBullet * velocity);
    }

    //metodo que destruye la bala segun su tiempo de vida
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(timeDead);
        Destroy(gameObject);
    }

    //Metodo que recibe quien ha disparado la bala y el modo de bala
    public void shootBullet(ShootModes shootMode, GameObject c)
    {
        whoShot = c.tag;

        if (!c.tag.Equals("Player"))
        {
            sound = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(2).GetComponent<AudioSource>();
            directionBullet = -1;
        }
        else {

            sound = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(4).GetComponent<AudioSource>();
            directionBullet = 1;
        }

        if (shootMode != ShootModes.boomerang)
        {
            sound = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(3).GetComponent<AudioSource>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), c.GetComponent<Collider2D>());
        }
        sound.enabled = true;
        sound.Play();
        StartCoroutine(destroyBullet());
    }

    //funcion que controla las colisiones de la bala
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerShip>().bajar_vida();
            StartCoroutine(die());
        }
        else if (collision.gameObject.tag.Equals("Barrier"))
        {
            collision.GetComponent<barrier>().quitarVida();
            StartCoroutine(die());
        }
        else if ((collision.gameObject.tag.Equals("Enemy_Raso") || collision.gameObject.tag.Equals("Enemy_Superior") || collision.gameObject.tag.Equals("Boss")) && whoShot.Equals("Player"))
        {
            collision.GetComponent<EnemyShip>().bajar_vida();
            StartCoroutine(die());
        }
        else if (collision.gameObject.tag.Equals("Shield") && !whoShot.Equals("Player"))
        {
            StartCoroutine(die());
        }
    }
    
    //funcion que destruye la bala cambiando el spirte y generando el sonido
    IEnumerator die()
    {
        velocity = 0;
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<Animator>().transform.localScale = new Vector2(4, 4);
        GetComponent<Animator>().enabled = true;

        sound = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(1).GetComponent<AudioSource>();
        sound.enabled = true;
        sound.Play();
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(gameObject);
    }

    //funcion que rota el collinder del disparo boomerang para que se adapte al sprite
    private void rotateCollinder()
    {
        transform.GetChild(0).Rotate(new Vector3(0,0, transform.GetChild(0).rotation.z - 90));
    }
}

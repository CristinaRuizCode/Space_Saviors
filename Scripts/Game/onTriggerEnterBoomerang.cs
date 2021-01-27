using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerEnterBoomerang : MonoBehaviour
{
    //------------- VARIABLES PRIVADAS ------------
    // 1. explosion     -> Gameobject de la explosion generada por contacto con el boomerang
    // 2. cancath       -> Varaibale para controlar cuando el player puede recoger el boomerang
    GameObject explosion;
    bool canCath = false;

    private void Awake()
    {
        explosion = Resources.Load<GameObject>("Explosions/Explosion");
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //si es boomerang, comprobar si he llegado al final de la pantalla para cambiar direcion bala
        if (collision.gameObject.name.Equals("MaxRangeShotBoomerang"))
        {
            transform.parent.GetComponent<Bullet>().directionBullet = -1;
        }
        else if (collision.gameObject.tag.Equals("Enemy_Raso") || collision.gameObject.tag.Equals("Enemy_Superior") || collision.gameObject.tag.Equals("Boss"))
        {
            StartCoroutine(explosionWhenHit());
            collision.GetComponent<EnemyShip>().bajar_vida();
        }
        else if (collision.gameObject.tag.Equals("Barrier"))
        {
            StartCoroutine(explosionWhenHit());
            collision.GetComponent<barrier>().quitarVida();
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            if (canCath)
            {
                StartCoroutine(recoveryEnergy());
            }
            else
            {
                canCath = true;
            }
        }
    }

    //funcion que genera una explosion donde impacte el boomerang
    IEnumerator explosionWhenHit()
    {
        GameObject explosionInst = Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), explosion.transform.rotation);

        explosionInst.transform.localScale = new Vector2(5, 5);
        AudioSource sound = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(2).GetComponent<AudioSource>();
        sound.enabled = true;
        sound.Play();
        yield return new WaitForSeconds(explosionInst.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(explosionInst);
    }

    //funcion que recupera energia si lel jugador recoger el boomerang
    IEnumerator recoveryEnergy()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>().updateEnergy(+20);
        AudioSource sound =GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(4).GetComponent<AudioSource>();
        sound.enabled = true;
        sound.Play();
        Color c = gameObject.transform.parent.GetComponent<SpriteRenderer>().color;
        c.a = 0;
        gameObject.transform.parent.GetComponent<SpriteRenderer>().color = c;
        yield return new WaitForSeconds(sound.clip.length);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}

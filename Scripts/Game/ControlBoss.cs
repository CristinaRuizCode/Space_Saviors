using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlBoss : MonoBehaviour
{
    //------------- VARIABLES PRIVADAS ------------
    // 1. speedMove                 -> velocidad de movimiento del enemigo  
    // 2. tiempoBajarVerticalmente  -> tiempo que tarda el boss en bajar un tramo verticalmente
    // 3. verticalJump              -> cantidad de espacio que el boss se desplaza verticalmente
    // 4. win                       -> bool auxiliar para cuando el player gana
    // 5. soundExplosion            -> AudioSource de explosion del boss
    float speedMove, tiempoBajarVerticalmente = 40, verticalJump;
    GameObject InstBoss;
    bool win;
    AudioSource soundExplosion;

    //------------ VARIABLES PUBLICAS -------------
    // 1. dir   -> direcion a la que se deslaza el boss (1 o -1)
    public int dir;

    private void Start()
    {
        win = false;

        //cargamos el prefab del BOSS como hijo del gameobject Enemies
        InstBoss = Instantiate(Resources.Load<GameObject>("Enemies/Boss"));
        InstBoss.transform.name = InstBoss.transform.name.Replace("(Clone)", "").Trim();
        Rigidbody2D rb = InstBoss.AddComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.freezeRotation = true;
        InstBoss.transform.SetParent(this.gameObject.transform, false);
        InstBoss.transform.localScale = InstBoss.transform.localScale * Screen.width / Screen.height;

        verticalJump = InstBoss.GetComponent<Renderer>().bounds.size.y / 4;

        //Estadisticas multiplicadas por la dificultad
        speedMove = InstBoss.GetComponent<EnemyShip>().speedMove * PlayerPrefs.GetFloat("Enemies_Speed");
        InstBoss.GetComponent<EnemyShip>().speedMove = speedMove;
        InstBoss.GetComponent<EnemyShip>().hp = InstBoss.GetComponent<EnemyShip>().hp * (int)PlayerPrefs.GetFloat("Enemies_Life");

        //destruyo las barreras que existan y las vuelvo a crear con todas las vidas
        recoveryAllBarriers();

        //Activo animacion de aparicion de boss
        StartCoroutine(startBoss());
    }

    private void FixedUpdate()
    {
        //nos movemos hasta fin 
        if (InstBoss.GetComponent<EnemyShip>().hp > 0)
        {
            transform.Translate(Vector2.right * dir * Time.deltaTime * speedMove);
        }
        else if (!win)
        {
            StartCoroutine(winGame());
            win = true;
        }
    }

    //Funcion que detruye todas las barreras (si quedan) y las reconstruye
    void recoveryAllBarriers()
    {
        GameObject barriers = GameObject.FindGameObjectWithTag("DeadZone");

        //Las destruimos
        for (int j = barriers.transform.childCount - 1; j >= 0; j--)
        {
            Destroy(barriers.transform.GetChild(j).gameObject);
        }

        //llamamos a la funcion para iniciar las barreras otra vez
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StartAndControlGame>().createBarriers();
    }

    //funcion que inicia la animacion de aparicion del boss
    IEnumerator startBoss()
    {
        PlayerShip player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>();
        //Esperamos a que aparezca para dejar que el player pueda disparar
        player.canShoot = false;
        yield return new WaitForSeconds(InstBoss.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

        player.canShoot= true;
        //comenzamos movimiento horizontal y disparos
        StartCoroutine(verticalMove());
        StartCoroutine(shoot());
    }

    //funcion de disparo del boss
    IEnumerator shoot()
    {
        while (InstBoss.GetComponent<EnemyShip>().hp > 0)
        {
            InstBoss.GetComponent<EnemyShip>().Shot();
            yield return new WaitForSeconds(Random.Range(0.5f, InstBoss.GetComponent<EnemyShip>().shootTime));
        }
    }


    //funcion de movimiento verticla del boss
    IEnumerator verticalMove()
    {
        while (InstBoss.GetComponent<EnemyShip>().hp > 0)
        {
            yield return new WaitForSeconds(tiempoBajarVerticalmente);
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalJump, 0);
        }

    }

    //funcion que realiza la animacion de muerte del boss, ejecuta el sonido y termina el juego
    IEnumerator winGame()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControlScore>().updateScore(10000);

        GameObject boss = transform.GetChild(0).gameObject;
        boss.GetComponent<PolygonCollider2D>().enabled = false;
        boss.GetComponent<Animator>().SetBool("dieBoss", true);

        soundExplosion = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(1).GetComponent<AudioSource>();
        soundExplosion.enabled = true;
        soundExplosion.Play();
        yield return new WaitForSeconds(soundExplosion.clip.length);

        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FinishGame>().WinGame());
    }
}


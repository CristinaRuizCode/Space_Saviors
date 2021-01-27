using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlEnemiesShips : MonoBehaviour
{
    //---------- VARIABLES CONTROL DE SPAWN INICIAL-------
    // 1. enemysRaso                            -> cantidad de filas de enemigos rasos
    // 2. separacionNaves_x, separacionNaves_y  -> separacion entre naves
    // 3. start_x                               -> posicion X inicial donde hacen spawn las naves
    // 4. posi_x,posi_y                         -> posiciones donde va a hacer spwan una nave
    // 5. filas, columnas                       -> cantidad de filas y columnas de enemigos
    float enemysRaso;
    float separacionNaves_x, separacionNaves_y;
    float start_x;
    float posi_x, posi_y;
    int filas = 5, columnas = 9;

    //---------- VARIABLES MOVIMIENTO VERTICAL -------
    // 1. tiempoBajarVerticalmente              -> Tiempo que lso enemigos tardan en bajar un posicion verticalmente
    // 2. dir                                   -> direcion e la que se mueven horizontalmente (1 der, -1 izq) (PUBLICA)
    // 3. speedMove                             -> Velocidad de movimiento horizontal
    // 4. verticalJump                          -> Anchura del salto vertical
    [SerializeField]float tiempoBajarVerticalmente = 25;
    public int dir;
    float speedMove, verticalJump;

    //---------- VARIABLES CONTROL DE DISPARO -------
    // 1. timeShotRaso, timeShotSuperior        -> Tiempo de disparo de enemigos
    // 2. numEnemiesRaso, numEnemiesSuperior    -> Numero de enemigos rasos y superiores
    float timeShotRaso, timeShotSuperior;
    int numEnemiesRaso, numEnemiesSuperior;


    //---------- PREFABS DE LAS NAVES ENEMIGAS -------
    // 1. prefabEnemyRaso, prefabEnemySuperior  -> prefabs de enemigos rasos y superiores
    // 2. mulSpeedMove                          -> Segun la dificultad multiplicamos el movimiento de los enemigos
    // 3. mulHp                                 -> Segun la dificultad multiplicamos la vida de los enemigos
    GameObject prefabEnemyRaso, prefabEnemySuperior;
    float mulSpeedMove;
    int mulHp;

    //-------- VARIABLE PARA SONIDO EXPLOSION --------
    // 1. soundExplosion    -> Audiosource de la explosion de una nave
    AudioSource soundExplosion;


    private void Awake()
    {
        enemysRaso = PlayerPrefs.GetFloat("Enemies_Amount");
        prefabEnemyRaso = Resources.Load<GameObject>("Enemies/Enemy_Raso");
        prefabEnemySuperior = Resources.Load<GameObject>("Enemies/Enemy_Superior");

        mulHp = (int)PlayerPrefs.GetFloat("Enemies_Life");
        mulSpeedMove = PlayerPrefs.GetFloat("Enemies_Speed");

        timeShotRaso = prefabEnemyRaso.GetComponent<EnemyShip>().shootTime;
        timeShotSuperior = prefabEnemySuperior.GetComponent<EnemyShip>().shootTime;

        numEnemiesRaso = (int)enemysRaso * columnas;
        numEnemiesSuperior = (filas * columnas) - numEnemiesRaso;

        soundExplosion = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Start()
    {
        //Calculamos la posicion de la interfaz izquierda en base a su anchura para obtener el punto de inicio de spawn de las naves enemigas
        Transform hud = GameObject.FindGameObjectWithTag("HUD").transform;
        RectTransform RTLeft= hud.GetChild(2).GetComponent<RectTransform>();
        Rect RLeft = RTLeft.rect;

        Vector3 positionLeft = Camera.main.ScreenToWorldPoint(new Vector3(RTLeft.anchoredPosition.x + RLeft.width, 0,0));
        start_x = positionLeft.x;

        posi_x = start_x;
        posi_y = 0;

        verticalJump = prefabEnemySuperior.GetComponent<Renderer>().bounds.size.y/ 2;
        dir = 1;

        //instanciamos una nave superior(que ocupa mas), escalamos segun pantalla y cogemos sus bounds, despues destruimos
        GameObject b = Instantiate(prefabEnemySuperior, new Vector3(0, 0, 0), Quaternion.identity);
        changeScale(b.transform);

        //calculamos la separacion por columnas y por filas respecto a los bounds obtenidos
        separacionNaves_x = b.GetComponent<Renderer>().bounds.size.x;
        separacionNaves_y = b.GetComponent<Renderer>().bounds.size.y;

        Destroy(b);

        //NAVES ENEMIGAS

        //enemigos rasos
        for (int i = 0; i < enemysRaso; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                instanciarNave(prefabEnemyRaso);
            }
            updatePosition();
        }

        //enemigos superiores
        for (int i = 0; i < filas-enemysRaso; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                instanciarNave(prefabEnemySuperior);
            }

            updatePosition();
        }

        StartCoroutine(verticalMove()); //comienza el movimiento vertical
        if (enemysRaso > 0)
        {
            StartCoroutine(EnemyShot(prefabEnemyRaso.name,0,(int)numEnemiesRaso -1, timeShotRaso)); //comienzan los disparos RASOS
        }
        
        if (enemysRaso < 5)
        {
            StartCoroutine(EnemyShot(prefabEnemySuperior.name,numEnemiesRaso, numEnemiesRaso + (int)numEnemiesSuperior -1, timeShotSuperior)); //comienzan los disparos SUPERIORES
        }

        speedMove = transform.GetChild(transform.childCount-1).GetComponent<EnemyShip>().speedMove;
    }

    public void FixedUpdate()
    {
        //Si tenemos enemigos
        if (numEnemiesRaso + numEnemiesSuperior > 0)
        {
            transform.Translate(Vector2.right * dir * Time.deltaTime * speedMove);
        }
        else
        {
            //El player no podrá disparar (boomerang tampoco) hasta que aparezca el boss, pero si se podrá mover, gastar escudos, dodges...
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShip>().canShoot = false;

            //centramos este objeto en 0, 0
            transform.position = new Vector2(0, 0);
            //desactivamos este script y activamos el del boss
            GetComponent<ControlBoss>().enabled = true;
            GetComponent<ControlEnemiesShips>().enabled = false;
        }
    }

    //funcion que esocge unos enemigos (rasos o superiores) al azar para disparar entre intervalos aleatorios (en base al tiempo de disparo de las naves)
    IEnumerator EnemyShot(string whoShot,int start, int numEnemies, float timeShot)
    {
        int nm = 0;
        int random = 0;
        bool go = true;
        while (go)
        {
            if (prefabEnemyRaso.name.Equals(whoShot))
            {
                numEnemies = numEnemiesRaso;
            }
            else
            {
                start = numEnemiesRaso;
                numEnemies = numEnemiesRaso + (int)numEnemiesSuperior -1 ;
            }
            nm = numEnemies / 5;
            nm = Random.Range(1, nm);


            //escogemos naves al azar
            for (int i = 0; i < nm; i++)
            {
                if (transform.childCount > 0 && (numEnemiesRaso > 0 || numEnemiesSuperior> 0))
                {
                    random = Random.Range(start, numEnemies);
                    if (random < transform.childCount)
                    {
                        transform.GetChild(random).GetComponent<EnemyShip>().Shot();
                    }
                    
                }
                yield return new WaitForSeconds(Random.Range(0.5f, timeShot / 2));

            }

            yield return new WaitForSeconds(timeShot);

            if (prefabEnemyRaso.name.Equals(whoShot))
            {
                if (numEnemiesRaso == 0)
                {
                    go = false;
                }
            }
            else
            {
                if (numEnemiesSuperior == 0)
                {
                    go = false;
                }
            }

        }
    }


    //funcion que cambia la escala de una nave
    private void changeScale(Transform ship)
    {
        ship.transform.localScale = ship.transform.localScale * Screen.width / Screen.height;
    }

    //funcion que actualiza la posicion de spwan de una nave enemiga
    private void updatePosition()
    {
        posi_y = posi_y + separacionNaves_y;
        posi_x = start_x;
    }

    //funcion que instancia una nave enemiga
    private void instanciarNave(GameObject ob)
    {
        
        GameObject x = Instantiate(ob, new Vector3(posi_x, posi_y, 0), Quaternion.identity);
        Rigidbody2D rb = x.AddComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.freezeRotation = true;
        changeScale(x.transform);
        x.transform.SetParent(this.gameObject.transform, true);
        x.GetComponent<EnemyShip>().speedMove = x.GetComponent<EnemyShip>().speedMove * mulSpeedMove;
        x.GetComponent<EnemyShip>().hp = x.GetComponent<EnemyShip>().hp * mulHp;
        x.transform.name = x.transform.name.Replace("(Clone)", "").Trim();
        posi_x = posi_x + separacionNaves_x;
    }

    //funcion que realiza el movimiento vertical descendente de las naves
    IEnumerator verticalMove()
    {
        while (numEnemiesRaso + numEnemiesSuperior > 0)
        {
            yield return new WaitForSeconds(tiempoBajarVerticalmente);
            transform.position = new Vector3(transform.position.x, transform.position.y - verticalJump, 0);
        }

    }

    //funcion actualiza el numero de enemigos rasos y superiores que existen
    public void destroyChild(GameObject ship)
    {
        if(ship.tag.Equals("Enemy_Raso")){
            numEnemiesRaso--;
        }
        else
        {
            numEnemiesSuperior--;
        }

        soundExplosion.enabled = true;
        soundExplosion.Play();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControlScore>().updateScore(100);
        Destroy(ship);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

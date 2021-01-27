using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShip : Ship
{
    //--------------- VARIABLES PUBLICAS ------------
    // 1. shields                               -> escudos de la nave
    // 2. dodges                                -> dodges de la nave
    // 3. energy                                -> energia d el nave
    // 4. actualEnergy                          -> energia actual de la nave
    // 5. timeShot                              -> tiempo auxiliar para controlar el disparo
    // 6. timeBoomerangShot                     -> tiempo auxiliar para controlar el disparo boomerang
    // 7. boomerangTime                         -> tiempo de CD para el disparo boomerang
    // 8. canShoot                              -> bool para controlar que el player no dispare mientras el boss hace spawn
    public int shields, dodges;
    public float energy;
    [HideInInspector] public float actualEnergy;
    public float timeShot, timeBoomerangShot;
    public float boomerangTime;
    public bool canShoot;

    //----------------- VARIABLES PRIVADAS --------
    // 1. timeShield            -> duracion de un escudo
    // 2. timeDodge             -> tiempo para realizar el dodge
    // 3. time                  -> tiempo auxiliar para controlar el dodge
    // 4. timeEnergy            -> tiempo auxiliar para recuperar energia
    // 5. timeRecoveryEnergy    -> tiempo que se tarda en recuperar energia
    // 6. shieldActivate        -> bool para indicar si un escudo esta activado
    // 7. dodgeActivate         -> bool para indicar si un dodge esta activado
    // 8. aux                   -> bool axiliar para controlar el dodge
    float timeShield, timeDodge, time ,timeEnergy, timeRecoveryEnergy;
    bool shieldActivate, dodgeActivate, aux;
    

    //----- OBJECTOS Y PREFABS -----------
    GameObject prefabShield, startAndControlGame, ammoBoomerang, ammoOriginal;  
    string dodgeDir;
    Bullet.ShootModes auxiliarShootMode;

    Animator warning;


    private void Awake()
    {
        warning = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(7).GetComponent<Animator>();
        actualEnergy = energy;
        timeShield = 3;
        shieldActivate = false;
        canShoot = true;

        timeEnergy = 0;
        timeRecoveryEnergy = 3;

        dodges = 2;
        timeDodge = 0.2f;
        time = timeDodge + 0.1f;
        aux = false;
        dodgeActivate = false;

        timeShot = shootTime;
        boomerangTime = 5;
        timeBoomerangShot = boomerangTime;
        ammoOriginal = Resources.Load<GameObject>("Bullets/"+ammo.name);
        ammoBoomerang = Resources.Load<GameObject>("Bullets/BoomerangBullet");

        prefabShield = Resources.Load<GameObject>("Shields/Shield");
        startAndControlGame = GameObject.FindGameObjectWithTag("MainCamera");

    }

    public void FixedUpdate()
    {
        //DODGE
        if (dodgeActivate)
        {
            dodgeMovement();
        }

        //HORIZONTAL/DODGE
        if (Input.GetAxis("Horizontal") != 0)
        {
            Movement(Input.GetAxis("Horizontal"));

            if (((Input.GetKeyDown(KeyCode.A) && !dodgeActivate) || (Input.GetKeyDown(KeyCode.D) && !dodgeActivate)) && !shieldActivate && dodges > 0)
            {
                if (time <= timeDodge && time != 0)
                {
                    if (actualEnergy - 30 >= 0)
                    {
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            dodgeDir = "dodgeLeft";
                        }
                        else
                        {
                            dodgeDir = "dodgeRight";
                        }
                        dodgeActivate = true;
                        updateEnergy(-30);
                        StartCoroutine(dodge(dodgeDir));
                    }
                    else
                    {
                        StartCoroutine(showWarning());
                    }
                }
                aux = false;
                time = 0;
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                
                if (!dodgeActivate)
                {
                    aux = true;
                }
            }
        }

        if (aux)
        {
            time += Time.deltaTime;
            if (time > timeDodge) //para que no se tome el tiempo infinitamente
            {
                aux = false;
            }
        }

        
        //SHOT
        if (Input.GetAxis("Normal_Shot") != 0 && shootTime < timeShot && canShoot)
        {
            Shot();
            timeShot = 0;
        }

        //BOOMERANG
        if (Input.GetAxis("Boomerang_Shot") != 0 &&  boomerangTime < timeBoomerangShot && canShoot)
        {
            if (actualEnergy - 50 >= 0)
            {
                //cambiar tipo de disparo por boomerang
                auxiliarShootMode = shootMode;
                ammo = ammoBoomerang;
                shootMode = Bullet.ShootModes.boomerang;

                //disparar
                Shot();
                timeBoomerangShot = 0;
                updateEnergy(-50);
            }
            else
            {
                StartCoroutine(showWarning());
            }
            
        }

        //ESCUDO
        if (Input.GetAxis("Shield") != 0 && !shieldActivate && shields>0 && !dodgeActivate)
        {
            if (actualEnergy - 30 >= 0)
            {
                updateEnergy(-30);
                StartCoroutine(activateShield());
            }
            else
            {
                StartCoroutine(showWarning());
            }
        }
        
        timeShot += Time.deltaTime;
        timeBoomerangShot += Time.deltaTime;
        timeEnergy += Time.deltaTime;

        //ENERGIA
        if (timeEnergy > timeRecoveryEnergy)
        {
            if (actualEnergy + 10 > energy)
            {
                actualEnergy = 100;
            }
            else
            {
                actualEnergy += 10;
            }
            timeEnergy = 0;
        }
    }

    protected override void Movement(float input)
    {
        base.Movement(input);
    }

    protected void dodgeMovement()
    {
        int dir;

        if (dodgeDir.Equals("dodgeLeft"))
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.right * dir * 30; 
    }

    public void changeDodgeActivate()
    {
        dodgeActivate = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.left * 0;
    }

    //funcion que realiza la animacion de dodge
    IEnumerator dodge(string dodgeDir)
    {
        //Quitar un dodge del jugador y de la interfaz
        dodges--;
        startAndControlGame.GetComponent<StartAndControlGame>().quitarDodge();

        AudioSource dodge = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(5).GetComponent<AudioSource>();
        dodge.enabled = true;
        dodge.Play();

        Animator animator = transform.GetComponent<Animator>();
        animator.SetBool(dodgeDir, true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        animator.SetBool(dodgeDir, false);
    }

    public override void bajar_vida()
    {
        base.bajar_vida();
        //quitar una vida en la interfaz
        startAndControlGame.GetComponent<StartAndControlGame>().quitarVida();
        
        if (hp == 0)
        {
            StartCoroutine(losseGame());
        }
    }

    public override void Shot()
    {
        //Si es boomerang, instanciamos bala y volvemos al modo de disparo normal
        if (shootMode == Bullet.ShootModes.boomerang)
        {
            instanciarBala(1, 0, transform.GetComponent<Renderer>().bounds.size.y / 2);
            //volver a poner el ammo original de la nave
            ammo = ammoOriginal;
            shootMode = auxiliarShootMode;
        }
        else
        {
            base.Shot();
        }
    }
    
    //funcion que activa un escudo
    IEnumerator activateShield()
    {
        shieldActivate = true;
        //Quitar un escudo del jugador y de la interfaz
        shields--;
        startAndControlGame.GetComponent<StartAndControlGame>().quitarEscudo();

        //activar prefab de escudo y ponerlo de hijo de la nave
        GameObject instShield = Instantiate(prefabShield, new Vector3(0, 0, 0), Quaternion.identity);
        instShield.AddComponent<PolygonCollider2D>();
        instShield.GetComponent<PolygonCollider2D>().isTrigger = true;
        Rigidbody2D rb = instShield.AddComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.freezeRotation = true;
        instShield.transform.SetParent(transform, false);

        //Cuando se vaya a acabar aumentamos velocidad de animacion
        yield return new WaitForSeconds(timeShield-1);
        Color color = instShield.GetComponent<SpriteRenderer>().color;
        color.a = color.a - 0.3f;
        instShield.GetComponent<SpriteRenderer>().color = color;
        instShield.GetComponent<Animator>().speed = 3;

        yield return new WaitForSeconds(timeShield);

        Destroy(instShield);
        shieldActivate = false;
    }

    //funcion que muestra que no hay suficiente energia
    private IEnumerator showWarning()
    {
        warning.SetBool("warning",true);
        yield return new WaitForSeconds(1.5f);
        warning.SetBool("warning", false);
    }


    //funcion que actualiza la energia
    public void updateEnergy(float energyUsed)
    {
        if (actualEnergy + energyUsed > energy)
        {
            actualEnergy = energy;
        }
        else
        {
            actualEnergy += energyUsed;
        }
    }


    //funcion que se activa cuando el player muere
    IEnumerator losseGame()
    {
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("playerDie", true);
        AudioSource explosion = GameObject.FindGameObjectWithTag("MusicAndSounds").transform.GetChild(1).GetComponent<AudioSource>();
        explosion.enabled = true;
        explosion.Play();
        yield return new WaitForSeconds(explosion.clip.length);

        StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FinishGame>().LoseGame());
    }
}

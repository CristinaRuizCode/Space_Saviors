using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartAndControlGame : MonoBehaviour
{
    //-------------- VARIABLES PRIVADAS ----------------
    // 1. interfaceLeft, interfaceRight  -> HUD izquierdo y derecho
    // 2. numBarriers                    -> numero de barreras
    // 3. escalaBarriers,offsetBarriers  -> escala de las barreras y el offset
    // 4. energy                         -> energia de la nave en el HUD
    // 5. porcentageEnergy               -> porcentaje de energia de la nave
    // 6. player                         -> El jugador
    // 7. maxShields, maxHp, maxDodges   -> maximo de escudos, de vida y de dodges
    Transform interfaceLeft, interfaceRight;
    int numBarriers = 5;
    float escalaBarriers = 0.4f, offsetBarriers = 5;
    float energy;
    Text porcentageEnergy;
    PlayerShip player;
    int maxShields = 2, maxHp = 3, maxDodges= 2;

    void Start()
    {
        //---------------- MUSICA Y EFECTOS DE SONIDO -------------------
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().loadVol();

        //----------------- BARRERAS ----------------
        createBarriers();

        //----------------- NAVE JUGADOR ----------------
        GameObject shipPlayer = Instantiate(Resources.Load<GameObject>("Ships/" + PlayerPrefs.GetString("ShipName")), new Vector3(0, -42, 0), Quaternion.identity);
        shipPlayer.GetComponent<PlayerShip>().enabled = true;
        shipPlayer.transform.localScale = shipPlayer.transform.localScale * Screen.width / Screen.height;
        Rigidbody2D rb = shipPlayer.AddComponent<Rigidbody2D>();
        shipPlayer.AddComponent<PolygonCollider2D>();
        rb.simulated = true;
        rb.freezeRotation = true;

        player = shipPlayer.GetComponent<PlayerShip>();

        //----------------- INTERFAZ IZQUIERDA ----------------
        interfaceLeft = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(2);

        // ENERGIA
        porcentageEnergy = interfaceLeft.GetChild(4).GetComponent<Text>();

        player.transform.name = player.transform.name.Replace("(Clone)", "").Trim();
        //cambiamos la imagen de las vidas por los de la nave (ICON)
        string nameShip = player.name;

        switch (nameShip)
        {
            case "Bom":
                nameShip= "icon-plane-red";
                break;
            case "Cactilio":
                nameShip = "icon-plane-green";
                break;
            case "Moguri":
                nameShip = "icon-plane-purple";
                break;
            case "Excalibur":
                nameShip = "icon-plane-white";
                break;
            case "Shiva":
                nameShip = "icon-plane-blue";
                break;
        }
        Sprite icon = Resources.Load<Sprite>("Icons/" + nameShip);
        for (int i = 0; i < player.hp; i++)
        {
            interfaceLeft.GetChild(1).GetChild(i).GetComponent<Image>().sprite = icon;
        }

        //VIDAS
        for (int i = maxHp; i > player.hp; i--)
        {
            Destroy(interfaceLeft.GetChild(1).GetChild(i - 1).gameObject);
        }
        

        //----------------- INTERFAZ DERECHA ----------------
        interfaceRight = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(3);

        //ESCUDOS
        for (int i = maxShields; i > player.shields; i--)
        {
            Destroy(interfaceRight.GetChild(1).GetChild(i - 1).gameObject);
        }

        //DODGE
        nameShip = player.name;

        switch (nameShip)
        {
            case "Bom":
                nameShip = "plane_red_dodge";
                break;
            case "Cactilio":
                nameShip = "plane_green_dodge";
                break;
            case "Moguri":
                nameShip = "plane_purple_dodge";
                break;
            case "Excalibur":
                nameShip = "plane_white_dodge";
                break;
            case "Shiva":
                nameShip = "plane_blue_dodge";
                break;
        }

        Sprite dodge = Resources.Load<Sprite>("DodgesInterface/" + nameShip);
        for (int i = 0; i < maxDodges; i++)
        {
            interfaceRight.GetChild(4).GetChild(i).GetComponent<Image>().sprite = dodge;
        }

    }
    
    //Funcion que crea las barreras
    public void createBarriers()
    {
        GameObject prefabBarrier = Resources.Load<GameObject>("Barriers/barrier");
        GameObject barriers = GameObject.FindGameObjectWithTag("DeadZone");

        //instanciamos una barrera, cambiamos su escala segun pantalla, cogemos sus bounds
        GameObject barrier = Instantiate(prefabBarrier, new Vector3(0, 0, 0), Quaternion.identity);
        barrier.transform.localScale = barrier.transform.localScale * Screen.width / Screen.height * escalaBarriers;
        barrier.transform.SetParent(barriers.transform, false);

        float separacionBarriers = Screen.width / Screen.height - numBarriers * barrier.GetComponent<Renderer>().bounds.max.x - offsetBarriers;
        float posi_max = separacionBarriers;

        for (int i = 0; i < (numBarriers - 1) / 2; i++)
        {
            GameObject b1 = Instantiate(prefabBarrier, new Vector3(posi_max, 0, 0), Quaternion.identity);
            instanciate(b1, barriers);
            GameObject b2 = Instantiate(prefabBarrier, new Vector3(-posi_max, 0, 0), Quaternion.identity);
            instanciate(b2, barriers);

            posi_max = posi_max * 2;
        }
    }

    //funcion que instancia las barreras
    private void instanciate(GameObject barrier, GameObject barriers)
    {
        barrier.transform.localScale = barrier.transform.localScale * Screen.width / Screen.height * escalaBarriers;
        barrier.AddComponent<PolygonCollider2D>();
        barrier.GetComponent<PolygonCollider2D>().isTrigger = true;
        barrier.transform.SetParent(barriers.transform, false);
    }

    private void FixedUpdate()
    {
        //Actualizamos CD de los disparos en el HUD
        interfaceRight.GetChild(8).GetComponent<Image>().fillAmount = player.timeShot / player.shootTime;
        interfaceRight.GetChild(10).GetComponent<Image>().fillAmount = player.timeBoomerangShot / player.boomerangTime;

        //Actualizamos ENERGY en el HUD
        energy = player.actualEnergy / player.energy;
        interfaceLeft.GetChild(5).GetComponent<Slider>().value = energy;
        energy *= 100;
        porcentageEnergy.text = energy.ToString("0") + "%";
    }

    //-------------- FUNCIONES PARA ACTUALIZAR HUD ------------------------
    public void quitarVida()
    {
        Destroy(interfaceLeft.GetChild(1).GetChild(interfaceLeft.GetChild(1).childCount - 1).gameObject); 
    }

    public void quitarEscudo()
    {
        Destroy(interfaceRight.GetChild(1).GetChild(interfaceRight.GetChild(1).childCount - 1).gameObject);
    }

    public void quitarDodge()
    {
        Destroy(interfaceRight.GetChild(4).GetChild(interfaceRight.GetChild(4).childCount - 1).gameObject);
    }
}

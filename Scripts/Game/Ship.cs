using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // ----- VARIABLES PROTEGIDAS ------
    // 1. hp    -> vida de la nave
    // 2. speedMove -> Velocidad de movimiento
    // 3. shootTime -> velocidad a la que dispara
    // 4. ShootMode -> Tipo de disparo de la nave
    // 5. ammo      -> municion de la nave
    public int hp;
    [SerializeField]public float speedMove;
    [SerializeField]public float shootTime;
    [SerializeField]protected Bullet.ShootModes shootMode;

    [SerializeField]protected GameObject ammo;

    public Ship()
    {

    }
    public Ship(int h, float sm, float st)
    {
        hp = h;
        speedMove = sm;
        shootTime = st;
    }
    
    protected virtual void Movement(float input) {

        transform.Translate(Vector2.right * input * Time.deltaTime * speedMove);
    }

    //funcion de disparo
    public virtual void Shot()
    {
        float offsetX=0,offsetY=0;

        if (transform.tag.Equals("Player"))
        {
            offsetY = transform.GetComponent<Renderer>().bounds.size.y / 2;
        }
        else
        {
            offsetY = -transform.GetComponent<Renderer>().bounds.size.y / 2;
        }

        switch (shootMode)
        {
            case Bullet.ShootModes.simple:
                instanciarBala(1, offsetX, offsetY);
                break;
            case Bullet.ShootModes.doble:
                offsetX = transform.GetComponent<Renderer>().bounds.size.x / 2;
                instanciarBala(2, offsetX, offsetY);
                break;
            case Bullet.ShootModes.triple:
                offsetX = transform.GetComponent<Renderer>().bounds.size.x / 2;
                instanciarBala(3, offsetX, offsetY);
                break;
        }

    }

    //funcion que instancia las balas
    protected void instanciarBala(int numBalas, float osX, float osY)
    {
        GameObject bulletInst;

        for (int i = 0; i < numBalas; i++)
        {
            bulletInst = Instantiate(ammo, new Vector3(transform.position.x + osX, transform.position.y + osY, transform.position.z), ammo.transform.rotation);
            
            bulletInst.GetComponent<Bullet>().shootBullet(shootMode, gameObject);
            if (i == 0)
            {
                osX = -osX;
                
            }
            else if( i == 1)
            {
                osX = osX - osX;
            }
            
        }
    }

    //funcion para bajar la vida
    public virtual void bajar_vida()
    {
        hp--;
    }


}

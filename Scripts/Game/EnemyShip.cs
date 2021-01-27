using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{

    protected override void Movement(float inp)
    {
        base.Movement(inp);
    }

    public override void Shot()
    {
        base.Shot();
    }

    public override void bajar_vida()
    {
        base.bajar_vida();
        
        
        if (hp==0)
        {
            if (!this.gameObject.tag.Equals("Boss"))
            {
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        GameObject.FindGameObjectWithTag("Enemies").GetComponent<ControlEnemiesShips>().destroyChild(this.gameObject);
    }
}

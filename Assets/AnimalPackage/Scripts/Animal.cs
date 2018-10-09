using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : AiCharacter
{
    public GameObject[] drops;
    protected override void AnimAttack()
    {
        base.AnimAttack();
        animator.SetTrigger("Attack");
    }

    protected override void AnimDie()
    {
        if(drops.Length > 0)
        {
            int index = Random.Range(0, drops.Length - 1);
            Instantiate(drops[index], transform.position, transform.rotation);
        }
        
        base.AnimDie();
        animator.SetTrigger("Death");
    }


    protected override void AnimMove(float speed)
    {
        base.AnimMove(speed);
        animator.SetFloat("Speed", speed);
    }


}

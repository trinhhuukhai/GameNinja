using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update

    private float hp;
    private string currentAnimName;
    [SerializeField] private Animator anim;



    public bool isDeath => hp >= 0;

    private void Start()
    {
        OnInit();
    }
    public virtual void OnInit() { 
        hp = 100;
    }

    public virtual void OnDespawn() { }

    public virtual void OnHit(float damage) {
        if(!isDeath)
        {
            hp -= damage;

            if(isDeath)
            {
                OnDeath();
            }
        }
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    protected virtual void OnDeath()
    {

    }


}

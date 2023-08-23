using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{

    public Rigidbody2D rb;
    private Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
        Invoke(nameof(OnDespawn), 4f);
    }

    // Update is called once per frame
    public void OnInit()
    {

        rb.velocity = transform.right * 5f;
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);

            OnDespawn();
        }
    }
}
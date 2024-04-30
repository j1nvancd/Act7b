using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Particles")]
    public GameObject explotionParticle;

    [Header("Ball Info")]
    public int damageQuantity;
    public float activeTime;

    private float shootTime; //start shoot time
    [HideInInspector] public bool isPlayer;

    //event when the object change from active=false to true
    private void OnEnable()
    {
        shootTime = Time.time; //exact time when the shoot start    
    }

    private void Update()
    {
        //if reach the activeTime gameobject active = false
        if(Time.time - shootTime >= activeTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //desactive ball
        gameObject.SetActive(false);

        //Instatiate particles
        GameObject particles = Instantiate(explotionParticle, transform.position, Quaternion.identity);
            
        Destroy(particles, 1f);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageQuantity);
        }else if (other.CompareTag("Player"))
        {
            if(!isPlayer)
                other.GetComponent<PlayerController>().DamagePlayer(damageQuantity);
            
        }
    }



}

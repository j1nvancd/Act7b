using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform outPosition;
    public int currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float ballSpeed;

    public float shootFrequency;

    [Header("Audio")]
    public AudioSource shootSound; // AudioSource para reproducir el sonido del disparo
    public AudioClip shootClip; // AudioClip que se reproducirá cuando se dispare


    private ObjectPool objectPool;
    private float lastShootTime;
    private bool isPlayer;

    private EnemyController enemyController;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;

        //if I am the Player
        if (GetComponent<PlayerController>())
            isPlayer = true;
        else if (enemyController = GetComponent<EnemyController>())
        {
            shootFrequency = enemyController.enemyData.shootFrequency;
        }
        
        
        objectPool = GetComponent<ObjectPool>();
    }

    /// <summary>
    /// check if it is possible to shoot looking the frequency
    /// </summary>
    /// <returns>bool</returns>
    public bool CanShoot()
    {
        if (Time.time - lastShootTime >= shootFrequency)
            if (currentAmmo > 0 || infiniteAmmo)
                return true;

        return false;
    }

    public void Shoot()
    {
        //update lastShootTime
        lastShootTime = Time.time;
        //reduce the ammo
        currentAmmo--;

        // Reproducir el sonido del disparo
        shootSound.PlayOneShot(shootClip);

        //Get a new ball
        GameObject ball = objectPool.GetGameObject();

        //assign isPlayer to the ball
        ball.GetComponent<BallController>().isPlayer = isPlayer;

        //Put the ball at the outPosition
        ball.transform.position = outPosition.position;
        ball.transform.rotation = outPosition.rotation;


        if (isPlayer) { 
            //create a ray from the camera trough the middle of your screen
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        
            RaycastHit hit;
            Vector3 targetPoint;
            //Check if you are pointing to something and adjust the direction
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(5);//5m

            //Give velocity to the Ball
            ball.GetComponent<Rigidbody>().velocity = (targetPoint - ball.transform.position).normalized *  ballSpeed;

        }
        //Enemy shoot
        else
        {
            ball.GetComponent<Rigidbody>().velocity = (ball.transform.forward) * ballSpeed;

            //TODO randomly shoot to player heart
        }
    }

}

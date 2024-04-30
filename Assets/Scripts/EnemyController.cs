using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [Header("EnemyData")]
    public EnemyData enemyData;
    private int currentLife;
    public int maxLife;
    public int enemyScorePoint;

    [Header("EnemyMovement")]
    private float speed;
    public float attackRange;
    public float yPathOffSet;//explain later
    public float followRange;
    public bool alwaysFollow;

    private List<Vector3> listPath;

    private WeaponController weaponController;
    private PlayerController target;//player

    private Renderer enemyRenderer;

    [Header("Audio")]
    public AudioSource shootSound; // AudioSource para reproducir el sonido 
    public AudioClip dmgClip; // AudioClip que se reproducirá cuando se haga el daño

    private void Start()
    {
        //take data from ScriptableObject
        speed = enemyData.speed;
        currentLife = enemyData.enemyLife;
        enemyRenderer = GetComponent<Renderer>();
        enemyRenderer.material = enemyData.enemyMaterial;

        weaponController = GetComponent<WeaponController>();
        target = FindObjectOfType<PlayerController>();

        InvokeRepeating(nameof(UpdatePaths), 0.0f, 0.5f);
    }

    /// <summary>
    /// Update every 0.5 sec the path points to the target
    /// </summary>
    private void UpdatePaths()
    {
        //Create a MeshPath object
        NavMeshPath navMeshPath = new NavMeshPath();
        //Calculate all the points in the path to reach the target
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshPath);
        //Convert all the points to the List 
        listPath = navMeshPath.corners.ToList();
    }

    private void Update()
    {
        //check the distance betweeen enemy and player
        float distance = Vector3.Distance(transform.position, target.transform.position);

        //calculate if can follow and if can attack
        if (alwaysFollow || distance < followRange)
        {
            transform.LookAt(target.transform);

            if (distance > attackRange) ReachTarget();        
            else if (distance <= attackRange)
            {
                if (weaponController.CanShoot())
                    weaponController.Shoot();
            }
        }

        
        /*
        //Calculate the direction vector magnitude 1 
        //between enemy and player
        Vector3 direction = (target.transform.position - transform.position).normalized;
        //rotate the enemy to the player
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * angle;
        */   
      


       
            
    }

    /// <summary>
    /// Move the enemy to reach the target (Player)
    /// following the path calulated
    /// </summary>
    private void ReachTarget()
    {
        //if is not a path dont do anything
        if (listPath.Count == 0)
            return;

        //Calculate the new position looking at the listPath
        transform.position = Vector3.MoveTowards(transform.position, listPath[0] + new Vector3(0, yPathOffSet , 0)  , speed * Time.deltaTime);

        //everytime reach a position remove that position
        if (transform.position == listPath[0] + new Vector3(0, yPathOffSet, 0))
            listPath.RemoveAt(0);

    }
    /// <summary>
    /// Reduce the Enemy current live until die
    /// </summary>
    /// <param name="quantity">number of lives</param>
    public void DamageEnemy(int quantity)
    {
        // Reproducir el sonido de daño
        shootSound.PlayOneShot(dmgClip);

        GameManager.instance.UpdateScore(enemyScorePoint);

        currentLife -= quantity;
        if (currentLife <= 0)
            Destroy(gameObject);
    }

}

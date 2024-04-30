using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public int enemyNumber;
    public int enemyCount;
    public Transform enemyInitialPosition;
    public Vector3 enemyArea;
    public GameObject enemyObject;

    private Coroutine enemyCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Start the coroutine only if not started yet
        if (Input.GetKeyDown(KeyCode.E) && enemyCoroutine == null)
        {
            enemyCoroutine = StartCoroutine(GenerateEnemies());
        }
        //if click again E and is in use
        else if (Input.GetKeyDown(KeyCode.E) && enemyCoroutine != null)
        {
            StopCoroutine(enemyCoroutine);
            enemyCoroutine = null;
        }

            
        
    }

    IEnumerator GenerateEnemies()
    {
        InstantiateEnemy();
        yield return new WaitForSeconds(1.5f);

        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= enemyNumber)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length < enemyNumber);
            enemyCoroutine = StartCoroutine("GenerateEnemies");
        }
        else if (GameObject.FindGameObjectsWithTag("Enemy").Length < enemyNumber)
        {
            enemyCoroutine = StartCoroutine("GenerateEnemies");
        }
    }

    private void InstantiateEnemy()
    {
        float positionX = Random.Range(enemyInitialPosition.position.x, enemyInitialPosition.position.x + 30);
        float positionZ = Random.Range(enemyInitialPosition.position.z, enemyInitialPosition.position.z - 9);
        Instantiate(enemyObject, new Vector3(positionX, 1f, positionZ), Quaternion.identity);
    }
}

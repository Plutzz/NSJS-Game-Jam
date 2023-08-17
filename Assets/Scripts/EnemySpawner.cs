using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swarmerPrefab;
    [SerializeField]
    private GameObject bigSwarmerPrefab;

    [SerializeField]
    private float swarmerInterval = 3.5f;
    [SerializeField]
    private float bigSwarmerInterval = 10f;

    [SerializeField]
    private float playerSafeRadius = 5f;

    public static int numEnemies; //Increment when spawn, decrement it when enemy is killed

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));
        StartCoroutine(spawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        Vector3 _spawnPoint = new Vector3(0, 0, 0) + transform.position;
        float _distanceFromPlayer = (PlayerController.playerController.gameObject.transform.position - _spawnPoint).magnitude;
        
        if(_distanceFromPlayer > playerSafeRadius)
        {
            GameObject newEnemy = Instantiate(enemy, _spawnPoint, Quaternion.identity);
            numEnemies++;
        }
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}

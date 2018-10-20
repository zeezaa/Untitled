using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Enemies
{
    public GameObject smallEnemyRight, smallEnemyLeft, bigEnemyRight, bigEnemyLeft;
}

public class GameController : MonoBehaviour 
{
    public Enemies enemies; 
    public GameObject leftSpawnPoint;
    public GameObject rightSpawnPoint;
    public Text scoreText;
    public Text waveText;   
    public static int score = 0;
    public static int wave = 0;
    
    int smallEnemyCount;
    int bigEnemyCount;


    void Start ()
    {
        scoreText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        score = 0;
        wave = 0;
    }

    void Update()
    {
        scoreText.GetComponent<Text>().text = "Score: " + score;

        // Checks if enemies are still left.
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
            WaveCount();
    }

    // Wave counter.
    void WaveCount()
    {
        wave++;

        waveText.GetComponent<Text>().text = "Wave: " + wave;

        if (wave < 7)
        {
            bigEnemyCount = 0;
        }

        if (wave == 1)
        {
            smallEnemyCount = Random.Range(2, 4);
        }

        else if (wave == 2)
        {
            smallEnemyCount = Random.Range(3, 6);
        }

        else if (wave == 3)
        {
            smallEnemyCount = Random.Range(5, 9);
        }

        else if (wave == 4)
        {
            smallEnemyCount = Random.Range(6, 9);
        }

        else if (wave == 5)
        {
            smallEnemyCount = Random.Range(7, 9);
        }

        else if (wave == 6)
        {
            smallEnemyCount = 8;
        }

        else if (wave == 7)
        {
            smallEnemyCount = Random.Range(5, 9);
            bigEnemyCount = 1;
        }

        else if (wave == 8)
        {
            smallEnemyCount = Random.Range(3, 6);
            bigEnemyCount = 2;
        }

        else if (wave == 9)
        {
            smallEnemyCount = Random.Range(3, 6);
            bigEnemyCount = Random.Range(1, 4);
        }

        else if (wave == 10)
        {
            smallEnemyCount = Random.Range(5, 9);
            bigEnemyCount = Random.Range(1, 4);
        }

        else if (wave == 11)
        {
            smallEnemyCount = Random.Range(6, 9);
            bigEnemyCount = Random.Range(1, 4);
        }

        else if (wave == 12)
        {
            smallEnemyCount = Random.Range(7, 9);
            bigEnemyCount = Random.Range(2, 4);
        }

        else if (wave == 13)
        {
            smallEnemyCount = Random.Range(8, 11);
            bigEnemyCount = Random.Range(2, 4);
        }

        else if (wave == 14)
        {
            smallEnemyCount = Random.Range(8, 11);
            bigEnemyCount = Random.Range(3, 5);
        }

        else if (wave == 15)
        {
            smallEnemyCount = Random.Range(9, 11);
            bigEnemyCount = 4;
        }

        else
        {
            smallEnemyCount = 10;
            bigEnemyCount = 5;
        }

        
        Spawn(smallEnemyCount, bigEnemyCount);
    }

    // Enemy Spawnwer.
    void Spawn(int _smallEnemies, int _bigEnemies)
    {
        // Small enemy spawner.
        for (int i = _smallEnemies; i > 0; i--)
        {
            int rndSide = Random.Range(0, 2);
            float rndPosY = Random.Range(-1.3f, -0.6f);
            float rndPosXLeft = Random.Range(-8, -5.5f);
            float rndPosXRight = Random.Range(8, 5.5f);

            if (rndSide == 1)
            {
                Vector2 spawnPosition = new Vector2(rndPosXRight, rndPosY);
                Instantiate(enemies.smallEnemyRight, spawnPosition, rightSpawnPoint.transform.rotation);
            }

            else
            {
                Vector2 spawnPosition = new Vector2(rndPosXLeft, rndPosY);
                Instantiate(enemies.smallEnemyLeft, spawnPosition, leftSpawnPoint.transform.rotation);
            }
        }

        // Big enemy spawnwer.
        for (int i = _bigEnemies; i > 0; i--)
        {
            int rndSide = Random.Range(0, 2);
            float rndPosY = Random.Range(-1.3f, -0.6f);
            float rndPosXLeft = Random.Range(-10, -5.5f);
            float rndPosXRight = Random.Range(10, 5.5f);

            if (rndSide == 1)
            {
                Vector2 spawnPosition = new Vector2(rndPosXRight, rndPosY);
                Instantiate(enemies.bigEnemyRight, spawnPosition, rightSpawnPoint.transform.rotation);
            }

            else
            {
                Vector2 spawnPosition = new Vector2(rndPosXLeft, rndPosY);
                Instantiate(enemies.bigEnemyLeft, spawnPosition, leftSpawnPoint.transform.rotation);
            }
        }
    } 
}

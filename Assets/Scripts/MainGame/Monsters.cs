using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public int initialSpawn = 5;
    public Vector3 spawnArea = new Vector3(10, 0, 10);
    public float spawnHeight = 0f;
    public float spawnInterval = 5f;

    GameObject[] monsterPrefabs;
    float timer;

    void Start()
    {
        monsterPrefabs = Resources.LoadAll<GameObject>("monsters");

        if (monsterPrefabs.Length == 0)
        {
            Debug.LogError("No monsters in Resources/monsters!");
            enabled = false;
            return;
        }

        for (int i = 0; i < initialSpawn; i++)
            SpawnMonster();

        timer = spawnInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnMonster();
            timer = spawnInterval;
        }
    }

    void SpawnMonster()
    {
        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

        Vector3 pos = transform.position + new Vector3(
            Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f),
            spawnHeight,
            Random.Range(-spawnArea.z / 2f, spawnArea.z / 2f)
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
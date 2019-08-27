using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadialSpawner : MonoBehaviour
{
    [Header("Spawner Properties")]
    [SerializeField] private List<GameObject> spawnPrefabs;

    [SerializeField] private Transform initialTarget;
    [SerializeField] private float radius;

    [Space(10)]
    [Header("Debug")]
    [SerializeField] private bool showDebug;
    [SerializeField] private Color radiusColor;

    private float time;
    private float spawnPercSec;
    private bool canSpawn;
    [SerializeField] private int boatLimit;

    private List<GameObject> spawnedEnemies;

    public List<GameObject> walkingEnemies;

    // Start is called before the first frame update
    void Start()
    {
        spawnedEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            spawnedEnemies.RemoveAll(x => x == null);

            time += Time.deltaTime / 30;
            time = Mathf.Clamp(time, 0, 1.3f);

            spawnPercSec = Mathf.Pow((time + 5), 2 * time);
            spawnPercSec = Mathf.Clamp(spawnPercSec, 5, 10);
        }
        else
        {
            time = spawnPercSec = 0;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameObject spawned = Spawn();
        //    spawned.GetComponent<BoatController>().SetTarget(initialTarget);
        //}
    }

    public void StartSpawning()
    {
        if (canSpawn) return;

        canSpawn = true;
        StartCoroutine(Spawn_C());
    }

    IEnumerator Spawn_C()
    {
        while (true)
        {
            if (spawnedEnemies.Count < boatLimit)
            {
                yield return new WaitForSeconds(1);
                for (int i = 0; i < Mathf.CeilToInt(spawnPercSec); i++)
                {
                    Spawn().GetComponent<BoatController>().SetTarget(initialTarget);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    Vector3 GetRandomPosition(float _radius, float height = 0)
    {
        float angle = Random.Range(0, 360);

        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, height, y);
    }

    GameObject Spawn()
    {
        GameObject GetRandomPrefab() => spawnPrefabs.Count > 0 ? spawnPrefabs[Random.Range(0, spawnPrefabs.Count)] : null;

        GameObject prefab = GetRandomPrefab();
        GameObject instance = Instantiate(prefab);

        Vector3 randomPos = GetRandomPosition(radius, transform.position.y);

        instance.SetPosition(randomPos);
        instance.transform.parent = transform;

        spawnedEnemies.Add(instance);

        return instance;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebug) return;
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void StopSpawn()
    {
        canSpawn = false;
        StopAllCoroutines();
    }

    public void CleanUpScene()
    {
        List<GameObject> enemiesToClear = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        List<GameObject> boatsToClear = GameObject.FindGameObjectsWithTag("Enemy").ToList();

        enemiesToClear.ForEach(Destroy);
        boatsToClear.ForEach(Destroy);
    }
}

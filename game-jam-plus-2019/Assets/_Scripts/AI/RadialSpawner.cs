using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spawned =Spawn();
            spawned.GetComponent<BoatController>().SetTarget(initialTarget);
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

        return instance;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebug) return;
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}

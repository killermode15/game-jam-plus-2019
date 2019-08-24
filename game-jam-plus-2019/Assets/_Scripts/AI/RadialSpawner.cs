using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpawner : MonoBehaviour
{
    [Header("Spawner Properties")]
    [SerializeField] private List<GameObject> SpawnPrefabs;
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
            Spawn();
    }

    Vector3 GetRandomPosition(float _radius, float height = 0)
    {
        float angle = Random.Range(0, 360);

        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, height, y);

    }


    void Spawn()
    {
        GameObject GetRandomPrefab() => SpawnPrefabs.Count > 0 ? SpawnPrefabs[Random.Range(0, SpawnPrefabs.Count)] : null;

        GameObject prefab = GetRandomPrefab();
        GameObject instance = Instantiate(prefab);

        Vector3 randomPos = GetRandomPosition(radius, transform.position.y);

        instance.SetPosition(randomPos);
        instance.transform.parent = transform;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebug) return;
        Gizmos.color = radiusColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatController : MonoBehaviour
{
    private RadialSpawner radialSpawner;

    [SerializeField] private float speed;
    [SerializeField] private GameObject enemyPrefab;
    private Transform target;

    private void Start()
    {
        radialSpawner = GameObject.FindObjectOfType<RadialSpawner>();
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (!target) return;
        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        transform.LookAt(targetPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetType() == typeof(TerrainCollider))
        {
            Debug.Log("Test");
            Unload();
        }
    }

    private void Unload()
    {
        // Spawn person
        GameObject enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        radialSpawner.walkingEnemies.Add(enemy);

        Destroy(gameObject);
    }


}

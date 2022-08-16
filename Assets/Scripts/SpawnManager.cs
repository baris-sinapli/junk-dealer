using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private float Radius = 1;
    [SerializeField] private float SpawnLimit = 3;

    private PlatformManager _platformManager;

    private void Awake()
    {
        _platformManager = gameObject.GetComponentInParent<PlatformManager>();
    }

    void Update()
    {
        if (_platformManager.spawnPoints.Count < SpawnLimit && Input.GetMouseButtonDown(1))
            SpawnRandomObjectAtRandomPoint();
    }

    private void SpawnRandomObjectAtRandomPoint()
    {
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * Radius;
        Vector3 position = new Vector3(randomPosition.x, transform.position.y, randomPosition.y);
        Instantiate(ItemPrefab, position, Quaternion.identity);
        _platformManager.spawnPoints.Add(position);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}

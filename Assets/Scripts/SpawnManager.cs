using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private float Radius = 1;
    [SerializeField] private float SpawnLimit = 3;
    
    private ulong lastSpawnTime;
    private string logPlatformName;
    public float msToWait = 1800000;

    public List<GameObject> spawnPoints;
    private Junk[] junkList;

    private void Awake()
    {
        junkList = transform.parent.GetComponentInParent<PlatformManager>().collectableJunks;
        logPlatformName = transform.parent.parent.name + ".SpawnTime";
    }

    private void Start()
    {
        lastSpawnTime = ulong.Parse(PlayerPrefs.GetString(logPlatformName));

    }

    void Update()
    {
        spawnPoints.RemoveAll(x => !x); // List update after Destroying elements (avoiding null reference)

        if (spawnPoints.Count < SpawnLimit && isSpawnReady())
        {
            SpawnRandomObjectAtRandomPoint();
        }
            
    }

    private void SpawnRandomObjectAtRandomPoint()
    {
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * Radius;
        Vector3 position = new Vector3(randomPosition.x + transform.parent.parent.position.x, transform.position.y, randomPosition.y + transform.parent.parent.position.z);
        var newParticle = Instantiate(ItemPrefab, position, Quaternion.identity);
        int randomNum = UnityEngine.Random.Range(0, junkList.Length);

        newParticle.transform.parent = gameObject.transform;
        newParticle.GetComponent<JunkContent>().junkContent = junkList[randomNum];
        
        spawnPoints.Add(newParticle);
        lastSpawnTime = (ulong)DateTime.Now.Ticks;

    }
    public void LastSpawnTime(ulong spawnTime)
    {
        lastSpawnTime = spawnTime;
        PlayerPrefs.SetString(logPlatformName, lastSpawnTime.ToString());
    }

    public bool isSpawnReady()
    {
        ulong diff = ((ulong)DateTime.Now.Ticks - lastSpawnTime);
        ulong ms = diff / TimeSpan.TicksPerMillisecond;

        float secondsLeft = (msToWait - ms) / 1000;

        return (secondsLeft < 0) ? true : false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}

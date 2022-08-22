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
        ReloadSpawnPoints();
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
        newParticle.transform.name = spawnPoints.Count.ToString();
        SaveSpawnPoint(position, randomNum);
        

        lastSpawnTime = (ulong)DateTime.Now.Ticks;

    }

    public void SaveSpawnPoint(Vector3 position, int junkNum)
    {
        PlayerPrefs.SetInt("SpawnPointCount", spawnPoints.Count); // Save count of spawn points
        VectorSave.SetVector3(transform.parent.parent.name + ".SP" + spawnPoints.Count.ToString(), position); // Save the position vector as playerprefs
        PlayerPrefs.SetInt("JunkContent.SP" + spawnPoints.Count.ToString(), junkNum); // Save the junk content of spawn point
    }

    private void ReloadSpawnPoints()
    {
        int SPCount = PlayerPrefs.GetInt("SpawnPointCount", spawnPoints.Count); // Load count of spawn points
        
        for(int i = 0; i < SPCount; i++)
        {
            if(PlayerPrefs.HasKey(transform.parent.parent.name + ".SP" + (i+1).ToString()))
            {
                // Instantiate - reload the old particle by saved location vector
                Vector3 positionVector = VectorSave.GetVector3(transform.parent.parent.name + ".SP" + (i + 1).ToString());
                var oldParticle = Instantiate(ItemPrefab, positionVector, Quaternion.identity);
                // Reassign the junk content to reloaded particle
                int junkNum = PlayerPrefs.GetInt("JunkContent.SP" + (i + 1).ToString());
                oldParticle.GetComponent<JunkContent>().junkContent = junkList[junkNum];
                // Assign parent object and add to spawnpoints list
                oldParticle.transform.parent = gameObject.transform;
                spawnPoints.Add(oldParticle);
            }

        }
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

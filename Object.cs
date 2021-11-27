using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class Object : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;

    [SerializeField]
    private List<GameObject> spawnPoints = new List<GameObject>();

    [SerializeField] [Range(0, 100)]
    private int spawnRate;

    [SerializeField]
    private bool allowRotation;

    [SerializeField]
    private bool InstantiateOnStart;

    private int spawnedObjects = 0;

    [SerializeField]
    private int minimumObjects;
    [SerializeField]
    private int maximumObjects;

    private void Start()
    {
        if(InstantiateOnStart)
        {
            GenerateAll();
        }
    }

    public void GenerateAll()
    {
        ClearObjects();

        SpawnMinimumAmount();

        GenerateRandomObjects();

        NavMeshBuilder.BuildNavMesh(); ///Generates long loading time
    }

    private void SpawnMinimumAmount()
    {
        List<int> prevSp = new List<int>();
        int tempSpawnRate = spawnRate;

        int i = 0;
        while(i < minimumObjects)
        {
            spawnRate = 100;
            int rndSp = Random.Range(0, spawnPoints.Count);
            if (prevSp.Contains(rndSp))
            {
                continue;
            }
            else
            {
                CreateObject(spawnPoints[rndSp]);
                prevSp.Add(rndSp);
                i++;
            }
        }
        spawnRate = tempSpawnRate;
    }
    public void AddSpawnpoint()
    {
        GameObject sp = new GameObject("Spawnpoint");
        sp.transform.SetParent(this.transform);
        sp.transform.localPosition = Vector3.zero;
        spawnPoints.Add(sp);
    }

    public void GenerateRandomObjects()
    {
        foreach(GameObject sp in spawnPoints)
        {
            CreateObject(sp);
        }
    }

    private void CreateObject(GameObject _spawnPoint)
    {
        if(maximumObjects < minimumObjects)
        {
            maximumObjects = minimumObjects;
            Debug.Log("Changed maximumObjects count to " + minimumObjects);
        }
        else if(maximumObjects <= 0)
        {
            maximumObjects = spawnPoints.Count;
            Debug.Log("Changed maximumObjects count to " + maximumObjects);
        }

        if(spawnedObjects < maximumObjects)
        {
            if (_spawnPoint.transform.childCount == 0)
            {
                int rnd = Random.Range(0, 101);
                if (rnd < spawnRate)
                {
                    int rndObj = Random.Range(0, objects.Length);
                    GameObject obj = Instantiate(objects[rndObj], _spawnPoint.transform.position, _spawnPoint.transform.rotation);
                    obj.transform.SetParent(_spawnPoint.transform);

                    if (allowRotation)
                        obj.transform.localRotation = Quaternion.Euler(new Vector3(objects[rndObj].transform.eulerAngles.x, Random.Range(0f, 360f), objects[rndObj].transform.eulerAngles.z));
                    spawnedObjects++;
                }
            }
        }
    }

    public void ClearObjects()
    {
        spawnedObjects = 0;
        foreach(GameObject spawnPoint in spawnPoints)
        {
            foreach(Transform child in spawnPoint.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    public void ClearSpawnPoints()
    {
        foreach(GameObject spawnPoint in spawnPoints)
        {
            DestroyImmediate(spawnPoint);
        }

        spawnPoints.Clear();
    }

    public void DeleteLastSpawnPoint()
    {
        if(spawnPoints.Count > 0)
        {
            DestroyImmediate(spawnPoints[spawnPoints.Count - 1].gameObject);
            spawnPoints.RemoveAt(spawnPoints.Count - 1);
        }
    }

    private void OnDrawGizmos()
    {
        BoxCollider colOfObj = objects[0].GetComponent<BoxCollider>();
        Gizmos.color = Color.green;
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i] != null)
                if (allowRotation)
                    Gizmos.DrawWireSphere(spawnPoints[i].transform.position, (Mathf.Max(colOfObj.size.x, colOfObj.size.z) / 2) + 1);
                else
                    Gizmos.DrawWireCube(spawnPoints[i].transform.position, colOfObj.size);

        }
    }
}

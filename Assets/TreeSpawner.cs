using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject treePrefab0;
    // [SerializeField] GameObject treePrefab0;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] int count = 3;

    private void Start()
    {
        List<Vector3> emptyPos = new List<Vector3>();

        GameObject[] ImTree = new GameObject[]
        {
            treePrefab, treePrefab0
        };

        int Indosat = Random.Range (0, 2);


        for (int x = - terrain.Extent; x <= terrain.Extent; x++)
        {
            if(transform.position.z == 0 && x == 0)
                continue;

            emptyPos.Add(transform.position + Vector3.right * x);
        }

        for (int i = 0; i < count; i++)
        {
            var index = Random.Range(0,emptyPos.Count-1);
            var spawnPos = emptyPos[index];
            Instantiate(ImTree[Indosat],spawnPos,Quaternion.identity,this.transform);
            emptyPos.RemoveAt(index);
        }

        Instantiate(ImTree[Indosat],transform.position + Vector3.right *-(terrain.Extent+1),Quaternion.identity,this.transform);
        Instantiate(ImTree[Indosat],transform.position + Vector3.right *(terrain.Extent+1),Quaternion.identity,this.transform);
    }
}

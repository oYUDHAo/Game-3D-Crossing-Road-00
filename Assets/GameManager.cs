using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
  [SerializeField] GameObject gameOver;
  [SerializeField] Player player;
  [SerializeField] GameObject road;
  [SerializeField] GameObject grass;
  [SerializeField] int extent = 7;
  [SerializeField] int frontDistance = 12;
  [SerializeField] int backDistance = -4;
  [SerializeField] int maxSameTerrainRepeat = 2;

 // int maxZpos;

  Dictionary<int,TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
  TMP_Text gameOverText;
  private int playerLastMaxTravel;

  private void Start()
  {
    gameOver.SetActive(false);
    gameOverText = gameOver.GetComponentInChildren<TMP_Text>();

    for (int z = backDistance; z <= 0; z++)
    {
        CreateTerrain(grass, z);
    }
    
    for (int z = 1; z <= frontDistance; z++)
    {
        var prefab = GetNextRandomTerrainPrefab(z);

        CreateTerrain(prefab, z);
    }
    player.SetUp(backDistance, extent);
  }

  private void Update()
  {
      if(player.IsDie && gameOver.activeInHierarchy== false)
        StartCoroutine(ShowGameOver());
      
      if(player.MaxTravel==playerLastMaxTravel)
        return;
      
      playerLastMaxTravel = player.MaxTravel;

      // Create Front
      var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel+frontDistance);
      CreateTerrain(randTbPrefab,player.MaxTravel+frontDistance);

      // delete back
      var lastTB = map[player.MaxTravel-1 + backDistance];
      // TerraiBlock lastTB = map[player.MaxTravel+frontDistance];
      // int lastPos = player.MaxTravel;
      // foreach (var(pos, tb) in map)
      // {
      //   if(pos<lastPos)
      //   {
      //     lastPos = pos;
      //     lastTB = tb;
      //   }
      // }

      map.Remove(player.MaxTravel-1 +backDistance);
      Destroy(lastTB.gameObject);

      player.SetUp(player.MaxTravel + backDistance, extent);
  }

  IEnumerator ShowGameOver()
  {
    yield return new WaitForSeconds(4);
    gameOverText.text = "Your Score : "+ player.MaxTravel;
    gameOver.SetActive(true);
  }

  private void CreateTerrain(GameObject prefab, int zPos)
  {
        var go = Instantiate(prefab,new Vector3(0,0,zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos,tb);
        // Debug.Log(map[zPos] is Road);
  }

  private GameObject GetNextRandomTerrainPrefab(int nextPos)
  {
    bool isUniform = true;
    var tbRef = map[nextPos-1];
    for(int distance = 2; distance <= maxSameTerrainRepeat; distance++)
    {
        if(map[nextPos-distance].GetType() !=tbRef.GetType())
        {
            isUniform = false;
            break;
        }
            
    }

    if (isUniform)
    {
        if (tbRef is Grass)
            return road;
        else
            return grass;
    }
    return Random.value > 0.5f ? road : grass;
  }
}

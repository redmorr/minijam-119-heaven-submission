using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float InitalSpawnEveryXSecond;
    public float FinalSpawnEveryXSecond;
    public float TimeToMax;
    public bool Spawn = true;
    private Tilemap tilemap;
    private List<Vector3> tileWorldLocations = new List<Vector3>();

    private float timer = 0f;

    public void Stop()
    {
        Spawn = false;
    }

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = tilemap.CellToWorld(localPlace);
            if (tilemap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
                //tilemap.SetTile(localPlace, null);
            }
        }
    }

    private void Update()
    {
        if (Spawn)
        {
            if (Mathf.Lerp(InitalSpawnEveryXSecond, FinalSpawnEveryXSecond, Time.timeSinceLevelLoad/TimeToMax) < timer)
            {
                Instantiate(enemyPrefab, tileWorldLocations[Random.Range(0, tileWorldLocations.Count - 1)], Quaternion.identity);
                timer = 0f;
            }
            timer += Time.deltaTime;
        }
    }

}

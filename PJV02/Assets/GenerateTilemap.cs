using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileProbabilityDictionary : SerializableDictionary<RandomTile, float> { };

public class GenerateTilemap : MonoBehaviour
{
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap decorationTilemap;
    [SerializeField] TileProbabilityDictionary decorationTilesWithProbability;
    [SerializeField] TileBase tile;
    [SerializeField] Vector3Int origin = new Vector3Int(0, 0, 0);
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    [SerializeField] int minHeight = 1;
    [SerializeField] int minSectionWidth = 1;
    [SerializeField] int seed = 100;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random(seed.GetHashCode());

        var map = GenerateArray(width, height, true);
        map = RandomWalkTopSmoothed(map);
        RenderMap(map);
    }

    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public void RenderMap(int[,] map)
    {
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            int y = 0;
            for (y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    groundTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0 + origin.z), tile);
                }
                else
                {
                    break;
                }
            }
            // Got to the top of the platform, add decorations
            foreach (var tile in decorationTilesWithProbability.Keys)
            {
                var p = decorationTilesWithProbability[tile];
                if (RandomBool(p))
                {
                    decorationTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0 + origin.z), tile);
                    break;
                }
            }
        }
    }

    public bool RandomBool(float probability)
    {
        return random.Next(100) <= probability * 100;
    }

    public int[,] RandomWalkTopSmoothed(int[,] map)
    {
        //Determine the start position
        int lastHeight = minHeight - 1;

        //Used to determine which direction to go
        int nextMove = 0;
        //Used to keep track of the current sections width
        int sectionWidth = 0;

        //Work through the array width
        for (int x = 0; x <= map.GetUpperBound(0); x++)
        {
            //Determine the next move
            nextMove = random.Next(3);

            //Only change the height if we have used the current height more than the minimum required section width
            if (nextMove == 0 && lastHeight >= minHeight && sectionWidth > minSectionWidth)
            {
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                lastHeight++;
                sectionWidth = 0;
            }
            //Increment the section width
            sectionWidth++;

            //Work our way from the height down to 0
            for (int y = lastHeight; y >= 0; y--)
            {
                map[x, y] = 1;
            }
        }

        //Return the modified map
        return map;
    }
}

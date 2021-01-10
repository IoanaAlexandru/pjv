using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileProbabilityDictionary : SerializableDictionary<RandomTile, float> { };

[Serializable]
public class ObjectProbabilityDictionary : SerializableDictionary<GameObject, float> { };

public class GenerateTilemap : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap decorationTilemap;
    public Tilemap collectibleTilemap;
    public TileProbabilityDictionary decorationTilesWithProbability;
    public ObjectProbabilityDictionary enemiesWithProbability;
    public TileBase tile;
    public PrefabTile collectibleTile;
    public float collectibleProbability;
    public Vector3Int origin = new Vector3Int(0, 0, 0);
    public int width = 10;
    public int height = 10;
    public int minHeight = 1;
    public int minSectionWidth = 1;
    public int seed = 100;

    System.Random random;

    List<Section> sections;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random(seed.GetHashCode());

        var map = GenerateArray(width, height, true);
        List<Section> sections;
        (map, sections) = RandomWalkTopSmoothed(map);

        RenderMap(map, sections);
        this.sections = sections;
    }

    private void Update()
    {
        return;
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

    public void RenderMap(int[,] map, List<Section> sections)
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
            // Add collectibles
            if (RandomBool(collectibleProbability))
            {
                collectibleTilemap.SetTile(new Vector3Int(x + origin.x, y + origin.y, 0 + origin.z), collectibleTile);
            }
        }

        // Generate enemies
        foreach (var section in sections)
        {
            foreach (var enemy in enemiesWithProbability.Keys)
            {
                var p = enemiesWithProbability[enemy];
                if (RandomBool(p))
                {
                    var newEnemy = Instantiate(enemy, new Vector3((section.center.x + origin.x) * groundTilemap.cellSize.x, (section.center.y + origin.y + 2) * groundTilemap.cellSize.y, 9), Quaternion.identity);

                    float leftLimit = (section.center.x + origin.x - section.width / 2 + 0.5f) * groundTilemap.cellSize.x;
                    float rightLimit = (section.center.x + origin.x + section.width / 2 - 0.5f) * groundTilemap.cellSize.x;

                    var controller = newEnemy.GetComponent<ZombieController>();
                    controller.leftLimit = leftLimit;
                    controller.rightLimit = rightLimit;
                    controller.movingLeft = RandomBool(0.5f);
                    
                    break;
                }
            }
        }
    }

    public bool RandomBool(float probability)
    {
        return random.Next(100) <= probability * 100;
    }

    public (int[,] map, List<Section> sectionCenters) RandomWalkTopSmoothed(int[,] map)
    {
        var sectionCenters = new List<Section>();

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
                sectionCenters.Add(new Section(new Vector2(x - sectionWidth / 2, lastHeight), sectionWidth));
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < map.GetUpperBound(1) && sectionWidth > minSectionWidth)
            {
                sectionCenters.Add(new Section(new Vector2(x - sectionWidth / 2, lastHeight), sectionWidth));
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
        return (map, sectionCenters);
    }
}

public struct Section
{
    public  Vector2 center;
    public int width;

    public Section(Vector2 center, int width) {
        this.center = center;
        this.width = width;
    }
}

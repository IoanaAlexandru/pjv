using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTilemap : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase tile;
    [SerializeField] Vector3 origin = new Vector3(0, 0, 0);
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    [SerializeField] int minHeight = 1;
    [SerializeField] int minSectionWidth = 1;

    // Start is called before the first frame update
    void Start()
    {
        var map = GenerateArray(width, height, true);
        map = RandomWalkTopSmoothed(map, 100);
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
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x + (int)origin.x, y + (int)origin.y, 0 + (int)origin.z), tile);
                }
            }
        }
    }

    public int[,] RandomWalkTopSmoothed(int[,] map, float seed)
    {
        //Seed our random
        System.Random rand = new System.Random(seed.GetHashCode());

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
            nextMove = rand.Next(3);

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

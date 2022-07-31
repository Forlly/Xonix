using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase wayTile;

    public static GridController Instance;
    public int[,] map;


    private void Awake()
    {
        Instance = this;
    }

    public void GeneratePlayingField()
    {

        map = new int[1920 / 50, 1080 / 50];
        for (int i = (-(1920 / 100) + 1); i < (1920 / 100 - 1); i++) //разрешение экрана / cellSize * pixel per Unit тайла
        {
            for (int j = -(1080 / 100); j < 1080/100 ; j++)
            {
                map[i + 1920/100 , j + 1080/100] = 0;

                tilemap.SetTile(new Vector3Int(i,j,0), grassTile);
                if (i == 1920/100 - 2)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                    tilemap.SetTile(new Vector3Int(1920/100 - 2,j,0), waterTile);
                }
                else if (i == -(1920 / 100) + 1)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                    tilemap.SetTile(new Vector3Int(-(1920 /100) + 1 , j,0), waterTile);
                }

                if (j == -(1080 / 100))
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                    tilemap.SetTile(new Vector3Int(i,-(1080 / 100),0), waterTile);
                }
                else if (j == 1080/100 - 1)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                    tilemap.SetTile(new Vector3Int(i,1080/100 - 1,0), waterTile);
                }
                
            }
        }

        for (int i = 0; i < map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < map.GetUpperBound(1); j++)
            {
                Debug.Log($"{map[i,j]}  ");
            }
            Debug.Log("\n");
                
        }
    }

    public void UpdateMap(int[,] _map)
    {
        map = _map;
        
        for (int i = (-(1920 / 100) + 1); i < (1920 / 100 - 1); i++) //разрешение экрана / cellSize * pixel per Unit тайла
        {
            for (int j = -(1080 / 100); j < 1080/100 ; j++)
            {

                if ( map[i + 1920/100 , j + 1080/100] == 1)
                {
                    tilemap.SetTile(new Vector3Int(i  , j ,0), null);   
                    tilemap.SetTile(new Vector3Int(i  , j,0), waterTile);
                }
              
            }
        }
    }


    public void SetTileWay(int x, int y)
    {
        Debug.Log("WAYTILE");
        tilemap.SetTile(new Vector3Int(x,y,0), null);
        tilemap.SetTile(new Vector3Int(x,y,0), wayTile);
    }
    
    public void SetTileWater(int x, int y)
    {
        Debug.Log("Water");
        tilemap.SetTile(new Vector3Int(x,y,0), null);
        tilemap.SetTile(new Vector3Int(x,y,0), waterTile);
    }
}

using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase wayTile;

    public static GridController Instance;
    public int[,] map;

    public float shadedArea = 0;
    public int currentLevel = 1;
    [SerializeField] private Text shadedAreaText;
    [SerializeField] private Text currentLevelText;

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
                if (i == 1920/100 - 2 || i == 1920/100 - 3)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                }
                else if (i == -(1920 / 100) + 1 || i == -(1920 / 100) + 2)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                }

                if (j == -(1080 / 100) || j == -(1080 / 100) + 1)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                }
                else if (j == 1080/100 - 1 || j == 1080/100 - 2)
                {
                    map[i + 1920/100 , j + 1080/100] = 1;
                }
                
            }
        }
        
        UpdateMap(map);
        
    }

    public void UpdateMap(int[,] _map)
    {
        map = _map;
        
        for (int i = (-(1920 / 100) + 1); i < (1920 / 100 - 1); i++) //разрешение экрана / cellSize * pixel per Unit тайла
        {
            for (int j = -(1080 / 100); j < 1080/100 ; j++)
            {

                if ( map[i + 1920/100 , j + 1080/100] == 1 || map[i + 1920/100 , j + 1080/100] == 2)
                {
                    map[i + 1920 / 100, j + 1080 / 100] = 1;
                    tilemap.SetTile(new Vector3Int(i  , j ,0), null);   
                    tilemap.SetTile(new Vector3Int(i  , j,0), waterTile);
                }
              
            }
        }
        
        shadedArea = CalculateShadedArea(_map);
        shadedAreaText.text = $"{Math.Round(shadedArea, 1)}%";

        if (shadedArea >= 75f)
        {
            ClearPlayingField(_map);
        }
    }


    public void ThrowWayTruck(int[,] _map)
    {
        for (int i = (-(1920 / 100) + 1); i < (1920 / 100 - 1); i++) //разрешение экрана / cellSize * pixel per Unit тайла
        {
            for (int j = -(1080 / 100); j < 1080/100 ; j++)
            {

                if (map[i + 1920/100 , j + 1080/100] == 2)
                {
                    map[i + 1920 / 100, j + 1080 / 100] = 0;
                    tilemap.SetTile(new Vector3Int(i  , j ,0), null);   
                    tilemap.SetTile(new Vector3Int(i  , j,0), grassTile);
                }
              
            }
        }
    }
    public void ClearPlayingField(int[,] _map)
    {
        tilemap.ClearAllTiles();
        EnemysControls.Instance.ClearEnemys();
        GeneratePlayingField();
        
        currentLevel++;
        EnemysControls.Instance.GenerateEnemysOnMap(map);
        TimerOfLevel.Instance.levelComplete = true;
        
        shadedAreaText.text = "0%";
        currentLevelText.text =  $"Lvl: {currentLevel}";
        CharacterController.Instance.map = map;
    }
    

    public float CalculateShadedArea(int[,] _map)
    {
        float shadedArea = 0;
        float allArea = 1;
        float percentShadedArea;
        
        for (int i = 0; i < _map.GetUpperBound(0); i++)
        {
            for (int j = 0; j < _map.GetUpperBound(1); j++)
            {
                allArea++;
                if (_map[i, j] == 1)
                    shadedArea++;
            }
        }

        percentShadedArea = (shadedArea / allArea) * 100f;
        return percentShadedArea;
    }

    public void SetTileWay(int x, int y)
    {
        tilemap.SetTile(new Vector3Int(x,y,0), null);
        tilemap.SetTile(new Vector3Int(x,y,0), wayTile);
    }
    
    public void PaintArea(int[,] _map, int x, int y)
    {
        if (x == 0 || x == _map.GetUpperBound(0) - 1 || y == _map.GetUpperBound(1) - 1 || y == 0 )
        {
            return;
        }
        if (_map[x, y] == 0)
            _map[x, y] = 1;

        if (_map[x, y - 1] == 0)
        {
            PaintArea(_map, x, y - 1);
        }
        
        if (_map[x,y + 1] == 0)
        {
            PaintArea(_map, x, y + 1);
        }
        
        if (_map[x - 1,y] == 0)
        {
            PaintArea(_map,x - 1, y);
        }
        
        if (_map[x + 1,y] == 0)
        {
            PaintArea(_map,x + 1, y);
        }
    }
    
}

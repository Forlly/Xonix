using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemysControls : MonoBehaviour
{
    public List<EnemyController> enemys;
    public static EnemysControls Instance;

    public EnemyController waterEnemy;
    public EnemyController grassEnemy;

    private int countOfWaterEnemys = 0;
    private int countOfGrassEnemys = 1;
    
    private Vector3 minPos;
    private Vector3 maxPos;
    
    [SerializeField] private GameObject gameOver;
    private void Awake()
    {
        Instance = this;
        minPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));   //Получаем вектор нижнего левого угла камеры
        maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
    }

    public void DeleteEnemy(EnemyController enemyController)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == enemyController)
            {
                Destroy(enemyController.gameObject);
                enemys.Remove(enemys[i]);
            }
        }

        if (enemys.Count == 0)
        {
            GridController.Instance.ClearPlayingField(CharacterController.Instance.map);
        }
    }
    public void ClearEnemys()
    {
        EnemyController tmp;
        for (int i = 0; i < enemys.Count; i++)
        {
            tmp = enemys[i];
            Destroy(tmp.gameObject);
            
        }
        enemys.Clear();
    }
    public void GenerateEnemysOnMap(int[,] map)
    {
        
        countOfGrassEnemys = GridController.Instance.currentLevel;
        if (GridController.Instance.currentLevel % 5 == 0)
        {
            countOfWaterEnemys++;
        }
        Debug.Log(countOfGrassEnemys);
        Debug.Log(countOfWaterEnemys);
        for (int i = 0; i < countOfWaterEnemys; i++)
        {
            Vector2 rand = new Vector2(Random.Range(1, map.GetUpperBound(0) - 1),
                Random.Range(1, map.GetUpperBound(1) - 1));
            while (map[(int)rand.x, (int)rand.y] != 1)
            {
                rand = new Vector2(Random.Range(1, map.GetUpperBound(0) - 1),
                    Random.Range(1, map.GetUpperBound(1) - 1));
            }

            rand.x = ( rand.x - 1920/100 + 0.5f) / 2;
            rand.y = (rand.y - 1080/100 ) / 2;
            enemys.Add(Instantiate(waterEnemy, rand,Quaternion.identity));
        }
        
        for (int i = 0; i < countOfGrassEnemys; i++)
        {
            Vector2 rand = new Vector2(Random.Range(1, map.GetUpperBound(0) - 1),
                Random.Range(1, map.GetUpperBound(1) - 1));
            while (map[(int)rand.x, (int)rand.y] != 0)
            {
                rand = new Vector2(Random.Range(1, map.GetUpperBound(0) - 1),
                    Random.Range(1, map.GetUpperBound(1) - 1));
            }
            
            rand.x = ( rand.x - 1920/100 + 0.5f) / 2;
            rand.y = (rand.y - 1080/100 ) / 2;
            enemys.Add(Instantiate(grassEnemy, rand,Quaternion.identity));
        }
        
    }
}

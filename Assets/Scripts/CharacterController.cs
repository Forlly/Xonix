using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    public static CharacterController Instance;

    private Vector3 minPos;
    private Vector3 maxPos;
    private bool isTrace = false;
    public bool enemyHitWay = false;

    private Vector2 prevPos;
    private GridController gridController;
    public int[,] map;

    public int currentHP = 3;
    [SerializeField] private Text currentHPText;
    
    private Vector2 startWayPos;
    private Vector2 endWayPos;
    private Vector2 startWayPosMAP;
    private Vector2 endWayPosMAP;
    
    [SerializeField] private GameObject gameOver;
    

    private void Start()
    {
        Instance = this;
        minPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));   //Получаем вектор нижнего левого угла камеры
        maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));   //Получаем верхний правый угол камеры
       gridController = GridController.Instance;
       map = gridController.map;
    }

   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
            direction = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
            direction = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
            direction = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
            direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        if (transform.position.x >= maxPos.x - 0.25f)
        {
            if (direction != Vector2.left)
            {
                direction = new Vector2(0, direction.y);
            }
        }

        if (transform.position.x <= minPos.x + 0.25f )
        {
            if (direction != Vector2.right)
            {
                direction = new Vector2(0, direction.y);
            }
        }
        
        if (transform.position.y >= maxPos.y - 0.25f)
        {
            if (direction != Vector2.down)
            {
                direction = new Vector2(direction.x, 0);
            }
        }
        
        if (transform.position.y <= minPos.y + 0.7f)
        {
            if (direction != Vector2.up)
            {
                direction = new Vector2(direction.x, 0);
            }
        }

        prevPos = transform.position;
        transform.position = new Vector3(transform.position.x + direction.x/2, transform.position.y + direction.y/2,
            transform.position.z);
        

        if (isTrace)
        {
            isTrace = false;
            StartCoroutine(WayTrack());
        }

        if (map[(int)Math.Ceiling((prevPos.x + maxPos.x )*2), (int)Math.Ceiling((prevPos.y + maxPos.y - 0.25)*2)] == 1
            && map[(int)Math.Ceiling((transform.position.x + maxPos.x )*2), (int)Math.Ceiling((transform.position.y + maxPos.y - 0.25)*2)] == 0)
        {
            isTrace = true;
            gridController.SetTileWay((int) Math.Ceiling((transform.position.x - 0.25) * 2),
                (int) Math.Ceiling((transform.position.y- 0.25) * 2));

            startWayPos = new Vector2((int) Math.Ceiling((transform.position.x - 0.25) * 2),
                (int) Math.Ceiling((transform.position.y - 0.25) * 2));
            
            startWayPosMAP = new Vector2((int) Math.Ceiling((transform.position.x + maxPos.x) * 2),
                (int) Math.Ceiling((transform.position.y + maxPos.y - 0.25) * 2));
            map[(int) Math.Ceiling((transform.position.x + maxPos.x) * 2),
                (int) Math.Ceiling((transform.position.y + maxPos.y - 0.25) * 2)] = 2;
        }
        
    }


    public void LossHP()
    {
        currentHP--;
        currentHPText.text = $"HP: {currentHP}";
        if (currentHP <= 0)
        {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
    }

    private IEnumerator WayTrack()
    {
        while (map[(int)Math.Ceiling((transform.position.x + maxPos.x )*2), 
                   (int)Math.Ceiling((transform.position.y + maxPos.y - 0.25)*2)] != 1)
        {
            if (enemyHitWay)
            {
                enemyHitWay = false;
                gridController.ThrowWayTruck(map);
                LossHP();
                yield break ;
            }
            gridController.SetTileWay((int) Math.Ceiling((transform.position.x - 0.25) * 2),
                (int) Math.Ceiling((transform.position.y- 0.25) * 2));
            map[(int) Math.Ceiling((transform.position.x + maxPos.x) * 2),
                (int) Math.Ceiling((transform.position.y + maxPos.y - 0.25) * 2)] = 2;
            yield return null;
        }
        
        endWayPos = new Vector2((int) Math.Ceiling((prevPos.x - 0.25) * 2),
            (int) Math.Ceiling((prevPos.y - 0.25) * 2));
        endWayPosMAP = new Vector2((int) Math.Ceiling((prevPos.x + maxPos.x) * 2),
            (int) Math.Ceiling((prevPos.y + maxPos.y - 0.25) * 2));
        

        float startX = (int) Math.Ceiling(( Mathf.Abs(endWayPosMAP.x) - Mathf.Abs(startWayPosMAP.x)));
        float startY = (int) Math.Ceiling(( Mathf.Abs(endWayPosMAP.y) -  Mathf.Abs(startWayPosMAP.y)));
        int directionX;
        int directionY = 0;
        if (startX >=0)
        {
            directionX = 1;
        }
        else
        {
            directionX = -1;
        }

        

        if ( map[((int) (startWayPosMAP.x + directionX)), (int) (startWayPosMAP.y )] == 2)
        {
            directionX = 0;
            if (startY >0)
            {
                directionY = 1;
            }
            else
            {
                directionY = -1;
            }
        }
        
        
        if ( Mathf.Abs(startWayPosMAP.x) == Mathf.Abs(endWayPosMAP.x) //если линия прямая
             && map[((int) (startWayPosMAP.x)), (int) ((startWayPosMAP.y + endWayPosMAP.y)/2)] == 2)
        {
            if (startWayPosMAP.x <= 17)
            {
                directionX = -(directionX);
            }
        }
        else if ( Mathf.Abs(startWayPosMAP.y) == Mathf.Abs(endWayPosMAP.y) 
                  && map[((int) ((startWayPosMAP.x + endWayPosMAP.x)/2)), (int) (startWayPosMAP.y)] == 2)
        {
            if (startWayPosMAP.y > 10)
            {
                directionY = -(directionY);
            }
        }
        
        
        map[((int) (startWayPosMAP.x + directionX)), (int) (startWayPosMAP.y + directionY)] = 1;
        map[((int) (startWayPosMAP.x)), (int) (startWayPosMAP.y)] = 1;
        gridController.PaintArea( map,((int) (startWayPosMAP.x + directionX)), (int) (startWayPosMAP.y + directionY));


        for (int i = 1; i < map.GetUpperBound(0); i++)//закрашивание замкнутой путем области
        {
            for (int j = 1; j < map.GetUpperBound(1); j++)
            {
                if (map[i,j] == 0 && map[i - 1,j - 1] == 2&& map[i - 1,j] == 2 && map[i,j - 1] == 2 )
                {
                    int k = j;
                    int m = i;
                    while (map[i,k] != 2)
                    {
                        k++;
                        if (k == map.GetUpperBound(1))
                        {
                            break;
                        }
                    }
                    while (map[m,j] != 2)
                    {
                        m++;
                        if (m == map.GetUpperBound(0))
                        {
                            break;
                        }
                    }

                    if (map[i,k] == 2 && map[m,j] == 2)
                    {
                        map[i, j] = 1;
                        gridController.PaintArea(map,i,j);
                    }
                }
            }
        }
        
        gridController.UpdateMap(map);

    }
}

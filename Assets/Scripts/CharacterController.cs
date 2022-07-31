using System;
using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;

    private Vector3 minPos;
    private Vector3 maxPos;
    private bool isTrace = false;

    private Vector2 prevPos;
    private GridController gridController;
    private int[,] map;


    private Vector2 startWayPos;
    private Vector2 endWayPos;
    private Vector2 startWayPosMAP;
    private Vector2 endWayPosMAP;
    

    private void Start()
    {
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
            Debug.Log("HI");
            isTrace = true;
            gridController.SetTileWay((int) Math.Ceiling((transform.position.x - 0.25) * 2),
                (int) Math.Ceiling((transform.position.y- 0.25) * 2));

            startWayPos = new Vector2((int) Math.Ceiling((transform.position.x - 0.25) * 2),
                (int) Math.Ceiling((transform.position.y - 0.25) * 2));
            
            startWayPosMAP = new Vector2((int) Math.Ceiling((transform.position.x + maxPos.x) * 2),
                (int) Math.Ceiling((transform.position.y + maxPos.y - 0.25) * 2));
        }
        
    }



    private IEnumerator WayTrack()
    {
        while (map[(int)Math.Ceiling((transform.position.x + maxPos.x )*2), 
                   (int)Math.Ceiling((transform.position.y + maxPos.y - 0.25)*2)] != 1)
        {
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
        gridController.SetTileWater((int) Math.Ceiling(endWayPos.x),
            (int) Math.Ceiling(endWayPos.y));
        gridController.SetTileWater((int) Math.Ceiling(startWayPos.x),
            (int) Math.Ceiling(startWayPos.y));




        float startX = (int) Math.Ceiling(( Mathf.Abs(endWayPosMAP.x) - Mathf.Abs(startWayPosMAP.x)));
        float startY = (int) Math.Ceiling(( Mathf.Abs(endWayPosMAP.y) -  Mathf.Abs(startWayPosMAP.y)));
        int directionX;
        int directionY;
        float tmp;
        if (startX >=0)
        {
            directionX = 1;
        }
        else
        {
            Debug.Log("SWAP");
            tmp = endWayPosMAP.x;
            endWayPosMAP.x = startWayPosMAP.x;
            startWayPosMAP.x = tmp;
            directionX = -1;
        }
        
        if (startY >=0)
        {
            directionY = 1;
        }
        else
        {
            Debug.Log("SWAP");  
            tmp = endWayPosMAP.y;
            endWayPosMAP.y = startWayPosMAP.y;
            startWayPosMAP.y = tmp;
            directionY = -1;
        }
        
        
        Debug.Log("START END");
        Debug.Log(startWayPosMAP);
        Debug.Log(endWayPosMAP);
        Debug.Log(startWayPos);
        Debug.Log(endWayPos);
        
        Debug.Log("New value");
        Debug.Log(map[(int)startWayPosMAP.x,(int)startWayPosMAP.y]);
        Debug.Log("X");
        Debug.Log(directionX);
        Debug.Log((int)startWayPosMAP.x);
        Debug.Log((int)(endWayPosMAP.x));
        Debug.Log("Y");
        Debug.Log(directionY);
        Debug.Log((int)startWayPosMAP.y);
        Debug.Log((int)(endWayPosMAP.y));

        for (int i = (int)(startWayPosMAP.x); i <= (int)(endWayPosMAP.x ); i++)
        {
            for (int j = (int)(startWayPosMAP.y); j <= (int)(endWayPosMAP.y); j++)
            {
                Debug.Log("Prev value");
                Debug.Log(map[i, j]);
                
                map[i, j] = 1;
                
                Debug.Log("New value");
                Debug.Log(i);
                Debug.Log(j);
            }
        }

        gridController.UpdateMap(map);

    }

   /* private IEnumerator Movement()
    {
        while (!Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.S) 
                                           && !Input.GetKeyDown(KeyCode.A) &&    !Input.GetKeyDown(KeyCode.D))
        {
            yield return null;
        }
        
        while (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) 
                                            || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.W))
                direction = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.S))
                direction = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.A))
                direction = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.D))
                direction = Vector2.right;
            
            
            
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
            
            
        }
        


        StartCoroutine(Movement());
    }

    private IEnumerator Move()
    {
        transform.position = new Vector3(transform.position.x + direction.x/2, transform.position.y + direction.y/2,
            transform.position.z);
        
        yield return new WaitForFixedUpdate();
        StartCoroutine(Move());
    }*/
}

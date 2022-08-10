using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction = Vector2.zero;
    private Vector3 minPos;
    private Vector3 maxPos;
    private GridController gridController;
    private CharacterController characterController;
    private int randomDirection;

    public bool TypeOfEnemy = false;
    
    [SerializeField] private float speed = 1;

    private Vector2[] variantsOfDirection ;
    private void Start()
    {
        gridController = GridController.Instance;
        variantsOfDirection = new []
        {
            new Vector2(1, 1), new Vector2(-1, -1), 
            new Vector2(-1, 1), new Vector2(1, -1)
        };
        
        randomDirection = Random.Range(0, 3);
        direction = variantsOfDirection[2];
        minPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));   //Получаем вектор нижнего левого угла камеры
        maxPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));   //Получаем верхний правый угол камеры
    }

    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector2(transform.position.x + direction.x, transform.position.y + direction.y), 1);

        transform.position = new Vector2(
            transform.position.x + direction.x * Time.deltaTime * speed, 
            transform.position.y + direction.y * Time.deltaTime * speed);
        

        if (Mathf.Abs(transform.position.x - CharacterController.Instance.transform.position.x) <=0.35f && 
            Mathf.Abs(transform.position.y - CharacterController.Instance.transform.position.y) <=0.35f)
        {

            CharacterController.Instance.LossHP();
        }
        
       
        if ((int)Math.Ceiling((transform.position.x + maxPos.x + direction.x + 0.75f)*2) >= gridController.map.GetUpperBound(0)
            || (int)Math.Ceiling((transform.position.x + maxPos.x + direction.x - 0.75f)*2) <= 0)
        {
            direction.x = -direction.x;
        }

        if ( (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2) >= gridController.map.GetUpperBound(1)
            || (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y + 0.25f)*2) <= 0)
        {
            direction.y = -direction.y;
        }
        
        
        else if (gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x )*2), 
                (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2)] == Convert.ToInt32(!TypeOfEnemy))
        {
            if (gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x + 0.25 )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.5f)*2)] == Convert.ToInt32(!TypeOfEnemy))
            {
                direction.y = -direction.y;
            }
            if (gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.75f)*2)] == Convert.ToInt32(!TypeOfEnemy))
            {
                direction.x = -direction.x;
            }

            if (gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x + 0.25f )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2)] == Convert.ToInt32(!TypeOfEnemy)
                && gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2)] == Convert.ToInt32(!TypeOfEnemy)
                && gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x - 0.25f )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2)] == Convert.ToInt32(!TypeOfEnemy)
                && gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x )*2), 
                    (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.5f)*2)] == Convert.ToInt32(!TypeOfEnemy))
            {
                EnemysControls.Instance.DeleteEnemy(this);
            }
        }
        else if (gridController.map[(int)Math.Ceiling((transform.position.x + maxPos.x + direction.x )*2), 
                     (int)Math.Ceiling((transform.position.y + maxPos.y + direction.y - 0.25f)*2)] == 2)
        {
            Debug.Log("HIT");
            CharacterController.Instance.enemyHitWay = true;
        }
        
        
    }
    

}

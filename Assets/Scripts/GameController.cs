using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridController grid;
    void Awake()
    {
        Application.targetFrameRate = 120;
        grid.GeneratePlayingField();
        EnemysControls.Instance.GenerateEnemysOnMap(grid.map);
    }


}

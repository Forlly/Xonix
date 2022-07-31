using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridController grid;
    void Awake()
    {
        grid.GeneratePlayingField();
    }


}

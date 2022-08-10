using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerOfLevel : MonoBehaviour
{
    [SerializeField] private Text timerText;
    public int timer = 0;
    public bool levelComplete = false;
    [SerializeField] private GameObject gameOver;
    public static TimerOfLevel Instance;
    
    private void Start()
    {
        Instance = this;
        StartCoroutine(StartTimer());

    }

    private IEnumerator StartTimer()
    {
        timer = 0;
        while (timer < 60)
        {
            if (levelComplete)
            {
                timer = 0;
                levelComplete = false;
            }
            timer++;
            timerText.text = $"Time: {timer.ToString()}";
            yield return new WaitForSeconds(1f);
        }

        Time.timeScale = 0;
        gameOver.SetActive(true);
    }


}
 

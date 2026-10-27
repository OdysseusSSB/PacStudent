using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TMP_Text timeDisplay;
    public TMP_Text ghostTimeDisplay;
    public TMP_Text scoreDisplay;

    private int score = 0;
    private float ghostUnscaredTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeDisplay.text = FormatTime(Time.timeSinceLevelLoad);
        scoreDisplay.text = FormatScore(score);
        ghostTimeDisplay.text = FormatGhostUnscaredTime(ghostUnscaredTime - Time.timeSinceLevelLoad);
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene("StartScene");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    private string FormatGhostUnscaredTime(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        if(seconds <= 0)
        {
            return "";
        }
        else
        {
            return seconds.ToString();
        }
    }
    
    private string FormatScore(int score)
    {
        return score.ToString("D6");
    }
}

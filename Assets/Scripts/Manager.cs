using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TMP_Text timeDisplay;
    public TMP_Text ghostTimeDisplay;
    public TMP_Text scoreDisplay;
    [SerializeField]
    GhostController[] ghosts;
    private int score = 0;
    private float ghostUnscaredTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Awake()
    {
        for (int i = 0; i < ghosts.Length; i++)
            Debug.Log($"Awake: ghosts[{i}] = {(ghosts[i] ? ghosts[i].name : "NULL")}", this);
    }*/

    void Start()
    {
        scoreDisplay.text = FormatScore(score);
    }

    // Update is called once per frame
    void Update()
    {
        timeDisplay.text = FormatTime(Time.timeSinceLevelLoad);
        float ghostTime = ghostUnscaredTime - Time.timeSinceLevelLoad;
        ghostTimeDisplay.text = FormatGhostUnscaredTime(ghostTime);
        if(ghostTime < 3.0f && ghostTime > 0)
        {
            foreach(GhostController ghost in ghosts)
            {
                ghost.Recover();
            }
        }
        else if(ghostTime <= 0)
        {
            foreach(GhostController ghost in ghosts)
            {
                ghost.BeNormal();
            }
        }
    }

    public void ChangeScore(int x)
    {
        score += x;
        scoreDisplay.text = FormatScore(score);
    }

    public void HandleCollision(GameObject other)
    {
        switch (other.tag)
        {
            case "Pellet":
                ChangeScore(10);
                Destroy(other);
                break;
            case "Power Pill":
                ChangeScore(50);
                Destroy(other);
                ScareGhosts();
                break;
            case "Cherry":
                ChangeScore(100);
                Destroy(other);
                break;
            default:
                break;
        }
    }

    public void ScareGhosts()
    {
        foreach(GhostController ghost in ghosts)
        {
            ghost.Scare();
        }
        ghostUnscaredTime = Time.timeSinceLevelLoad + 10f;
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

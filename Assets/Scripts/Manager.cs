using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TMP_Text timeDisplay;
    public TMP_Text ghostTimeDisplay;
    public TMP_Text scoreDisplay;
    public Healthbar lifeIndicator;
    public TMP_Text countdown;
    public GameObject cover;
    [SerializeField]
    GhostController[] ghosts;
    [SerializeField]
    PacStudentController pacStudent;
    [SerializeField]
    MusicManager musicManager;
    private int score = 0;
    private int health = 3;
    private float ghostUnscaredTime = 0f;
    private bool playerDeathSequenceActive;
    private float startTime;
    private bool active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Awake()
    {
        for (int i = 0; i < ghosts.Length; i++)
            Debug.Log($"Awake: ghosts[{i}] = {(ghosts[i] ? ghosts[i].name : "NULL")}", this);
    }*/

    void Start()
    {
        scoreDisplay.text = FormatScore(score);
        lifeIndicator.SetHealth(health);
        pacStudent.active = false;
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdown.gameObject.SetActive(true);

        cover.GetComponent<SpriteRenderer>().enabled = true;
        countdown.text = "3";
        yield return new WaitForSeconds(1f);

        countdown.text = "2";
        yield return new WaitForSeconds(1f);

        countdown.text = "1";
        yield return new WaitForSeconds(1f);

        countdown.text = "GO";
        yield return new WaitForSeconds(1f);

        
        countdown.gameObject.SetActive(false);
        pacStudent.active = true;
        cover.GetComponent<SpriteRenderer>().enabled = false;
        musicManager.StopIntroMusic();
        musicManager.PlayLoopMusic();
        active = true;
        startTime = Time.timeSinceLevelLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active) return;
        timeDisplay.text = FormatTime(Time.timeSinceLevelLoad - startTime);
        float ghostTime = ghostUnscaredTime - Time.timeSinceLevelLoad;
        ghostTimeDisplay.text = FormatGhostUnscaredTime(ghostTime);

        if (ghostTime <= 0f)
        {
            musicManager.SetScaredMode(false);
            foreach (GhostController ghost in ghosts)
            {
                if (ghost.State != GhostState.DEAD)
                {
                    ghost.BeNormal();
                }
            }
        }
        else
        {
            foreach (GhostController ghost in ghosts)
            {
                if (ghostTime <= 3f)
                {
                    ghost.Recover();
                }
                else
                {
                    ghost.Scare();
                }
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
            case "Ghost":
                HandleGhostCollision(other);
                break;
            default:
                break;
        }
    }

    private void HandleGhostCollision(GameObject ghost)
    {
        if (playerDeathSequenceActive)
        {
            return;
        }

        if (!ghost.TryGetComponent(out GhostController ghostController))
        {
            Debug.LogError($"Ghost object '{ghost.name}' is missing a GhostController.", ghost);
            return;
        }

        if (ghostController.State == GhostState.NORMAL)
        {
            health = Mathf.Max(0, health - 1);
            lifeIndicator.SetHealth(health);
            playerDeathSequenceActive = true;
            StartCoroutine(PlayerDeathSequence());
        }
        else if (ghostController.State == GhostState.SCARED
                 || ghostController.State == GhostState.RECOVERING)
        {
            int stateVersion = ghostController.Die();
            pacStudent.PlayGhostEatSound();
            ChangeScore(300);
            StartCoroutine(RespawnEatenGhost(ghostController, stateVersion));
        }
    }

    private IEnumerator PlayerDeathSequence()
    {
        float deathDuration = pacStudent.BeginDeathSequence();
        foreach (GhostController ghost in ghosts)
        {
            ghost.SetPaused(true);
        }

        yield return new WaitForSeconds(deathDuration);

        ghostUnscaredTime = 0f;
        musicManager.SetScaredMode(false);
        pacStudent.Reset();
        foreach (GhostController ghost in ghosts)
        {
            ghost.Reset();
            ghost.SetPaused(false);
        }

        playerDeathSequenceActive = false;
    }

    private IEnumerator RespawnEatenGhost(GhostController ghost, int stateVersion)
    {
        yield return new WaitForSeconds(3f);

        if (ghost.State != GhostState.DEAD || ghost.StateVersion != stateVersion)
        {
            yield break;
        }

        float remainingScaredTime = ghostUnscaredTime - Time.timeSinceLevelLoad;
        if (remainingScaredTime <= 0f)
        {
            ghost.BeNormal();
        }
        else if (remainingScaredTime <= 3f)
        {
            ghost.Recover();
        }
        else
        {
            ghost.Scare();
        }
    }

    public void ScareGhosts()
    {
        ghostUnscaredTime = Time.timeSinceLevelLoad + 10f;
        musicManager.SetScaredMode(true);
        foreach (GhostController ghost in ghosts)
        {
            ghost.Scare();
        }
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

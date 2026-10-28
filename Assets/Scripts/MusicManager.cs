using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip introMusic;
    [SerializeField]
    private AudioClip loopMusic;
    [SerializeField]
    private AudioClip scaredMusic;

    private bool scaredMode;
    private bool loopMusicStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
    }

    public void StopIntroMusic()
    {
        audioSource.Stop();
    }

    public void PlayLoopMusic()
    {
        loopMusicStarted = true;
        PlayCurrentMusic();
    }

    public void SetScaredMode(bool enabled)
    {
        if (scaredMode == enabled)
        {
            return;
        }

        scaredMode = enabled;
        if (loopMusicStarted)
        {
            PlayCurrentMusic();
        }
    }

    private void PlayCurrentMusic()
    {
        audioSource.Stop();
        audioSource.clip = scaredMode ? scaredMusic : loopMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

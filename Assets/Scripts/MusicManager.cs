using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip introMusic;
    [SerializeField]
    private AudioClip loopMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.clip = introMusic;
        audioSource.Play();
        Invoke(nameof(StopIntroMusic), 3f);
        Invoke(nameof(PlayLoopMusic), 3f);
    }

    private void StopIntroMusic()
    {
        audioSource.Stop();
    }

    private void PlayLoopMusic()
    {
        audioSource.clip = loopMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

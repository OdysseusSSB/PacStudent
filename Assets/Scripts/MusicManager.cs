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
        audioSource.PlayOneShot(introMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

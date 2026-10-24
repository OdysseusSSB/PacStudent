using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // a copy already exists, so remove this one
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
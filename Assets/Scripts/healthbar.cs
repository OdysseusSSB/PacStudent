using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private int health = 3;

    private CanvasRenderer rend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<CanvasRenderer>();
        UpdateHealthBar(health);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar(health);
    }

    void UpdateHealthBar(int health)
    {
        if(health < 0)
        {
            this.GetComponent<Image>().sprite = sprites[0];
        }
        else if(health < sprites.Length)
        {
            this.GetComponent<Image>().sprite = sprites[health]; 
        }
        else
        {
            this.GetComponent<Image>().sprite = sprites[sprites.Length - 1];
        }
    }
}

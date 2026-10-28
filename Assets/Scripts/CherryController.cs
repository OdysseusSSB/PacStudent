using UnityEngine;

public class CherryController : MonoBehaviour
{
    private int width;
    private int height;
    [SerializeField]
    private LevelGenerator level;
    private Tweener tweener;
    private float nextSpawn = 1.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tweener = GetComponent<Tweener>();
        width = level.GetWidth();
        height = level.GetHeight();
    }

    void Spawn()
    {
        width = level.GetWidth();
        height = level.GetHeight();

        switch ((int)(Random.value * 4.0f))
        {
            case 0:
                transform.position = new Vector2(-width/2 - 0.5f, (int)((Random.value - 0.5f) * height) + 0.5f);
                break;
            case 1:
                transform.position = new Vector2(width/2 + 0.5f, (int)((Random.value - 0.5f) * height) + 0.5f);
                break;
            case 2:
                transform.position = new Vector2((int)((Random.value - 0.5f) * width), height/2 + 0.5f);
                break;
            case 3:
                transform.position = new Vector2((int)((Random.value - 0.5f) * width), -height/2 - 0.5f);
                break;
        }

        // Go to opposite side
        tweener.AddTween(transform, transform.position, new Vector3(- transform.position.x, - transform.position.y + 1, 0), 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextSpawn)
        {
            Spawn();
            nextSpawn = Time.time + 10f;
        }
    }
}
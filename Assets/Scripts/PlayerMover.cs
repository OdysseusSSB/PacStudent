using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Tweener tweener;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tweener = GetComponent<Tweener>();
        tweener.AddTween(player.transform, player.transform.position, player.transform.position + new Vector3(5f, 0, 0), 0f, 2.5f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(5f, 0, 0), player.transform.position + new Vector3(5f, -4f, 0), 2.5f, 2f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(5f, -4f, 0), player.transform.position + new Vector3(0, -4f, 0), 4.5f, 2.5f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(0, -4f, 0), player.transform.position + new Vector3(0, 0, 0), 7f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    Animator animator;
    private Tweener tweener;
    private int direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = player.GetComponent<Animator>();

        tweener = GetComponent<Tweener>();
        tweener.AddTween(player.transform, player.transform.position, player.transform.position + new Vector3(5f, 0, 0), 0f, 2.5f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(5f, 0, 0), player.transform.position + new Vector3(5f, -4f, 0), 2.5f, 2f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(5f, -4f, 0), player.transform.position + new Vector3(0, -4f, 0), 4.5f, 2.5f);
        tweener.AddTween(player.transform, player.transform.position + new Vector3(0, -4f, 0), player.transform.position + new Vector3(0, 0, 0), 7f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < 2.5f && direction != 3)
        {
            animator.SetInteger("Direction", 3);
            direction = 3;
        }
        else if(Time.time >= 2.5f && Time.time < 4.5f && direction != 0)
        {
            animator.SetInteger("Direction", 0);
            direction = 0;
        }
        else if(Time.time >= 4.5f && Time.time < 7f && direction != 1)
        {
            animator.SetInteger("Direction", 1);
            direction = 1;
        }
        else if(Time.time >= 7f && direction != 2)
        {
            animator.SetInteger("Direction", 2);
            direction = 2;
        }
        
    }
}

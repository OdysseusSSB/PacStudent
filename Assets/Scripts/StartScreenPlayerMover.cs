using UnityEngine;

public class StartScreenPlayerMover : MonoBehaviour
{
    private const float MovementSpeed = 2f;

    [SerializeField]
    private GameObject player;
    Animator animator;
    private Tweener tweener;
    private int direction;

    [SerializeField]
    private Vector3[] positions;
    private int currentPositionIndex = 0;
    private float nextMoveTime = 0f;
    void Start()
    {
        animator = player.GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
        animator.SetInteger("Direction", 3);
        animator.SetFloat("Speed", 1f);

        Vector3 startPos = player.transform.position;
        Vector3 endPos = positions[0];
        float duration = (startPos - endPos).magnitude / MovementSpeed;
        tweener.AddTween(player.transform, startPos, endPos, duration);
        nextMoveTime = Time.time + duration;
        currentPositionIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!tweener.TweenExists(player.transform) && Time.time >= nextMoveTime)
        {
            MoveToNextPosition();
        }
    }

    void MoveToNextPosition()
    {
        currentPositionIndex++;
        if(currentPositionIndex >= positions.Length)
        {
            currentPositionIndex = 0;
        }
        switch (currentPositionIndex)
        {
            case 0:
                animator.SetInteger("Direction", 3);
                break;
            case 1:
                animator.SetInteger("Direction", 0);
                break;
            case 2:
                animator.SetInteger("Direction", 1);
                break;
            case 3:
                animator.SetInteger("Direction", 2);
                break;
            default:
                break;
        }
        Vector3 startPos = player.transform.position;
        Vector3 endPos = positions[currentPositionIndex];
        float duration = (startPos - endPos).magnitude / MovementSpeed;
        tweener.AddTween(player.transform, startPos, endPos, duration);
        nextMoveTime = Time.time + duration;
    }
}

using UnityEngine;

public class StartScreenPlayerMover : MonoBehaviour
{
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
        animator.SetInteger("Speed", 1);

        tweener.AddTween(player.transform, player.transform.position, positions[0], 0f, 2.5f);
        nextMoveTime = 2.5f;
        currentPositionIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextMoveTime)
        {
            MoveToNextPosition();
        }
    }

    void MoveToNextPosition()
    {
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
        float duration = (startPos - endPos).magnitude / 2f; // Adjust the speed as needed
        tweener.AddTween(player.transform, startPos, endPos, Time.time, duration);
        currentPositionIndex++;
        nextMoveTime = Time.time + duration;
        Debug.Log($"Moving to position {currentPositionIndex - 1}: {endPos} over {duration} seconds.");
    }
}

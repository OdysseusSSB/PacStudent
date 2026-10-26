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
        Vector3 startPos = player.transform.position;
        Vector3 endPos = positions[currentPositionIndex];
        float duration = (startPos - endPos).magnitude / 2f; // Adjust the speed as needed
        tweener.AddTween(player.transform, startPos, endPos, Time.time, duration);
        currentPositionIndex++;
        nextMoveTime += duration;
        Debug.Log($"Moving to position {currentPositionIndex - 1}: {endPos} over {duration} seconds.");
    }
}

using UnityEngine;

enum Direction
{
    Up = 2,
    Down = 0,
    Left = 1,
    Right = 3
}

public class PacStudentController : MonoBehaviour
{
    Animator animator;
    private AudioSource audioSource;
    private Tweener tweener;
    private KeyCode lastInput;
    private float lastAudioScheduled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private AudioClip footstepClip;
    void Start()
    {
        animator = GetComponent<Animator>();

        tweener = GetComponent<Tweener>();
        tweener.AddTween(transform, transform.position, transform.position + new Vector3(5f, 0, 0), 0f, 2.5f);
        tweener.AddTween(transform, transform.position + new Vector3(5f, 0, 0), transform.position + new Vector3(5f, -4f, 0), 2.5f, 2f);
        tweener.AddTween(transform, transform.position + new Vector3(5f, -4f, 0), transform.position + new Vector3(0, -4f, 0), 4.5f, 2.5f);
        tweener.AddTween(transform, transform.position + new Vector3(0, -4f, 0), transform.position + new Vector3(0, 0, 0), 7f, 2f);

        audioSource = GetComponent<AudioSource>();
        lastAudioScheduled = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateInput();
        Move();
        if (Time.time >= lastAudioScheduled + 0.5f)
        {
            audioSource.PlayOneShot(footstepClip);
            lastAudioScheduled = Time.time;
        }
    }

    private void EvaluateInput()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            lastInput = KeyCode.W;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            lastInput = KeyCode.S;
        }
        else if(Input.GetKeyDown(KeyCode.A))
        {
            lastInput = KeyCode.A;
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            lastInput = KeyCode.D;
        }
    }

    private void Move()
    {
        Direction direction;
        switch(lastInput)
        {
            case KeyCode.W:
                direction = Direction.Up;
                break;
            case KeyCode.S:
                direction = Direction.Down;
                break;
            case KeyCode.A:
                direction = Direction.Left;
                break;
            case KeyCode.D:
                direction = Direction.Right;
                break;
            default:
                return; // No valid input
        }

        if(true) // Collision check
        {
            animator.SetInteger("Direction", (int)direction);
            tweener.AddTween(transform, transform.position, transform.position + DirectionToVector3(direction), Time.time, 0.5f);
        }
    }

    Vector3 DirectionToVector3(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                return new Vector3(0, 1, 0);
            case Direction.Down:
                return new Vector3(0, -1, 0);
            case Direction.Left:
                return new Vector3(-1, 0, 0);
            case Direction.Right:
                return new Vector3(1, 0, 0);
            default:
                return Vector3.zero;
        }
    }
}

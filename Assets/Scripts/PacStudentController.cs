using UnityEngine;

enum Direction
{
    Up = 2,
    Down = 0,
    Left = 1,
    Right = 3,
    None = -1
}

public class PacStudentController : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private LevelGenerator level;
    private AudioSource audioSource;
    private Tweener tweener;
    private KeyCode lastInput;
    private Direction direction = Direction.None;
    private float lastAudioScheduled;
    [SerializeField]
    private ParticleSystem collisionPS;
    [SerializeField]
    private AudioClip footstepClip;
    [SerializeField]
    private AudioClip collisionClip;
    private bool isColliding = false;
    [SerializeField]
    private Manager manager;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", (int)Direction.Right);
        animator.SetFloat("Speed", 0f);
        tweener = GetComponent<Tweener>();

        audioSource = GetComponent<AudioSource>();
        lastAudioScheduled = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateInput();
        if(! tweener.TweenExists(transform))
        {
            animator.SetFloat("Speed", 0f); // Reset to idle when not moving
            lastAudioScheduled = Time.time; // Reset the last audio scheduled time when not moving
            Move();
        }
        else
        {
            if (Time.time >= lastAudioScheduled + 0.1f)
            {
                audioSource.PlayOneShot(footstepClip);
                lastAudioScheduled = Time.time;
            }
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
        Direction inputDirection;
        switch(lastInput)
        {
            case KeyCode.W:
                inputDirection = Direction.Up;
                break;
            case KeyCode.S:
                inputDirection = Direction.Down;
                break;
            case KeyCode.A:
                inputDirection = Direction.Left;
                break;
            case KeyCode.D:
                inputDirection = Direction.Right;
                break;
            default:
                inputDirection = direction;
                break;
        }

        Vector3 targetPosition = transform.position + DirectionToVector3(inputDirection);
        if(! level.IsInYBound(targetPosition))
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
            return;
        }

        if(! level.IsInXBound(targetPosition))
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            return;
        }

        if(! level.IsWall(targetPosition)) // Collision check
        {
            direction = inputDirection;
            animator.SetInteger("Direction", (int)direction);
            tweener.AddTween(transform, transform.position, transform.position + DirectionToVector3(direction), Time.time, 0.2f);
            isColliding = false;
        }
        else if(! level.IsWall(transform.position + DirectionToVector3(direction)))
        {
            tweener.AddTween(transform, transform.position, transform.position + DirectionToVector3(direction), Time.time, 0.2f);
            isColliding = false;
        }
        else
        {
            if(!isColliding) {
                ParticleSystem collisionPSInstance = Instantiate(collisionPS, transform.position + DirectionToVector3(direction) * 0.5f, Quaternion.identity);
                audioSource.PlayOneShot(collisionClip);
                isColliding = true;
                /*Debug.Log("Collided");
                Debug.Log(collisionPSInstance);
                */
            }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        manager.HandleCollision(other.gameObject);
    }
}

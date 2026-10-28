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
    private static readonly int DirectionParameter = Animator.StringToHash("Direction");
    private static readonly int AliveParameter = Animator.StringToHash("Alive");
    private static readonly int SpeedParameter = Animator.StringToHash("Speed");
    private static readonly int DeathState = Animator.StringToHash("Base Layer.Player Death");

    private Animator animator;
    [SerializeField]
    private LevelGenerator level;
    private AudioSource audioSource;
    private Tweener tweener;
    private KeyCode lastInput;
    private Direction direction = Direction.None;
    private float lastAudioScheduled;
    private Vector3 startPosition;
    [SerializeField]
    private ParticleSystem collisionPS;
    [SerializeField]
    private AudioClip footstepClip;
    [SerializeField]
    private AudioClip collisionClip;
    [SerializeField]
    private AudioClip cherryClip;
    [SerializeField]
    private AudioClip pelletClip;
    [SerializeField]
    private AudioClip deathClip;
    [SerializeField]
    private AudioClip ghostEatClip;
    private bool isColliding = false;
    private bool isDying;
    private bool waitingForInput;
    public bool active = false;
    [SerializeField]
    private Manager manager;

    private const float DeathAnimationDuration = 0.52f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
        startPosition = transform.position;
    }

    void Start()
    {
        animator.SetInteger(DirectionParameter, (int)Direction.Right);
        animator.SetFloat(SpeedParameter, 0f);
        lastAudioScheduled = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            if(isDying)
            {
                return;
            }

            EvaluateInput();
            if (waitingForInput)
            {
                if (lastInput == KeyCode.None)
                {
                    return;
                }

                waitingForInput = false;
            }

            if(! tweener.TweenExists(transform))
            {
                animator.SetFloat(SpeedParameter, 0f); // Reset to idle when not moving
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
            animator.SetInteger(DirectionParameter, (int)direction);
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (!active || isDying || waitingForInput)
        {
            return;
        }

        manager.HandleCollision(other.gameObject);
        switch(other.tag)
        {
            case "Cherry":
            audioSource.PlayOneShot(cherryClip);
            break;
            case "Pellet":
            audioSource.PlayOneShot(pelletClip);
            break;
            default:
            break;
        }
    }

    public float BeginDeathSequence()
    {
        isDying = true;
        waitingForInput = false;
        lastInput = KeyCode.None;
        direction = Direction.None;
        tweener.CancelTween(transform);
        audioSource.Stop();
        animator.SetFloat(SpeedParameter, 0f);
        animator.SetBool(AliveParameter, false);
        animator.Play(DeathState, 0, 0f);
        audioSource.PlayOneShot(deathClip);

        ParticleSystem deathEffect = Instantiate(collisionPS, transform.position, Quaternion.identity);
        ParticleSystem.MainModule main = deathEffect.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
        deathEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        deathEffect.Play();

        return DeathAnimationDuration;
    }

    public void PlayGhostEatSound()
    {
        audioSource.PlayOneShot(ghostEatClip);
    }

    public void StopMovement()
    {
        tweener.CancelTween(transform);
        lastInput = KeyCode.None;
        direction = Direction.None;
        animator.SetFloat(SpeedParameter, 0f);
    }

    public void Reset()
    {
        tweener.CancelTween(transform);
        transform.position = startPosition;
        animator.SetBool(AliveParameter, true);
        animator.SetInteger(DirectionParameter, (int)Direction.Right);
        animator.SetFloat(SpeedParameter, 0f);
        direction = Direction.None;
        lastInput = KeyCode.None;
        lastAudioScheduled = Time.time;
        isDying = false;
        waitingForInput = true;
    }
}

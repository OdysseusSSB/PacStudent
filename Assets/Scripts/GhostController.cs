using UnityEngine;

public enum GhostState
{
    NORMAL = 0,
    SCARED = 1,
    DEAD = 2,
    RECOVERING = 3
}

public class GhostController : MonoBehaviour
{
    private GhostState state = GhostState.NORMAL;
    private Animator animator;
    private Vector3 home;
    private bool isPaused;
    private float animatorSpeed = 1f;
    private int stateVersion;

    void Awake()
    {
        animator = GetComponent<Animator>();
        home = transform.position;
    }

    public GhostState State => state;
    public int StateVersion => stateVersion;

    public void Recover()
    {
        if (state == GhostState.DEAD || state == GhostState.RECOVERING)
        {
            return;
        }

        SetState(GhostState.RECOVERING);
    }

    public void BeNormal()
    {
        if (state == GhostState.NORMAL)
        {
            return;
        }

        SetState(GhostState.NORMAL);
    }

    public void Scare()
    {
        if (state == GhostState.DEAD || state == GhostState.SCARED)
        {
            return;
        }

        SetState(GhostState.SCARED);
    }

    public int Die()
    {
        if (state != GhostState.DEAD)
        {
            SetState(GhostState.DEAD);
        }

        return stateVersion;
    }

    public void SetPaused(bool paused)
    {
        if (isPaused == paused)
        {
            return;
        }

        isPaused = paused;
        if (paused)
        {
            animatorSpeed = animator.speed;
            animator.speed = 0f;
        }
        else
        {
            animator.speed = animatorSpeed;
        }
    }

    public void Reset()
    {
        stateVersion++;
        transform.position = home;
        BeNormal();
    }

    private void SetState(GhostState newState)
    {
        state = newState;
        animator.SetInteger("State", (int)state);
    }
}

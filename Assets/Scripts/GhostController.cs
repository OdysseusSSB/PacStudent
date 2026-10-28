using UnityEngine;

enum State
{
    NORMAL = 0,
    SCARED = 1,
    DEAD = 2,
    RECOVERING = 3
}

public class GhostController : MonoBehaviour
{
    private State state = State.NORMAL;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Recover()
    {
        state = State.RECOVERING;
        animator.SetInteger("State", (int)state);
    }

    public void BeNormal()
    {
        state = State.NORMAL;
        animator.SetInteger("State", (int)state);
    }

    public void Scare()
    {
        state = State.SCARED;
        animator.SetInteger("State", (int)state);
    }

    public void Die()
    {
        state = State.DEAD;
        animator.SetInteger("State", (int)state);
    }
}

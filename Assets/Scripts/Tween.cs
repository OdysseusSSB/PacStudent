using UnityEngine;

public class Tween
{
    public Tween(Transform target, Vector3 startPos, Vector3 endPos, float startTime, float duration)
    {
        Target = target;
        StartPos = startPos;
        EndPos = endPos;
        StartTime = startTime;
        Duration = duration;
    }

    public void UpdatePosition()
    {
        Target.position = Vector3.Lerp(StartPos, EndPos, Cube(Time.time - StartTime) / Cube(Duration));
    }

    private float Cube(float x)
    {
        return (x * x * x);
    }
    public Transform Target { get; private set; }

    public Vector3 StartPos { get; private set; }

    public Vector3 EndPos { get; private set; }

    public float StartTime { get; private set; }

    public float Duration { get; private set; }
}
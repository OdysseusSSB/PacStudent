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

    public bool UpdatePosition()
    {
        if(Time.time > StartTime + Duration)
        {
            Target.position = EndPos;
            return true;
            //Tween is complete
        }
        else if (Time.time > StartTime)
        {
            Target.position = Vector3.Lerp(StartPos, EndPos, (Time.time - StartTime) / Duration);
        }
        return false;
    }
    public Transform Target { get; private set; }

    public Vector3 StartPos { get; private set; }

    public Vector3 EndPos { get; private set; }

    public float StartTime { get; private set; }

    public float Duration { get; private set; }
}
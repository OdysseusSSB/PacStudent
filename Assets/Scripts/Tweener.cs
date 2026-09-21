using UnityEngine;
using System.Collections.Generic;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;

    private List<Tween> activeTweens = new List<Tween>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < activeTweens.Count; i++)
        {
            Tween tween = activeTweens[i];
            if(tween.UpdatePosition())
            {
                activeTweens.RemoveAt(i);
                i--;
            }
        }
    }

    public bool TweenExists(Transform target)
    {
        return activeTweens.Exists(t => t.Target == target);
    }

    public void AddTween(Transform target, Vector3 startPos, Vector3 endPos, float duration)
    {
        activeTweens.Add(new Tween(target, startPos, endPos, Time.time, duration));
    }
    public void AddTween(Transform target, Vector3 startPos, Vector3 endPos, float startTime, float duration)
    {
        activeTweens.Add(new Tween(target, startPos, endPos, startTime, duration));
    }
}
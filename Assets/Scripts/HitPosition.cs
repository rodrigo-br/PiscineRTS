using UnityEngine;

public class HitPosition
{
    public Transform HitPositionTransform { get; private set; }
    public Transform Attacker { get; private set; }
    public Transform Target { get; private set; }
    public bool IsOccupied { get; private set; }

    public HitPosition(Transform hitPositionTransform, Transform target)
    {
        Attacker = null;
        HitPositionTransform = hitPositionTransform;
        Target = target;
        IsOccupied = false;
    }

    public void Occupy(Transform attacker)
    {
        Attacker = attacker;
        IsOccupied = true;
    }

    public void Release()
    {
        Attacker = null;
        IsOccupied = false;
    }
}

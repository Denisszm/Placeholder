using UnityEngine;
using System.Collections.Concurrent;

public abstract class FSM
{
    public abstract void OnEnter();
    public abstract void OnCall();
    public abstract void OnExit();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABObserver
{
    public abstract void ReceiveMsg(string msg);
}

public class Observer : ABObserver
{
    public override void ReceiveMsg(string msg)
    {
        throw new System.NotImplementedException();
    }
}
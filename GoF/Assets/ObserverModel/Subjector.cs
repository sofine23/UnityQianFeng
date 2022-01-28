using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 抽象的通知者
/// </summary>
public interface ISubject
{
    void AddObserver(ABObserver observer);

    void RemoveObserver(ABObserver observer);

    void Notify();
}

public class Subjector : MonoBehaviour, ISubject
{
    private IList<ABObserver> observers;

    private string msg = null;

    private void Awake()
    {
        observers = new List<ABObserver>();
    }

    public void Notify()
    {
        foreach (ABObserver item in observers)
        {
            item.ReceiveMsg(msg);
        }
    }

    public void AddObserver(ABObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(ABObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //如果满足条件就
        Notify();
    }
}
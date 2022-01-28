using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class Test : MonoBehaviour
{
    private State idle;

    // Start is called before the first frame update
    private void Start()
    {
        idle = new State("Idle");
        idle.OnStateEnter += objects => { Debug.Log("Idle Enter"); };
        idle.OnStateUpdate += objects => { Debug.Log("Idle Update"); };
        idle.OnStateExit += objects => { Debug.Log("Idle Exit"); };
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            idle.EnterState(null, null);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            idle.ExitState(null);
        }
    }
}
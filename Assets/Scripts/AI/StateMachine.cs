using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {
    public List<State> states { get; private set; }
    public State current { get; private set; }
    
	// Use this for initialization
	void Start () {
        states = new List<State>();
	}

    public void AddState(State state) {
        states.Add(state);

        if (current == null) {
            current = state;
        }
    }

    public void AddTransition(Transition t) {
        t.start.AddTransition(t);
    }
	
	// Update is called once per frame
	void Update () {
        current.Update();
        current = current.getTransition();
	}
}

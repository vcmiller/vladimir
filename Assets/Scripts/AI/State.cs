using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class State {
    public Action action { get; private set; }
    public List<Transition> transitions { get; private set; }

    public State(Action action) {
        transitions = new List<Transition>();
        this.action = action;
    }

    public void AddTransition(Transition t) {
        if (t.end != this && t.start == this && !transitions.Contains(t)) {
            transitions.Add(t);
        } else {
            throw new Exception("Invalid transition.");
        }
    }

    public State getTransition() {
        foreach (Transition t in transitions) {
            if (t.CanEnter()) {
                return t.Enter();
            }
        }

        return this;
    }

    public void Update() {
        action();
    }

    public delegate void Action();
}

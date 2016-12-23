using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition {
    public State start { get; private set; }
    public State end { get; private set; }

    public Predicate predicate { get; private set; }
    public State.Action action { get; private set; }

    public Transition(State start, State end, Predicate predicate, State.Action action = null) {
        this.start = start;
        this.end = end;
        this.predicate = predicate;
        this.action = action;
    }

    public bool CanEnter() {
        return predicate();
    }

    public State Enter() {
        if (action != null) {
            action();
        }

        return end;
    }

    public delegate bool Predicate();

}

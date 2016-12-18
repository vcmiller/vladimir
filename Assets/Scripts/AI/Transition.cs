using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition {
    public State start { get; private set; }
    public State end { get; private set; }

    public Predicate predicate { get; private set; }

    public Transition(State start, State end, Predicate predicate) {
        this.start = start;
        this.end = end;
        this.predicate = predicate;
    }

    public bool CanEnter() {
        return predicate();
    }

    public delegate bool Predicate();

}

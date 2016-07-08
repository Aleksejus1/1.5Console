using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour {
    private float CurrentTime;
    public float currentTime { get { return CurrentTime; } }
    public float time;
    public bool active = true;
    public Func<Action> endFunction,updateFunction,resetFunction;

	void Awake () {
        reset(active);
	}
	void Update () {
        if(active&&CurrentTime>0) {
            CurrentTime-=Time.deltaTime;
            if(CurrentTime<0) {
                CurrentTime = 0;
                endFunction();
            }
            else updateFunction();
        }
	}
    public void reset(bool active) {
        if(resetFunction!=null) resetFunction();
        CurrentTime = time;
        this.active = active;
    }
    public float progress() { return CurrentTime/time; }
}

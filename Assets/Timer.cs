using UnityEngine;
using System.Collections;
using System;

public class Timer : MonoBehaviour {
    private float CurrentTime;
    public float currentTime { get { return CurrentTime; } }
    public float time;
    public Func<Action> endFunction,updateFunction;

	void Start () {
        reset();
	}
	void Update () {
        if(CurrentTime>0) {
            CurrentTime-=Time.deltaTime;
            if(CurrentTime<0) {
                CurrentTime = 0;
                endFunction();
            }
            else updateFunction();
        }
	}
    public void reset() {
        CurrentTime = time;
    }
    public float progress() { return CurrentTime/time; }
}

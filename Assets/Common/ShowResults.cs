using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowResults : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string text = "";
		text += "Average frame time:\t"+Profiler.FrameTimeAverage + "\n";
		text += "Standard Deviation:\t"+Profiler.FrameTimeStdDev + "\n";
		guiText.text = text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

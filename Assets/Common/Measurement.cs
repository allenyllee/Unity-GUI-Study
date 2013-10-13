using UnityEngine;
using UnityEditorInternal;
using System.Collections;

public class Measurement : MonoBehaviour {
	
	private const int SamplesToCollect = 1000;
	
	private int startFrameIndex;
	
	void Start() {
		Profiler.logFile = "profileData.log";
		Profiler.enableBinaryLog = true;
		Profiler.enabled = true;
		
		startFrameIndex = ProfilerDriver.lastFrameIndex;
	}
	
	void Update () {
		int nrOfSamples = ProfilerDriver.lastFrameIndex - startFrameIndex;
		
		//stop measuring after collecting the required number of samples and discarding the initial ones
		if(nrOfSamples >= SamplesToCollect) {
			//Profiler.enableBinaryLog = false;
			//Application.LoadLevel("ShowResults");
			
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}
}

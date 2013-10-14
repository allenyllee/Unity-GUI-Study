using UnityEngine;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

public class Measurement : MonoBehaviour {
	
	private const int SamplesToCollect = 10000;
	private const int SamplesToDiscard = 200;
	
	public static Dictionary<string, List<float>> Samples;
	
	private int startFrameIndex;
	
	// output variables used when sampling frame times in MeasureStat
	private float[] sample = new float[1];
	private float maxValue; //never used, but needed when querying profiler data
	
	
	void Start() {
		startFrameIndex = ProfilerDriver.lastFrameIndex;
		
		Samples = new Dictionary<string, List<float>>();
		
		Samples["Rendering"]        = new List<float>();
		Samples["Scripts"]          = new List<float>();
		Samples["Physics"]          = new List<float>();
		Samples["GarbageCollector"] = new List<float>();
		Samples["VSync"]            = new List<float>();
		Samples["Others"]           = new List<float>();
	}
	
	void Update () {
		
		int nrOfSamples = ProfilerDriver.lastFrameIndex - startFrameIndex;
		
		// discard the initial samples to avoid measuring the overhead while loading the scene
		if(nrOfSamples <= SamplesToDiscard)
			return;
			
		//stop measuring after collecting the required number of samples and discarding the initial ones
		if(nrOfSamples > SamplesToCollect + SamplesToDiscard) {
			Application.LoadLevel("ShowResults");
			return;
		}
		
		// Collect frame times in each relevant area
		MeasureStat("Rendering");
		MeasureStat("Scripts");
		MeasureStat("Physics");
		MeasureStat("GarbageCollector");
		MeasureStat("VSync");
		MeasureStat("Others");
		
	}
	
	private void MeasureStat(string statLabel) {
		//Collect sample value from profiler, for last frame
		ProfilerDriver.GetStatisticsValues (
			ProfilerDriver.GetStatisticsIdentifier(statLabel),
			ProfilerDriver.lastFrameIndex,
			1.0f, sample, out maxValue
		);
		
		Samples[statLabel].Add(sample[0]);
	}
}

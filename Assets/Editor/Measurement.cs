using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/*
 * Measurement is an editor window responsible for collecting data while the profiler runs.
 * Usage:
 * Start program and attach the profiler, make sure the profiler is initially not recording.
 * Start the recording in the profiler to start the experiment data collection.
 * Data recording stops automatically when the number of samples specified by SamplesToCollect have been collected.
 * The results are printed to the debug log in the editor console.
 * If you want to run the experiment again, make sure to stop the recording first to reset the collected data.
 */
public class Measurement : EditorWindow {
	
	private const int SamplesToCollect = 10000;
	private const int SamplesToDiscard = 100;
	
	private Dictionary<string, List<float>> samples;
	private bool started;
	private bool finished;
	private int startFrameIndex;
	private int lastNrOfSamples;
	
	// output variables used when sampling frame times in MeasureStat
	private float[] sampleBuffer;
	private float maxValue; //never used, but needed when querying profiler data
	
	[MenuItem ("GUI Experiment/Measurement Window")]
	static void CreateWindow () {
		Measurement window = 
			(Measurement)EditorWindow.GetWindow(typeof(Measurement));
	}
	
	void Start() {
		started = false;
		finished = false;
	}
	
	void startExperiment() {
		sampleBuffer = new float[1];
		
		startFrameIndex = ProfilerDriver.lastFrameIndex;
		lastNrOfSamples = -1;
		
		samples = new Dictionary<string, List<float>>();
		
		samples["Rendering"]        = new List<float>();
		samples["Scripts"]          = new List<float>();
		samples["Physics"]          = new List<float>();
		samples["GarbageCollector"] = new List<float>();
		samples["VSync"]            = new List<float>();
		samples["Others"]           = new List<float>();
		
		Debug.Log ("Experiment started");
	}
	
	void Update () {
		
		// Stop measuring if profiler is disabled.
		if(!Profiler.enabled) {
			started = false;
			finished = false;
			return;
		}
		
		// If finished measuring, wait for next experiment to start.
		if(finished) {
			return;
		}
		
		//Reset data and start measuring when starting profiler.
		//Only start if not finished
		if(!started) {
			if(Profiler.enabled) {
				started = true;
				startExperiment();
			}
			
			return;
		}
		
		int nrOfSamples = ProfilerDriver.lastFrameIndex - startFrameIndex;
		if(nrOfSamples > lastNrOfSamples) {
			lastNrOfSamples = nrOfSamples;
			
			//Print progress in log
			if(nrOfSamples % 200 == 0)
				Debug.Log("Processed samples: " + nrOfSamples + " / " + (SamplesToCollect + SamplesToDiscard));
			
			// discard the initial samples to avoid measuring the overhead while loading the scene
			if(nrOfSamples <= SamplesToDiscard)
				return;
				
			//stop measuring after collecting the required number of samples and discarding the initial ones
			if(nrOfSamples > SamplesToCollect + SamplesToDiscard) {
				//Application.LoadLevel("ShowResults");
				Debug.Log(GetResults());
				finished = true;
				started = false;
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
	}
	
	private void MeasureStat(string statLabel) {
		
		//Collect sample value from profiler, for last frame
		ProfilerDriver.GetStatisticsValues (
			ProfilerDriver.GetStatisticsIdentifier(statLabel),
			ProfilerDriver.lastFrameIndex,
			1.0f, sampleBuffer, out maxValue
		);
		
		samples[statLabel].Add(sampleBuffer[0]);
	}
	
	private string GetResults() {
		string text = "";
		text += getFormattedStat("Rendering");
		text += getFormattedStat("Scripts");
		text += getFormattedStat("Physics");
		text += getFormattedStat("GarbageCollector");
		text += getFormattedStat("VSync");
		text += getFormattedStat("Others");
		
		return text;
	}
	
	private string getFormattedStat(string statLabel) {
		
		// Calculate average value
		float average = samples[statLabel].Average();
		
		// Calculate error bars
		// These are based on standard deviations calculated separately for values above and below the average.
		float sumHigher = 0;
		float sumLower = 0;
		int countHigher = 0;
		int countLower = 0;
		foreach(float sample in samples[statLabel]) {
			if(sample >= average) {
				sumHigher += Mathf.Pow(sample - average, 2);
				countHigher++;
			}
			if(sample <= average) {
				sumLower += Mathf.Pow(sample - average, 2);
				countLower++;
			}
		}
		float ErrorBarHigher = 0;
		float ErrorBarLower = 0;
		if(countHigher > 2)
			ErrorBarHigher = average + Mathf.Sqrt((sumHigher) / (countHigher-1));
		if(countLower > 2)
			ErrorBarLower = average - Mathf.Sqrt((sumLower) / (countLower-1));
		
		return statLabel + " time:\t" + formatTime(average)
			+ " (" + formatTime(ErrorBarLower) + " .. " + formatTime(ErrorBarHigher) + ")" 
			+ " ms\n";
	}
	
	private string formatTime(float nanoSeconds) {
		return (nanoSeconds / 1000000).ToString("0.0000");
	}
}

using UnityEngine;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShowResults : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string text = "";
		text += getFormattedResults("Rendering");
		text += getFormattedResults("Scripts");
		text += getFormattedResults("Physics");
		text += getFormattedResults("GarbageCollector");
		text += getFormattedResults("VSync");
		text += getFormattedResults("Others");
		guiText.text = text;
	}
	
	private static string getFormattedResults(string statLabel) {
		List<float> samples = Measurement.Samples[statLabel];
		
		// Calculate average value
		float average = samples.Average();
		
		// Calculate error bars
		// These are based on standard deviations calculated separately for values above and below the average.
		float sumHigher = 0;
		float sumLower = 0;
		int countHigher = 0;
		int countLower = 0;
		foreach(float sample in samples) {
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
	
	private static string formatTime(float nanoSeconds) {
		return (nanoSeconds / 1000000).ToString("0.0000");
	}
}

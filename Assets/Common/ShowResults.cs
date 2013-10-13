using UnityEngine;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ShowResults : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Profiler.AddFramesFromFile("profileData.log");
		string text = "";
		/*text += getFormattedResults("Rendering");
		text += getFormattedResults("Scripts");
		text += getFormattedResults("Physics");
		text += getFormattedResults("VSync");*/
		guiText.text = text;
	}
	
	private static string getFormattedResults(string statLabel) {
		float maxValue;
		int firstFrame = ProfilerDriver.firstFrameIndex;
		float[] results = new float[ProfilerDriver.lastFrameIndex - firstFrame];
		
		//fetch requested stats into results array
		ProfilerDriver.GetStatisticsValues (
			ProfilerDriver.GetStatisticsIdentifier(statLabel),
			firstFrame, 1.0f, results, out maxValue
		);
		
		// calculate and format average and standard deviation
		string average = formatTime(results.Average());
		string stdDev = formatTime(standardDeviation(results));
		
		return statLabel + " time:\t" + average + " ± " + stdDev + " ms\n";
	}
	
	private static string formatTime(float nanoSeconds) {
		return (nanoSeconds / 1000000).ToString("0.0000");
	}
	
	private static float standardDeviation(IEnumerable<float> values)
	{   
		float result = 0;
		if (values.Count() > 0) 
		{      
			//Compute the Average      
			float avg = values.Average();
			
			//Perform the Sum of squared (value-avg)
			float sum = values.Sum(d => Mathf.Pow(d - avg, 2));
			
			//Put it all together      
			result = Mathf.Sqrt((sum) / (values.Count()-1));   
		}   
		return result;
	}
}

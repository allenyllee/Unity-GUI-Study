using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Profiler : MonoBehaviour {
	
	public static float FrameTimeAverage { get; private set; }
	public static float FrameTimeStdDev  { get; private set; }
	
	private static int frameCounter = 0;
	private static List<float> deltaTimes = new List<float>();
	private static bool isFirstFrame = true;
	private static float lastTime;
	private static bool finished = false;
	
	void Update () {
		if(finished)
			return;
		
		// Begin measuring after 1 second (to avoid measuring scene loading overhead)
		if(Time.timeSinceLevelLoad < 1)
			return;
		
		if(isFirstFrame) {
			lastTime = Time.realtimeSinceStartup;
			isFirstFrame = false;
			return;
		}
		
		frameCounter++;
		float currentTime = Time.realtimeSinceStartup;
		float elapsedTime = currentTime - lastTime;
		deltaTimes.Add(elapsedTime);
		lastTime = currentTime;
		
		//stop measuring after 11 seconds to get an actual measurement period of 10 seconds
		if(Time.timeSinceLevelLoad > 11) {
			CalculateData();
			finished = true;
			Application.LoadLevel ("ShowResults");
		}
	}
	
	private static void CalculateData() {
		FrameTimeAverage = deltaTimes.Average();
		FrameTimeStdDev  = CalculateStdDev(deltaTimes);
	}
	
	private static float CalculateStdDev(IEnumerable<float> values)
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

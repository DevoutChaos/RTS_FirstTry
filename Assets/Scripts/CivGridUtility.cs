using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CivGridUtility
{
	public static void ToSingleArray (CombineInstance[,] doubleArray, out CombineInstance[] singleArray)
	{
		List<CombineInstance> combineList = new List<CombineInstance> ();

		foreach (CombineInstance combine in doubleArray) {
			combineList.Add (combine);
		}

		singleArray = combineList.ToArray ();
	}
}

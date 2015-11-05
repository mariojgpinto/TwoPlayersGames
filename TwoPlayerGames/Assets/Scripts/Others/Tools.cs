using System;

public class Tools {
	public static System.Collections.Generic.List<T> Randomize<T>(System.Collections.Generic.List<T> list)
	{
		System.Collections.Generic.List<T> randomizedList = new System.Collections.Generic.List<T>();
		Random rnd = new Random();
		while (list.Count > 0)
		{
			int index = rnd.Next(0, list.Count); //pick a random item from the master list
			randomizedList.Add(list[index]); //place it at the end of the randomized list
			list.RemoveAt(index);
		}
		return randomizedList;
	}
}

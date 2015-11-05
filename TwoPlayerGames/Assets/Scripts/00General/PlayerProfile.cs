using UnityEngine;
using System;
using System.Collections;


[Serializable]
public class PlayerProfile {
	#region VARIABLES
	public int id;

	public string name = "";
	public int age = 18;
	public int sex = 0;
 
//	public bool randomColor;
	public Color preferedColor;

	public bool hasPhoto = false;
//	public Texture2D photo;
	#endregion


	public override string ToString ()
	{
		string str = "";

		str += "Player Profile:\n";
		str += "Name:" + name + "\n";
		str += "Age:" + age + "\n";
		str += "Sex:" + sex + "\n";
		str += "Color:" + preferedColor + "\n";
		str += "Photo:" + hasPhoto + "\n";



		return str;
	}
}

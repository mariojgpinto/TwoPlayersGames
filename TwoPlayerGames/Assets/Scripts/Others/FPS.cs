using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {
	static public float _frame_rate;
	
	int   _counter;
	float _last_tick;

	// Use this for initialization
	void Start () {
		_frame_rate = 0;
		_counter = 0;
		_last_tick = 0;
	}
	
	// Update is called once per frame
	void Update () {
		++_counter;
		if (_counter == 15)
		{
			float current_tick = Time.realtimeSinceStartup;
			_frame_rate = (float)_counter / (current_tick - _last_tick);
			_last_tick = current_tick;
			_counter = 0;
		}
	}
	
	void OnGUI(){
		//if(Controller.debugOnScreen){
			GUI.TextField(new Rect (0,Screen.height - 30,100,30), System.String.Format("FPS: {0:F2}", FPS._frame_rate));
		//}
	}
}
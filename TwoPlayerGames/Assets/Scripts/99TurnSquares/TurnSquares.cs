using UnityEngine;
using System.Collections;

public class TurnSquares : MonoBehaviour {
	#region VARIABLES
	int boardWidth = 10;
	int boardHeight = 17;
	float step = 2.5f;
	
	System.Collections.Generic.List<Card> cards = new System.Collections.Generic.List<Card>();
	#endregion
	
	#region SETUP
	void CreateCardMatrix()
	{
		GameObject parent = new GameObject();
		parent.name = "Cards";

		for (int i = 0; i < boardWidth; ++i)
		{
			for (int j = 0; j < boardHeight; ++j)
			{
				GameObject go = Instantiate(Resources.Load("Prefabs/99TurnSquares/Card") as GameObject);
				go.transform.position = new Vector3(i * step, j * step, 0);
				cards.Add(go.GetComponent<Card>());
				go.transform.parent = parent.transform;
			}
		}		
	}
	
	void AdjustCamera()
	{
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		
		cam.transform.position = new Vector3(((boardWidth - 1) * step) / 2,
		                                     ((boardHeight - 1) * step) / 2,
		                                     -10);
		
		float v1 = ((boardWidth) * step);
		float v3 = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10)).x;
		float orthograficStep = v3 / 20f;        
		int ac = 0;
		while (v3 < v1 && ac < 100)
		{
			ac++;
			cam.orthographicSize += orthograficStep;
			v3 = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10)).x;
		}
	}
	#endregion
	
	//
	#region GAME_LOGIC
	void ProcessTouch(float x, float y)
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name.StartsWith("Card"))
		{
			hit.collider.GetComponent<FlipCardScript>().FlipCard();
		}
	}

	#endregion
	
	#region UNITY_CALLBACKS
	// Use this for initialization
	void Start () {
//		CreateCardMatrix();
		
//		AdjustCamera();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			ProcessTouch(Input.mousePosition.x, Input.mousePosition.y);
			
		}
		#else
		if (Input.touchCount > 0/* && Input.touches[0].phase == TouchPhase.Began*/)
		//if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
		{
			ProcessTouch(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
		}
		#endif
	}

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("00-Menu");
		}
	}
	#endregion
}

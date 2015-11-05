using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemoryGame : MonoBehaviour
{
    #region VARIABLES
    int useless = 1;

	string prefabPath = "Prefabs/01Memory/Card";

    int boardWidth = 4;
	int boardHeight = 4;

	float stepH = 2.5f;
	float marginH = .0f; //BoundingBox already have margin
	float stepV = 2.5f;
	float marginV = .0f; //BoundingBox already have margin

	GameObject parent;
	System.Collections.Generic.List<Card> cards = new System.Collections.Generic.List<Card>();

	Vector3[] removePoint = new Vector3[2];
	float timeOfExposure = 1;
	bool isBusy = false;

	int touchCount = 0;
	int cardsLeft;
	Card[] selectedCards = new Card[2];

	PlayersScoresGUI score;

    #endregion

    #region SETUP
    void CreateCardMatrix()
    {
		parent = new GameObject();
		parent.name = "Cards";

        for (int i = 0; i < boardWidth; ++i)
        {
            for (int j = 0; j < boardHeight; ++j)
            {
				GameObject go = Instantiate(Resources.Load(prefabPath) as GameObject);
				go.transform.position = new Vector3(i * stepH, j * stepV, 0);
				Card card = go.GetComponent<Card>();
				cards.Add(card);				
				go.transform.parent = parent.transform;
				card.SaveInit();
            }
        }
    }

	void ResetMatrix(){
		cardsLeft = boardWidth * boardHeight;
		int nCombinations = (cardsLeft)/2;
		System.Collections.Generic.List<int> ids_sequence = new System.Collections.Generic.List<int>();
		
		for(int i = 0 ; i < nCombinations ; i++){
			ids_sequence.Add(i);
			ids_sequence.Add(i);
		}
		
		System.Collections.Generic.List<int> ids = Tools.Randomize(ids_sequence);	

		int ac = 0;
		for (int i = 0; i < boardWidth; ++i)
		{
			for (int j = 0; j < boardHeight; ++j)
			{
				cards[ac].SetCard(ids[ac]);
//				cards[ac].SetCard(0);
				cards[ac].ResetStatus();
				ac++;
			}
		}	
		score.ResetScores();

		score.turn = 1;
		score.ChangePlayersTurn();
	}

    void AdjustCamera()
    {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        cam.transform.position = new Vector3(((boardWidth - 1) * stepH) / 2,
		                                     ((boardHeight - 1) * stepV) / 2,
                                             -10);
		cam.transform.position = cam.transform.position + Vector3.up*.5f;

        float v1 = ((boardWidth) * stepH);
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

	#region BUTTON_CALLBACKS
	public void OnButtonPressed(int id){
		switch(id){
		case 1:
			//RETRY
			ResetMatrix();
			score.HideEndPanel();
			break;
		case 2:
			Application.LoadLevel("00-MainScene");
			break;
		default: break;
		}
	}

	#endregion
	
    #region GAME_LOGIC
    void ProcessTouch(float x, float y)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name.StartsWith("Card"))
        {			
			isBusy = true;

			Card card = hit.collider.GetComponent<Card>();

			if(touchCount == 1){
				if(selectedCards[0] == card){
					isBusy = false;
					return;
				}
			}

			selectedCards[touchCount] = card;
			card.TurnCard();
			touchCount++;

			int match = 0;
			if(touchCount == 2){
				if(selectedCards[0].GetID() == selectedCards[1].GetID()){
					match = 1;
					score.AddScore(1);
				}
				else{
					match = 2;
				}

				touchCount = 0;

				StartCoroutine(EndPlay(match));
			}
			else{
				isBusy = false;
			}
        }
    }

	IEnumerator EndPlay(int match){
		if(match == 0){
			yield return new WaitForSeconds(timeOfExposure * .25f);
		} else
		if(match == 1){			
			yield return new WaitForSeconds(timeOfExposure * .25f);
			selectedCards[0].RemoveCard(removePoint[score.turn]);
			selectedCards[1].RemoveCard(removePoint[score.turn]);
			yield return new WaitForSeconds(timeOfExposure * .75f);

			cardsLeft -= 2;

			if(cardsLeft == 0){
				score.EndGame();
			}
		} else
		if(match == 2){
			yield return new WaitForSeconds(timeOfExposure * .6f);			
			selectedCards[0].TurnCard();
			selectedCards[1].TurnCard();

			score.ChangePlayersTurn();
			yield return new WaitForSeconds(timeOfExposure * .4f);
		}

		isBusy = false;
	}


    #endregion

    #region UNITY_CALLBACKS
    // Use this for initialization
	void Start () {
		score = GameObject.Find("CanvasScoreGUI").GetComponent<PlayersScoresGUI>();
		score.buttonRetry.onClick.AddListener(() => OnButtonPressed(1));
		score.buttonExit.onClick.AddListener(() => OnButtonPressed(2));
		score.HideEndPanel();
		score.turn = 0;

		cardsLeft = boardWidth * boardHeight;
		isBusy = false;

		touchCount = 0;

		GameObject go = Instantiate(Resources.Load(prefabPath) as GameObject);
		stepH = go.GetComponent<BoxCollider>().bounds.size.x + marginH;
		stepV = go.GetComponent<BoxCollider>().bounds.size.y + marginV;
		Destroy(go);

        CreateCardMatrix();
		ResetMatrix();
        
        AdjustCamera();

		removePoint[0] = Camera.main.ScreenToWorldPoint(score.textScores[0].gameObject.transform.position);
		removePoint[1] = Camera.main.ScreenToWorldPoint(score.textScores[1].gameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
			if(!isBusy){
            	ProcessTouch(Input.mousePosition.x, Input.mousePosition.y);
			}            
        }
#else
        //if (Input.touchCount > 0/* && Input.touches[0].phase == TouchPhase.Began*/)
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
			if(!isBusy){
            	ProcessTouch(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
			}
        }
#endif

//		GameObject.Find("Text_Log").GetComponent<Text>().text = 
//				"Turn: " + turn + "\n" + 
//				"TouchCount: " + touchCount + "\n" + 
//				"isBusy: " + isBusy + "\n" + 
//				"Cards Left: " + cardsLeft;
    }

	void OnGUI(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.LoadLevel("00-Menu");
		}
	}
    #endregion
}
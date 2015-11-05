using UnityEngine;
using System.Collections;

public class ReversiPiece : MonoBehaviour {
	#region VARIABLES
	public bool isWhite = false;
	public bool isActive = true;

	bool isBusy = false;

	Vector3 defaultScale;
	Vector3 defaultPosition;


	#endregion

	#region SETUP
	public void Init(Vector3 _defaultSize, Vector3 _defaultPosition){
		defaultScale = _defaultSize;
		defaultPosition = _defaultPosition;
	}

	public void SetWhite(){
		isWhite = true;
		this.transform.rotation = Quaternion.Euler(0,180,0);
	}

	public void SetBlack(){
		isWhite = false;
		this.transform.rotation = Quaternion.Euler(0,0,0);
	}
	#endregion

	#region ACTIONS
	public void FlipPiece(){
		if(!isBusy)
			StartCoroutine(FlipPiece_rountine());
	}

	public void JumpPiece(){
		if(!isBusy)
			StartCoroutine(JumpPiece_rountine());
	}

	public void AddPiece(){
		this.gameObject.SetActive(true);		
		isActive = true;

		if(!isBusy)
			StartCoroutine(AddPiece_rountine());
	}

	public void HidePiece(){
		this.transform.localScale = Vector3.zero;
		isActive = false;
		this.gameObject.SetActive(false);
	}

	public void ErrorPiece(bool isWhite){
		this.gameObject.SetActive(true);		
		isActive = true;

		if(!isBusy)
			StartCoroutine(ErrorPiece_rountine(isWhite));
	}

	#endregion

	#region ACTIONS_AUXILIAR
	IEnumerator FlipPiece_rountine(){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .25f;
		Vector3 initialRotation = this.transform.rotation.eulerAngles;
		Vector3 initialPosition = this.transform.position;
		
		isBusy = true;
		
		isWhite = !isWhite;
		
		while(progress < 1)
		{
			this.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * 180 * progress));
			
			if(progress < .5f){
				this.transform.position = Vector3.Lerp(initialPosition, initialPosition - Vector3.forward, progress);
				this.transform.localScale = (defaultScale) + (defaultScale * .5f) * progress;
			}
			else{
				this.transform.position = Vector3.Lerp(initialPosition, initialPosition - Vector3.forward, 1-progress);
				this.transform.localScale = (defaultScale) + (defaultScale * .5f) * (1 - progress);
			}
			
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		
		this.transform.position = initialPosition;
		this.transform.localScale = defaultScale;
		this.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * 180));
		
		isBusy = false;
		
		yield break;
	}

	IEnumerator JumpPiece_rountine(){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .15f;
		
		isBusy = true;
		
		while(progress < 1)
		{
			if(progress < .5f){
				this.transform.localScale = (defaultScale) + (defaultScale * .5f) * progress;
			}
			else{
				this.transform.localScale = (defaultScale) + (defaultScale * .5f) * (1 - progress);
			}
			
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		
		this.transform.localScale = defaultScale;
		
		isBusy = false;
		
		yield break;
	}

	IEnumerator AddPiece_rountine(){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .35f;
		
		this.transform.position = defaultPosition - Vector3.forward;
		this.transform.localScale = Vector3.zero;
		Vector3 initialRotation = Vector3.up * ((isWhite) ? 180 : 0);
		
		isBusy = true;
		
		while(progress < 1)
		{
			this.transform.localScale = defaultScale * progress;			
			this.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * 360 * progress));
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		
		this.transform.localScale = defaultScale;
		this.transform.position = defaultPosition;
		
		if(isWhite){
			SetWhite();
		}
		else{
			SetBlack();
		}
		
		isBusy = false;
		
		yield break;
	}

	IEnumerator ErrorPiece_rountine(bool isWhite){
		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.1
		float duration = .2f;
		
		isBusy = true;

		this.transform.localScale = defaultScale;
		this.transform.position = defaultPosition;

		Renderer[] ren = this.gameObject.GetComponentsInChildren<Renderer>();
		Color[] origColors = new Color[ren.Length];

		for(int i = 0 ; i < ren.Length ; ++i){
			origColors[i] = ren[i].material.color;
		}


		foreach(Renderer r in ren){
			r.material.color = Color.red;
		}

		Color color1 = Color.red;
		Color color2 = isWhite ? Color.white : Color.black;

		Debug.Log("C-" + color2);

		while(progress < 1)
		{
			foreach(Renderer r in ren){
				r.material.color = Color.Lerp(color2, color1, progress);
			}
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		progress = 0; 
		while(progress < 1)
		{
			foreach(Renderer r in ren){
				r.material.color = Color.Lerp(color1, color2, progress);
			}
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		progress = 0; 
		while(progress < 1)
		{
			foreach(Renderer r in ren){
				r.material.color = Color.Lerp(color2, color1, progress);
			}
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}
		progress = 0; 
		while(progress < 1)
		{
			foreach(Renderer r in ren){
				r.material.color = Color.Lerp(color1, color2, progress);
			}
			
			progress += Time.deltaTime/duration;
			yield return true;//new WaitForSeconds(smoothness);
		}

		for(int i = 0 ; i < ren.Length ; ++i){
			ren[i].material.color = origColors[i];
		}

		isBusy = false;

		HidePiece();

		yield break;
	}
	#endregion

	#region UNITY_CALLBACKS
	// Use this for initialization
//	void Start () {
//	
//	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
	#endregion
}

using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour{

	public float baseSpeed;
	private float currentSpeed;
	
	//public bool rushing = false;
	//private float speedMod = 0;

	//float timeLeft = 2f;

	private Rigidbody2D myRigidBody;

	//private Animator myAnim;

	//public GameObject bubbles;

	private int trashCount = 0;
	public float speedDebuffPerKg = 0.1f;
	
	// UI Elements
	public TextMeshProUGUI trashCountText;
	public TextMeshProUGUI speedText;

	private TrashItem trashItem;
	
	void Start (){
		myRigidBody = GetComponent<Rigidbody2D> ();	
		//myAnim = GetComponent<Animator> ();
		
		currentSpeed = baseSpeed;
		UpdateUI();
	}
	
	void Update (){

		//resetBoostTime ();
		controllerManager();
		HandleTrashPickup();
		//myAnim.SetFloat ("Speed", Mathf.Abs(myRigidBody.linearVelocity.x));
	}

	void controllerManager (){
		if (Input.GetAxisRaw ("Horizontal") > 0f) {
			transform.localScale = new Vector3(1f,1f,1f);
			movePlayer ();
		} else if (Input.GetAxisRaw ("Horizontal") < 0f) {			
			transform.localScale = new Vector3(-1f,1f,1f);
			movePlayer ();
		} else if (Input.GetAxisRaw ("Vertical") > 0f) {
			myRigidBody.linearVelocity = new Vector3 (myRigidBody.linearVelocity.x, currentSpeed, 0f);
		} else if (Input.GetAxis ("Vertical") < 0f) {
			myRigidBody.linearVelocity = new Vector3 (myRigidBody.linearVelocity.x, -currentSpeed, 0f);
		}

		/*if(Input.GetButtonDown("Jump") && !rushing ){
			rushing = true;
			speedMod = 2;
			Instantiate (bubbles, gameObject.transform.position, gameObject.transform.rotation);
			movePlayer ();
		} */	
	}

	void HandleTrashPickup()
	{
		if (trashItem != null && Input.GetKeyDown(KeyCode.E))
		{
			PickupTrash(trashItem);
			trashItem = null;
		}
	}

	void movePlayer(){
		if (transform.localScale.x == 1) {
			myRigidBody.linearVelocity = new Vector3 (currentSpeed /*+ speedMod*/, myRigidBody.linearVelocity.y, 0f);	
		} else {
			myRigidBody.linearVelocity = new Vector3 (- (currentSpeed /*+ speedMod*/), myRigidBody.linearVelocity.y, 0f);
		}	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Trash"))
		{
			trashItem = other.GetComponent<TrashItem>();
			Debug.Log("Near trash: " + other.name);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Trash"))
		{
			if (trashItem == other.GetComponent<TrashItem>())
			{
				trashItem = null;
				Debug.Log("Left trash: " + other.name);
			}
		}
	}

	void PickupTrash(TrashItem trash)
	{
		trashCount++;
		
		// Calculate speed debuff
		float speedDebuff = trash.GetWeight() * speedDebuffPerKg;
		currentSpeed = Mathf.Max(0.1f, currentSpeed - speedDebuff); // Ensure speed doesn't go below a minimum
		
		Destroy(trash.gameObject);
		
		UpdateUI();
	}

	void UpdateUI()
	{
		if (trashCountText != null)
		{
			trashCountText.text = "Trash Collected: " + trashCount;
		}
		if (speedText != null)
		{
			speedText.text = "Current Speed: " + currentSpeed.ToString("F2"); // F2 for 2 decimal places
		}
	}

	/*void resetBoostTime(){
		if (timeLeft <= 0) {
			timeLeft = 2f;
			rushing = false;
			speedMod = 0;
		} else if(rushing) {
			timeLeft -= Time.deltaTime;
		}	
	} */

	/* public void hurt(){
		if(!rushing){
			gameObject.GetComponent<Animator> ().Play ("PlayerHurt");		
		}

	} */


}

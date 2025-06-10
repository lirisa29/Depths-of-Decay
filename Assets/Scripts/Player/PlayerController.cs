using UnityEngine;

public class PlayerController : MonoBehaviour{

	public float baseSpeed = 2f;
	[SerializeField] private float swimForce = 4f;
	[HideInInspector] public float currentSpeed;
	
	//public bool rushing = false;
	//private float speedMod = 0;

	//float timeLeft = 2f;

	private Rigidbody2D rb;

	//private Animator myAnim;

	//public GameObject bubbles;

	void Awake()
	{
		currentSpeed = baseSpeed;
	}
	
	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();	
		//myAnim = GetComponent<Animator> ();
		//UpdateUI();
	}
	
	void Update()
	{
		HandleInput ();
		//resetBoostTime ();
		//myAnim.SetFloat ("Speed", Mathf.Abs(myRigidBody.linearVelocity.x));
	}
	
	private void HandleInput()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector2 input = new Vector2(horizontal, vertical);

		// Normalize input for consistent direction
		if (input.sqrMagnitude > 1)
			input = input.normalized;

		// Apply force to simulate swimming
		rb.AddForce(input * swimForce);

		// Limit max swim speed
		if (rb.linearVelocity.magnitude > currentSpeed)
		{
			rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
		}

		// Flip sprite when swimming left/right
		if (horizontal != 0)
		{
			transform.localScale = new Vector3(Mathf.Sign(horizontal), 1f, 1f);
		}
		
		/*if(Input.GetButtonDown("Jump") && !rushing ){
			rushing = true;
			speedMod = 2;
			Instantiate (bubbles, gameObject.transform.position, gameObject.transform.rotation);
			movePlayer ();
		} */
	}

	public void ApplySpeedDebuff(float debuff)
	{
		currentSpeed = Mathf.Max(0.1f, currentSpeed - debuff);
	}
	
	public void ResetSpeed()
	{
		currentSpeed = baseSpeed;
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

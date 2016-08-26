using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public Rigidbody Player;
    public float Speed = 10.0f;
    public float MaxSpeed = 10f;

    // Use this for initialization
	void Start ()
	{
	    Player = GetComponent<Rigidbody>();

	}


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Player.AddForce(new Vector3(h, 0, v) * Speed);
//
//        if (Player.velocity.magnitude > MaxSpeed)
//        {
//            Player.velocity = Player.velocity.normalized*MaxSpeed * Time.fixedDeltaTime;
//        }

    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public Rigidbody player;

    public float speed = 10.0f;

	// Use this for initialization
	void Start ()
	{
	    player = GetComponent<Rigidbody>();

	}


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        player.AddForce(new Vector3(h, 0, v) * speed);

    }
}

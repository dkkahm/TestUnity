using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	MoveCharacter m_move_character;

	SimpleThirdPersonCamera m_camera;

	// Use this for initialization
	void Start () {
		m_move_character = GetComponent<MoveCharacter>();
		m_move_character.SetCharacter(this.transform);	

		m_camera = GetComponent<SimpleThirdPersonCamera>();
		m_camera.SetPlayer(this.transform, new Vector3(), new Vector3(0f, 2f, -4f));
		m_camera.SetCamera(Camera.main.transform);
	}
	
	// Update is called once per frame
	void Update () {
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		m_move_character.MoveForward(ver);
		m_move_character.TurnRight(hor * Mathf.Deg2Rad * 2f);

		float roll = Input.GetButton("Jump") ? 1.0f : 0.0f;
		m_move_character.Roll(roll * Mathf.Deg2Rad);

		m_camera.UpdateCamera(Time.deltaTime);

		Debug.Log(Camera.main.transform.position);
	}
}

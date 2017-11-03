using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
	MoveCharacter m_move_character;

	// Use this for initialization
	void Start () {
		m_move_character = GetComponent<MoveCharacter>();
		m_move_character.SetCharacter(this.transform);	
	}
	
	// Update is called once per frame
	void Update () {
		float hor = Input.GetAxis("Horizontal");
		float ver = Input.GetAxis("Vertical");

		m_move_character.MoveForward(ver);
		m_move_character.TurnRight(hor * Mathf.Deg2Rad * 2f);

		float roll = Input.GetButton("Jump") ? 1.0f : 0.0f;
		Debug.Log(roll);
		m_move_character.Roll(roll * Mathf.Deg2Rad);
	}
}

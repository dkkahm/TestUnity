using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour {
	Transform m_character;

	public void SetCharacter(Transform character) {
		m_character = character;
	}

	public void MoveForward(float distance = 1.0f) {
		m_character.Translate(Vector3.forward * distance);
	}

	public void MoveBackward(float distance = 1.0f) {
		m_character.Translate(Vector3.back * distance);
	}

	public void MoveLeft(float distance = 1.0f) {
		m_character.Translate(Vector3.left * distance);
	}

	public void MoveRight(float distance = 1.0f) {
		m_character.Translate(Vector3.right * distance);
	}

	public void TurnLeft(float rad = 0.1f) {
		m_character.Rotate(-1f * Vector3.up * rad * Mathf.Rad2Deg);
	}

	public void TurnRight(float rad = 0.1f) {
		m_character.Rotate(Vector3.up * rad * Mathf.Rad2Deg);
	}

	public void Roll(float rad = 0.1f) {
		m_character.Rotate(Vector3.forward * rad * Mathf.Rad2Deg);
	}
}

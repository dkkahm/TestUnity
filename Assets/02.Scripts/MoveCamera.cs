using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
	SimplePolarNavigation m_nav;
	GameObject m_pivot;

	// Use this for initialization
	void Start () {
		m_nav = this.GetComponent<SimplePolarNavigation>();
		m_pivot = GameObject.Find("Pivot");

		m_nav.Set(m_pivot.transform.position, this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if(Input.GetKey(KeyCode.LeftControl)) {
			Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
			m_pivot.transform.Translate(right * h * 0.1f);

			Vector3 up = Vector3.up;
			m_pivot.transform.Translate(up * v * 0.1f);

			m_nav.Move(m_pivot.transform.position);
		} else {
			m_nav.RotateHorizontal(h * 0.1f);
			m_nav.RotateVertical(v * 0.1f);
		}

		float wheel = Input.GetAxis("Mouse ScrollWheel");
		m_nav.Backward(wheel * 2f);

		m_nav.UpdatePolar(Time.deltaTime);

		this.transform.position = m_nav.GetPosition();
		this.transform.LookAt(m_pivot.transform);
	}
}

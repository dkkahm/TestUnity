using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePolarNavigation : MonoBehaviour {
	private const float MIN_R = 0.1f;

	class Polar {
		public Polar() {
			r = 1f;
			theta = 0f;
			phi = 0f;
		}

		public Polar(float _r, float _theta, float _phi) {
			r = _r;
			theta = _theta;
			phi = _phi;
		}

		public Vector3 ToCartesian() {
			float x = r * Mathf.Sin(theta);
			float z = - r * Mathf.Cos(theta);

			float y = r * Mathf.Sin(phi);
			float k = Mathf.Cos(phi);

			return new Vector3(x * k, y, z * k);
		}

		public float r;
		public float theta; // form -z
		public float phi; // to y
	};

	Vector3 m_pivot_target;
	Vector3 m_pivot_cur;

	Polar m_polar_target = new Polar();
	Polar m_polar_cur = new Polar();

	float m_speed_rotate_horizontal = 10f;
	float m_speed_rotate_vertical = 10f;
	float m_speed_backward = 10f;
	float m_speed_move = 10f;

	public SimplePolarNavigation() {
	}

	public void Set(Vector3 pivot, Vector3 camera) {
		m_pivot_target = new Vector3 (pivot.x, pivot.y, pivot.z);
		m_pivot_cur = new Vector3 (pivot.x, pivot.y, pivot.z);

		Vector3 diff = camera - pivot;

		m_polar_target.r = diff.magnitude;
		m_polar_target.theta = Mathf.Atan2 (diff.x, -diff.z);

		Vector3 a = new Vector3 (diff.x, 0f, diff.z);
		m_polar_target.phi = Mathf.Acos (Mathf.Clamp(Vector3.Dot (a, diff) / (a.magnitude * diff.magnitude), -1f, 1f));

		m_polar_cur.r = m_polar_target.r;
		m_polar_cur.theta = m_polar_target.theta;
		m_polar_cur.phi = m_polar_target.phi;

		TraceCur("Set");
	}

	public void SetSpeed(float speed_rotate_horizontal, float speed_rotate_vertical, float speed_backward, float speed_move) {
		m_speed_rotate_horizontal = speed_rotate_horizontal;
		m_speed_rotate_vertical = speed_rotate_vertical;
		m_speed_backward = speed_backward;
		m_speed_move = speed_move;
	}

	public void RotateHorizontal(float rad) {
		m_polar_target.theta += rad;
		// Debug.Log("t4:" + m_polar_target.theta);
	}

	public void RotateVertical(float rad) {
		m_polar_target.phi += rad;
		if(m_polar_target.phi > Mathf.PI / 2f * 0.9f)
			m_polar_target.phi = Mathf.PI / 2f * 0.9f;
		if(m_polar_target.phi < -Mathf.PI / 2f * 0.9f)
			m_polar_target.phi = -Mathf.PI / 2f * 0.9f;
	}

	public void Backward(float dist) {
		m_polar_target.r += dist;

		if(m_polar_target.r < MIN_R)
			m_polar_target.r = MIN_R;
	}

	public void Move(Vector3 pivot) {
		m_pivot_target.x = pivot.x;
		m_pivot_target.y = pivot.y;
		m_pivot_target.z = pivot.z;
	}

	public void UpdatePolar(float delta_time) {
		m_polar_cur.r = Mathf.Lerp(m_polar_cur.r, m_polar_target.r, delta_time * m_speed_backward);
		m_polar_cur.theta = Mathf.Lerp(m_polar_cur.theta, m_polar_target.theta, delta_time * m_speed_rotate_horizontal);
		m_polar_cur.phi = Mathf.Lerp(m_polar_cur.phi, m_polar_target.phi, delta_time * m_speed_rotate_vertical);

		m_pivot_cur = Vector3.Lerp(m_pivot_cur, m_pivot_target, delta_time * m_speed_move);
	}

	public Vector3 GetPosition() {
		TraceCur("GetPosition");
		return m_polar_cur.ToCartesian() + m_pivot_cur;
	}

	void TraceCur(string title) {
		// Debug.Log(title + ":CurPolar: r=" + m_polar_cur.r + ",t=" + m_polar_cur.theta + ",p=" + m_polar_cur.phi);
		// Debug.Log(title + ":CurPivot:" + m_pivot_cur);
	}
}

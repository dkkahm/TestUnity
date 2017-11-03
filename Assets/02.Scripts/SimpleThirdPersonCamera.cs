using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleThirdPersonCamera : MonoBehaviour {
	Transform m_camera = null;

	Transform m_player = null;
	Vector3 m_player_pivot = new Vector3();
	Vector3 m_camera_pos_from_pivot = new Vector3(0f, 1.0f, -1.0f);

	float m_speed_camera_move = 0.3f;
	float m_speed_camera_rotation = 0.3f;

	public void SetCamera(Transform camera) {
		m_camera = camera;
	}

	public void SetPlayer(Transform player, Vector3 pivot, Vector3 camera_pos_from_pivot) {
		m_player = player;
		m_player_pivot = pivot;
		m_camera_pos_from_pivot = camera_pos_from_pivot;
	}

	public void SetSpeed(float speed_move, float speed_rotation) {
		m_speed_camera_move = speed_move;
		m_speed_camera_rotation = speed_rotation;
	}

	public void UpdateCamera(float delta_time) {
		if(m_camera != null && m_player != null) {
			Vector3 pivot_pos = m_player.TransformPoint(m_player_pivot);
			// Debug.Log("povot:" + pivot_pos);
			Vector3 camera_pos_target = m_player.TransformPoint(m_player_pivot + new Vector3(0f, 0f, m_camera_pos_from_pivot.z)) + new Vector3(0f, m_camera_pos_from_pivot.y, 0f);
			// Debug.Log("camera_pos_target:" + camera_pos_target);
			Quaternion camera_lookat_target = Quaternion.LookRotation(pivot_pos - camera_pos_target);

			Vector3 camera_pos = Vector3.Lerp(m_camera.transform.position, camera_pos_target, m_speed_camera_move);
			Quaternion camera_lookat = Quaternion.Slerp(m_camera.transform.rotation, camera_lookat_target, m_speed_camera_rotation);

			m_camera.transform.position = camera_pos;
			m_camera.transform.rotation = camera_lookat;
		}
	}
}

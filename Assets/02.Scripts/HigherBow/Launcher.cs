using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {
    public Transform m_arrow_point_tr;

    private Transform m_transform;
    private Transform m_arrow_tr;

    //const float MIN_X_ANGLE = -85f;
    //const float MAX_X_ANGLE = 45f;

    const float MIN_Y_ANGLE = -15f;
    const float MAX_Y_ANGLE = 15f;

    //const float x_rot_speed = 50f;
    const float y_rot_speed = 50f;

    //float m_x_angle = 0f;
    float m_y_angle = 0f;

    // Use this for initialization
    void Start () {
        m_transform = GetComponent<Transform>();
        m_arrow_tr = GameObject.FindGameObjectWithTag("ARROW").GetComponent<Transform>();

        float x_angle = 0f;
        SceneManager.Instance.GetLauncherInitialRotation(ref x_angle, ref m_y_angle);
    }
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        //float v = -1f * Input.GetAxis("Vertical");

        //float x_angle_delta = v * x_rot_speed * Time.deltaTime;
        //m_x_angle += x_angle_delta;
        //if (m_x_angle < MIN_X_ANGLE) m_x_angle = MIN_X_ANGLE;
        //if (m_x_angle > MAX_X_ANGLE) m_x_angle = MAX_X_ANGLE;

        float y_angle_delta = h * y_rot_speed * Time.deltaTime;
        m_y_angle += y_angle_delta;
        if (m_y_angle < MIN_Y_ANGLE) m_y_angle = MIN_Y_ANGLE;
        if (m_y_angle > MAX_Y_ANGLE) m_y_angle = MAX_Y_ANGLE;

        Vector3 angle = new Vector3(0f, m_y_angle, 0f);

        m_transform.localRotation = Quaternion.Euler(angle);

        if (SceneManager.Instance.GS == GameState.IDLE
            || SceneManager.Instance.GS == GameState.AIMING
            || SceneManager.Instance.GS == GameState.PULLING)
        {
            m_arrow_tr.position = m_arrow_point_tr.position;
            m_arrow_tr.rotation = m_arrow_point_tr.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        
    }
}

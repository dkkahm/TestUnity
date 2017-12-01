using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    IDLE,
    AIMING,
    PULLING,
    FLYING,
    ZOOM_IN,
    ZOOM_STAY,
    ZOOM_OUT,
}

public class SceneManager : MonoBehaviour {
    private Launcher m_launcher;
    private Transform m_arrow_tr;
    private Arrow m_arrow;
    private Camera m_camera;
    private Transform m_camera_tr;

    private const float ARROW_SPEED = 30.0f;

    private Vector3 m_camera_normal_pos = new Vector3(0f, 40.5f, -28.0f);
    private Quaternion m_camera_normal_rot = Quaternion.Euler(46f, 0f, 0f);
    private float m_camera_normal_size = 9f;

    private Vector3 m_camera_zoom_pos = new Vector3(2.0159f, 4.294f, 9.9717f);
    private Quaternion m_camera_zoom_rot = Quaternion.Euler(22.381f, -11.241f, 0f);
    private float m_camera_zoom_size = 2f;

    private Vector3 m_camera_target_pos;
    private Quaternion m_camera_target_rot;
    private float m_camera_target_size;
    private float m_camera_zoom_time;

    private const float CAMERA_ZOOM_IN_TIME = 1f;
    private const float CAMERA_ZOOM_STAY_TIME = 1f;
    private const float CAMERA_ZOOM_OUT_TIME = 1f;
    private float m_zoom_start_time = 0f;

    private const int MIN_SCORE = 0;
    private const int MAX_SCORE = 10;
    private const float BULLS_EYE_RANGE = 0.05f;
    private const int BULLS_EYE_SCORE = 20;
    private bool m_is_bulls_eye = false;

    public Text m_score_text;
    Vector3 m_target_pos_for_score;
    private float m_score = 0f;

    private float m_wind = 0f;

    public GameState GS
    {
        get;
        private set;
    }

    private void Awake()
    {
        s_instance = this;

        m_launcher = GameObject.Find("Launcher").GetComponent<Launcher>();

        GameObject arrow_obj = GameObject.FindGameObjectWithTag("ARROW");
        m_arrow = arrow_obj.GetComponent<Arrow>();
        m_arrow_tr = arrow_obj.GetComponent<Transform>();

        Transform target_tr = GameObject.Find("Target").GetComponent<Transform>();
        m_target_pos_for_score = target_tr.position;
        m_target_pos_for_score.z = 0f;

        m_camera = Camera.main;
        m_camera_tr = m_camera.GetComponent<Transform>();

        m_camera_target_pos = m_camera_normal_pos;
        m_camera_target_rot = m_camera_normal_rot;
        m_camera_target_size = m_camera_normal_size;
    }

    private void Start()
    {
        GS = GameState.IDLE;
    }

    private void Update()
    {
        switch (GS)
        {
            case GameState.IDLE:
                PrepareForAiming();
                break;

            case GameState.AIMING:
                if (Input.GetAxis("Fire1") == 1.0f)
                {
                    PullArrow();
                }
                break;

            case GameState.PULLING:
                if (Input.GetAxis("Fire1") == 0.0f)
                {
                    FireArrow();
                }
                break;

            case GameState.ZOOM_IN:
            case GameState.ZOOM_STAY:
            case GameState.ZOOM_OUT:
                ZoomCamera();
                break;
        }
    }

    void PrepareForAiming()
    {
        GS = GameState.AIMING;

        m_wind = Random.Range(-1f, 1f);
    }

    void PullArrow()
    {
        GS = GameState.PULLING;
    }

    void FireArrow()
    {
        GS = GameState.FLYING;

        Vector3 arrow_pos = Vector3.zero;
        Quaternion arrow_rot = Quaternion.identity;

        m_arrow.Fly(ARROW_SPEED);
    }

    public void HitTargetByArrow()
    {
        GS = GameState.ZOOM_IN;

        m_arrow.Hold();

        m_zoom_start_time = Time.time;
        m_camera_target_pos = m_camera_zoom_pos;
        m_camera_target_rot = m_camera_zoom_rot;
        m_camera_target_size = m_camera_zoom_size;
        m_camera_zoom_time = CAMERA_ZOOM_IN_TIME;

        m_score += CalculateScore();
        m_score_text.text = m_score.ToString();
    }

    public void Missed()
    {
        GS = GameState.IDLE;
    }

    void ZoomCamera()
    {
        if(Time.time < m_zoom_start_time + m_camera_zoom_time)
        {
            float t = (Time.time - m_zoom_start_time) / m_camera_zoom_time;

            m_camera_tr.position = Vector3.Lerp(m_camera_tr.position, m_camera_target_pos, t);
            m_camera_tr.rotation = Quaternion.Slerp(m_camera_tr.rotation, m_camera_target_rot, t);
            m_camera.orthographicSize = Mathf.Lerp(m_camera.orthographicSize, m_camera_target_size, t);
        }
        else
        {
            m_camera_tr.position = m_camera_target_pos;
            m_camera_tr.rotation = m_camera_target_rot;
            m_camera.orthographicSize = m_camera_target_size;

            switch(GS)
            {
                case GameState.ZOOM_IN:
                    GS = GameState.ZOOM_STAY;

                    m_zoom_start_time = Time.time;
                    m_camera_zoom_time = CAMERA_ZOOM_STAY_TIME;

                    break;

                case GameState.ZOOM_STAY:
                    GS = GameState.ZOOM_OUT;

                    m_zoom_start_time = Time.time;
                    m_camera_target_pos = m_camera_normal_pos;
                    m_camera_target_rot = m_camera_normal_rot;
                    m_camera_target_size = m_camera_normal_size;
                    m_camera_zoom_time = CAMERA_ZOOM_OUT_TIME;

                    break;

                case GameState.ZOOM_OUT:
                    GS = GameState.IDLE;
                    break;
            }
        }
    }

    public float GetWind()
    {
        return m_wind;
    }

    int CalculateScore()
    {
        int score = 0;

        Vector3 arrow_pos = m_arrow_tr.position;
        arrow_pos.z = 0f;

        float dist = Vector3.Distance(m_target_pos_for_score, arrow_pos);
        m_is_bulls_eye = dist < BULLS_EYE_RANGE;

        if(m_is_bulls_eye)
        {
            score = BULLS_EYE_SCORE;
        }
        else
        {
            score = (int)Mathf.Floor(dist * -MAX_SCORE + MAX_SCORE);
            if (score < MIN_SCORE) score = MIN_SCORE;
        }

        Debug.Log(score);

        return score;
    }

    public void GetLauncherInitialRotation(ref float x_angle, ref float y_angle)
    {
        //x_angle = -50.0f;
        //y_angle = 0f;
    }

    private static SceneManager s_instance;

    public static SceneManager Instance
    {
        get
        {
            return s_instance;
        }
    }
}

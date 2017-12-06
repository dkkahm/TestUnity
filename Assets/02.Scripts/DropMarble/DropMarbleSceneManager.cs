using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropMarbleGateState
{
    IDLE,
    LIFTING,
    NAVIGATING,
    MOVING,
}

public class DropMarbleSceneManager : MonoBehaviour {
    DropMarbleGateState m_gs = DropMarbleGateState.IDLE;

    private GameObject m_player_obj;
    private Transform m_player_tr;
    private Vector3 m_player_idle_pos;
    private Vector3 m_player_target_pos;

    private GameObject[] m_balls;
    private List<Vector3> m_ball_idle_pos = new List<Vector3>();

    private GameObject m_lifting_board;
    private GameObject m_navigating_board;

    private float PLAYER_LIFTING_X = 0f;
    private const float PLAYER_LIFTING_Y = 7.0f;
    private float PLAYER_LIFTING_Z = 0f;

    private const float PLAYER_LIFTING_SPEED = 5.0f;

    private const float MIN_PLAYER_NAVIGATING_X = -3f;
    private const float MAX_PLAYER_NAVIGATING_X = 3f;

    private const float MIN_PLAYER_NAVIGATING_Z = -3f;
    private const float MAX_PLAYER_NAVIGATING_Z = 3f;

    private const float PLAYER_NAVIGATING_SPEED = 5.0f;

    public static DropMarbleSceneManager s_instance = null;

    private int m_in_corder_ball_count = 0;

    public static DropMarbleSceneManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    private void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    void Start () {
        m_player_obj = GameObject.FindGameObjectWithTag("Player");
        m_player_tr = m_player_obj.GetComponent<Transform>();

        m_player_idle_pos = m_player_tr.position;
        PLAYER_LIFTING_X = m_player_idle_pos.x;
        PLAYER_LIFTING_Z = m_player_idle_pos.z;

        m_balls = GameObject.FindGameObjectsWithTag("BALL");
        foreach(GameObject ball in m_balls)
        {
            m_ball_idle_pos.Add(ball.GetComponent<Transform>().position);
        }

        StartIdle();
    }
	
	// Update is called once per frame
	void Update () {
        switch(m_gs)
        {
            case DropMarbleGateState.IDLE:
                Idle();
                break;

            case DropMarbleGateState.LIFTING:
                Lifting();
                break;

            case DropMarbleGateState.NAVIGATING:
                Navigating();
                break;

            case DropMarbleGateState.MOVING:
                Moving();
                break;

        }
	}

    void StartIdle()
    {
        m_gs = DropMarbleGateState.IDLE;

        m_player_obj.GetComponent<Rigidbody>().isKinematic = true;
        m_player_tr.position = m_player_idle_pos;

        for(int i = 0; i < m_balls.Length; ++ i)
        {
            m_balls[i].GetComponent<Transform>().position = m_ball_idle_pos[i];
            m_balls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_balls[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        m_in_corder_ball_count = 0;
    }

    void Idle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray touch_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Debug.DrawRay(touch_ray.origin, touch_ray.direction * 100.0f, Color.green);

            RaycastHit touch_hit;
            if(Physics.Raycast(touch_ray.origin, touch_ray.direction * 100.0f, out touch_hit))
            {
                if(touch_hit.collider.gameObject.tag == "Player")
                {
                    StartLift();                    
                }
            }
        }
    }

    void StartLift()
    {
        m_gs = DropMarbleGateState.LIFTING;
    }

    void Lifting()
    {
        if (Input.GetMouseButton(0))
        {
            m_player_target_pos = new Vector3(PLAYER_LIFTING_X, PLAYER_LIFTING_Y, PLAYER_LIFTING_Z);
            m_player_tr.position = Vector3.Lerp(m_player_tr.position, m_player_target_pos, PLAYER_LIFTING_SPEED * Time.deltaTime);
            if(m_player_tr.position.y >= PLAYER_LIFTING_Y - 0.01f)
            {
                m_player_tr.position = m_player_target_pos;
                StartNavigate();
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StartIdle();
        }
    }

    void StartNavigate()
    {
        m_gs = DropMarbleGateState.NAVIGATING;
    }

    void Navigating()
    {
        if (Input.GetMouseButton(0))
        {
            Ray touch_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit touch_hit;

            int layer_mask = 1 << 8;
            if (Physics.Raycast(touch_ray.origin, touch_ray.direction * 100.0f, out touch_hit, layer_mask))
            {
                // if (touch_hit.collider.gameObject.tag == "BOARD")
                {
                    m_player_target_pos = new Vector3(Mathf.Clamp(touch_hit.point.x, MIN_PLAYER_NAVIGATING_X, MAX_PLAYER_NAVIGATING_X),
                        PLAYER_LIFTING_Y,
                        Mathf.Clamp(touch_hit.point.z, MIN_PLAYER_NAVIGATING_Z, MAX_PLAYER_NAVIGATING_Z));

                    m_player_tr.position = Vector3.Lerp(m_player_tr.position, m_player_target_pos, PLAYER_NAVIGATING_SPEED * Time.deltaTime);
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StartMove();
        }
    }

    void StartMove()
    {
        m_gs = DropMarbleGateState.MOVING;

        // m_player_tr.position = new Vector3(0f, PLAYER_LIFTING_Y, 0f);

        m_player_obj.GetComponent<Rigidbody>().isKinematic = false;
    }

    void Moving()
    {
        if (Input.GetMouseButtonUp(0))
        {
            StartIdle();
        }
        else
        {

        }
    }

    public void OnBallInCorner()
    {
        ++m_in_corder_ball_count;

        if (m_in_corder_ball_count == m_balls.Length)
        {
            Debug.Log("All balls are in corner!");
        }
    }

    public void OnBallOutOfCorder()
    {
        -- m_in_corder_ball_count;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMarbleBall : MonoBehaviour {
    public Material m_green_material;
    public Material m_red_material;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "SENSOR")
        {
            GetComponent<Renderer>().material = m_green_material;

            DropMarbleSceneManager.Instance.OnBallInCorner();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SENSOR")
        {
            GetComponent<Renderer>().material = m_red_material;
            DropMarbleSceneManager.Instance.OnBallOutOfCorder();
        }
    }
}

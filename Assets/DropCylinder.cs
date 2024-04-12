using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class DropCylinder : MonoBehaviour {

    public GameObject obstacle;
    //public GameObject testobj;
    GameObject[] agents;

    void Start() {

        agents = GameObject.FindGameObjectsWithTag("agent");
        obstacle = null;
        try
        {
            obstacle = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Box.prefab", typeof(GameObject));
        }
        catch(Exception ex)
        {
            var stst = ex.Message;
        }

    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {

                Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                foreach (GameObject a in agents) {

                    a.GetComponent<AIControl>().DetectNewObstacle(hitInfo.point);
                }
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIControl : MonoBehaviour {

    GameObject[] goalLocations;
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectionRadius = 20;
    float fleeRadius = 10;
    float start_timer;
    Vector3 cameraPosition;
    bool firstLine = true;

    Dictionary<string, GameObject> objList;
    List<List<string>> writeStringList;
    CSVFileController csvController;

    StreamWriter writer = null;
    void ResetAgent() {

        speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120.0f;
        anim.SetFloat("speedMult", speedMult);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position) {

        if (Vector3.Distance(position, this.transform.position) < detectionRadius) {

            Vector3 fleeDirection = (this.transform.position - position).normalized;
            Vector3 newgoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newgoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid) {

                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    // Use this for initialization
    void Start() {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0.0f, 1.0f));
        ResetAgent();

        writer = new StreamWriter($"test/{System.DateTime.Now.ToString("yyMMdd-HHmmss")}-test.csv", true);

        writeStringList = new List<List<string>>();
        start_timer = Time.time;
        cameraPosition = GameObject.Find("Camera").transform.position;
        var viewVec = GameObject.Find("Camera").transform.forward;
        objList = new Dictionary<string, GameObject>();
        objList.Add("Head", GameObject.Find("Crowd/Male_1/Eyes/HeadSphere"));
        objList.Add("Neck", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/NeckSphere"));
        objList.Add("LeftShoulder", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/LeftShoulderSphere"));
        objList.Add("LeftElbow", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/LeftArmSphere"));
        objList.Add("LeftHand", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/LeftHandSphere"));
        objList.Add("LeftHip", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:LeftUpLeg/LeftHipSphere"));
        objList.Add("LeftKnee", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/LeftKneeSphere"));
        objList.Add("LeftToe", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot/LeftToeSphere"));
        objList.Add("RightShoulder", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/RightShoulderSphere"));
        objList.Add("RightElbow", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/RightArmSphere"));
        objList.Add("RightHand", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/RightHandSphere"));
        objList.Add("RightHip", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:RightUpLeg/RightHipSphere"));
        objList.Add("RightKnee", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/RightKneeSphere"));
        objList.Add("RightToe", GameObject.Find("Crowd/Male_1/mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot/RightToeSphere"));


        StreamWriter writerCamera = new StreamWriter($"test/CameraData.txt", true);
        writerCamera.WriteLine($"Camera Pos : {cameraPosition.x} {cameraPosition.y} {cameraPosition.z}");
        writerCamera.WriteLine($"Camera view : {viewVec.x} {viewVec.y} {viewVec.z}");
        writerCamera.Close();
    }

    // Update is called once per frame
    void Update() {
        if (agent.remainingDistance < 1) {

            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
        printObjectCoord();
    }

    void printObjectCoord()
    {
        if (objList.Count <= 0)
            return;
        if(firstLine)
        {
            List<string> headers = new List<string>();
            headers.Add("Time");
            foreach(var obj in objList)
            {
                headers.Add(obj.Key);
                headers.Add("");
                headers.Add("");
            }

            //headers.AddRange(objList.Select(x => x.Key).ToList());
            writer.WriteLine(string.Join(",", headers));
            firstLine = false;
        }

        List<string> datas = new List<string>();
        datas.Add((Time.time - start_timer).ToString("N4").Replace(",", ""));
        foreach (var obj in objList)
        {
            try
            {
                datas.Add(((double)cameraPosition.x * 1000.0 - (double)obj.Value.transform.position.x * 1000.0).ToString("N8").Replace(",", ""));
                datas.Add(((double)cameraPosition.y * 1000.0 - (double)obj.Value.transform.position.y * 1000.0).ToString("N8").Replace(",", ""));
                datas.Add(((double)cameraPosition.z * 1000.0 - (double)obj.Value.transform.position.z * 1000.0).ToString("N8").Replace(",", ""));
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            
            //Vector3 pos = new Vector3(((double)cameraPosition.x * 1000.0 - (double)obj.Value.transform.position.x * 1000.0),
            //    (cameraPosition.y * 1000.0 - obj.Value.transform.position.y * 1000.0),
            //    (cameraPosition.z * 1000.0 - obj.Value.transform.position.z * 1000.0));
            //datas.Add((pos.x/1000.0 * 1000.0).ToString("N8"));
            //datas.Add((pos.y/1000.0*1000.0).ToString("N8"));
            //datas.Add((pos.z/1000.0*1000.0).ToString("N8"));
        }
        writer.WriteLine(string.Join(",", datas));
    }
}

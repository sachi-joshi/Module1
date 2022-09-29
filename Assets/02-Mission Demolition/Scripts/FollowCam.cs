using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    static public GameObject POI; //point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;


    void Awake()
    {
        camZ = this.transform.position.z;

    }

    void FixedUpdate()
    {
        //if(POI==null){
        //    return;
        //}


        ////Get POI position
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        if(POI==null){
            destination = Vector3.zero;

        }
        else{
            //get the position of the poi
            destination = POI.transform.position;
            //if poi is a profile check to see if it's at rest
            if(POI.tag =="Projectile"){
                //if is is sleeping(not moving)
                if(POI.GetComponent<Rigidbody>().IsSleeping()){
                    POI = null;
                    return;
                }
            }
        }

        //Limit X & Y to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        //force destination.z to be camZ to keep camera far enough away
        destination.z = camZ;

        //set the camera to the destination
        transform.position = destination;

        //Set the orthographicSize of the camera to keep ground in view
        Camera.main.orthographicSize = destination.y + 10;



    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

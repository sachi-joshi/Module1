using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;//Singleton

    [Header("Set In Inspector")]
    public float minDist = .1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this;
        //get a reference to the LineRenderer
        line = GetComponent<LineRenderer>();
        //Disable the LineRenderer until it's needed
        line.enabled = false;
        //Initialize the points list
        points = new List<Vector3>();

    }

    //This is a property(that is, a method masquerading as a field)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //When _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //this can be used to clear the line directly 
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();

    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }

        if (points.Count == 0)
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            //add an extra bit of line ot aid aiming later
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //Sets the first two points;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //enables the linerenderer
            line.enabled = true;


        }
        else
        {
            //normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }

    }

    public Vector3 lastPoint{
        get{
            if(points==null){
                //if there are no points returns Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }


    void FixedUpdate()
    {
        if(poi == null){
            //if there is no poi, search for one
            if(FollowCam.POI != null){
                if(FollowCam.POI.tag == "Projectile"){
                    poi = FollowCam.POI;

                }
                else{
                    return;//we didnt find a poi
                }
            }
            else{
                return;//we didnt find a poi
            }
        }

        //if there is a poi, its loc is added every FixedUpdate
        AddPoint();
        if(FollowCam.POI == null){
            //once FollowCam.POI is null, make the local poi null too
            poi = null;
        }

    
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
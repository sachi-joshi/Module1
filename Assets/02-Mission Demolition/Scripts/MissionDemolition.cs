using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode
{
    idle,
    playing,
    levelEnd

}

public class MissionDemolition : MonoBehaviour {

    static private MissionDemolition S; //private Singleton

    [Header("Set In Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;


    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";


	// Use this for initialization
	void Start () {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();

	}

    void StartLevel(){
        //Get rid of old castle if one exists
        if(castle != null){
            Destroy(castle);

        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");

        foreach(GameObject pTemp in gos){
            Destroy(pTemp);
        }

        //Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //Reset the goal 
        Goal.goalMet = false;
  
        UpdateGUI();

        mode = GameMode.playing;

    }

    void UpdateGUI(){
        //Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of" + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;

    }


	
	// Update is called once per frame
	void Update () {
        UpdateGUI();

        //Check for level and
        if((mode==GameMode.playing) && Goal.goalMet){
            //Change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Show Both");
            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);

        }
	}

    void NextLevel(){
        level++;
        if(level==levelMax){
            level = 0;
            
        }
        StartLevel();

    }

    public void SwitchView(string eView=""){
        if(eView == "" ){
            eView = uitButton.text;

        }

        showing = eView;

        switch(showing){
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;

        }
    }


    //static method that allows code anywhere to increment shotsTaken
    public static void ShotFired(){
        S.shotsTaken++;
    }
}



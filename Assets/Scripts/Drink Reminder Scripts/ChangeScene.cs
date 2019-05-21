using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    public static int page_now;
    public static bool singlemode = false;

	// Use this for initialization
	void Start () {

        singlemode = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changeForward()
    {
        	page_now = 14;
        	Application.LoadLevel("CreateProfile_EnterGender");
    }

    public void changetoMenue()
    {
        page_now = 1;
        Application.LoadLevel("Menue");

    }

    public void changeBack()
    {
        if (Application.loadedLevel == 21)
        {
            //Debug.Log(SOS_Controller.current_page);
            Application.LoadLevel(SOS_Controller.current_page);
        }
		else if(Application.loadedLevel == 19){
			Application.LoadLevel("Menue");
			GameController.login = false;
            SimpleTest.previousWeight = 0.0f;
            SimpleTest.updateUser = true;
		}
        else if(Application.loadedLevel == 4)
        {
            page_now = 19;
            Application.LoadLevel("Welcome");
        }
		else if (Application.loadedLevel == 14 || Application.loadedLevel == 15 || Application.loadedLevel == 16 || Application.loadedLevel == 17)
            Application.LoadLevel("Menue");
        else
            Application.LoadLevel(page_now - 1);
    }

    public void changeToRegister()
    {
        page_now = 15;
        Application.LoadLevel("CreateProfile_EnterName");
    }

    public void changeToLoad()
    {
        page_now = 16;
        Application.LoadLevel("LoadProfile");
    }

    public void startFastGame()
    {
        page_now = 17;
        Application.LoadLevel("FastGame");
    }

    public void startMultiGame()
    {
		//if(GameController.login){
		//	MyNavigation.currentPage = 4;
  //          page_now = 4;
		//	Application.LoadLevel("Manue-Layout");
		//}
		//else{
  //      page_now = 7;
  //      MyNavigation.pictureLayout = true;
  //      MyNavigation.size = 6;
  //      Application.LoadLevel("Memory");
		//}
    }

    public void startSingleGame()
    {
        //singlemode = true;

        //if (GameController.login)
        //{
        //    MyNavigation.currentPage = 4;
        //    Application.LoadLevel("Manue-Layout");
        //}
        //else {
        //    page_now = 7;
        //    MyNavigation.pictureLayout = true;
        //    MyNavigation.size = 6;
        //    Application.LoadLevel("Memory");
        //}
    }

    public void startBalloon(){
		page_now = 18;
		Application.LoadLevel("Balloon");
	}

	public void startAdventure(){

		//GameController.loadProfil(

		page_now = 19;
		Application.LoadLevel("MatchingPuzzle");
	}

    public void checkConnection()
    {
        page_now = 13;
        Application.LoadLevel("BluetoothTest");
    }

    public void crossword()
    {
        page_now = 25;
        Application.LoadLevel("main_menu");
    }

    public void balloon()
    {
        page_now = 1;
        Application.LoadLevel("Menue");
    }
}

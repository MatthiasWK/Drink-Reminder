using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    Actor tmp_actor = new Actor();
    public Actor[] all_actors = new Actor[100];
    GameInformation actorInformation = new GameInformation();
    int size = 0;
    public static string tmp_Name;
    public static int tmp_Gender;
	public static int tmp_score;
    public static float tmp_weight;
    public Button profil;
    public Canvas my_canvas;
    public GameObject weight_text;
    public static bool userCreated = false;
    private static string path = string.Empty;
    Color kindOfBlue = new Color(114/255F, 192/255F, 255/255F, 0.6F);
    Color kindOfRed = new Color(255 / 255F, 114 / 255F, 152 / 255F, 0.6F);

	public static bool login, firstLogin;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
            path = System.IO.Path.Combine(Application.persistentDataPath, "actors.xml");
        else
            path = System.IO.Path.Combine(Application.dataPath, "actors.xml");
    }

    void Start()
    {
        firstLogin = false;
		if(Application.loadedLevel == 18)
		{
		}
		else{
        if (path == null)
            System.IO.File.Create(path);

        UpdateSize();
        if (size == 0)
        {
            Actor tmp = new Actor();
            tmp.id = 000;
            tmp.name = "reserved";
            tmp.gender = 0;
            all_actors[0] = tmp;
        }
		}
    }

    public void SaveMyData()
    {
        tmp_weight = float.Parse(weight_text.GetComponent<Text>().text);

        //increase size +1 entry
        size = size +1;

        //save all values in tmp_actor
        all_actors[size] = setActorValuesToSave();

        //create and add values to List that will be saved in separate file "actors.xml"
        ActorContainer actorCollection = new ActorContainer();

        //actorCollection.actors.Add(all_actors[size]);
        foreach (Actor loop_actor in all_actors)
        {
            actorCollection.actors.Add(loop_actor);
        }

        actorCollection.Save(path);
        Debug.Log("new Entry was successfull saved!");
        userCreated = true;
    }

    public void UpdateSize()
    {
        //load separate file "actors.xml" and read the values
        var actorCollection = ActorContainer.Load(path);

        //update current size of all_actor array
        size = actorCollection.actors.Count;

        int i = 0;
        foreach (Actor actor in actorCollection.actors)
        {
            //set Values
            actorInformation.setName(actor.name);
            actorInformation.setGender(actor.gender);
            actorInformation.setID(actor.id);
			actorInformation.setScore(actor.score);

            Actor tmp = new Actor();
            tmp.name = actorInformation.getName();
            tmp.id = actorInformation.getID();
            tmp.gender = actorInformation.getGender();
			tmp.score = actorInformation.getScore();

            all_actors[i] = tmp;
            if(Application.loadedLevelName == "LoadProfile" && i > 0)
            {
                Button test = Instantiate(profil);
				profil.name = tmp.name;
                test.transform.parent = my_canvas.transform;
                test.image.rectTransform.localScale = new Vector3(1 , 1 , 1);
                test.image.rectTransform.sizeDelta = new Vector2(150, 75);
                if (i > 4)
                {
                    test.image.rectTransform.localPosition = new Vector3(-250 + 170, 150 - ((i - 1 - 4) * 85), 0);
                    if (tmp.gender == 1)
                        test.image.color = kindOfBlue;
                    else if (tmp.gender == 2)
                        test.image.color = kindOfRed;
                }
                else {
                    test.image.rectTransform.localPosition = new Vector3(-250, 150 - ((i - 1) * 85), 0);
                    if (tmp.gender == 1)
                        test.image.color = kindOfBlue;
                    else if (tmp.gender == 2)
                        test.image.color = kindOfRed;
                }

                

                test.GetComponentInChildren<Text>().text = tmp.name;
            }
            i++;
        }
        //need to be decreased cause will be increased in the next save step 
        if (i > 0)
            size--;

    }

    public void LoadMyData()
    {
        //load separate file "actors.xml" and read the values
        var actorCollection = ActorContainer.Load(path);

        //set the dynamic digital values to the values in the file
        foreach (Actor actor in actorCollection.actors)
        {
            //set Values
            actorInformation.setName(actor.name);
            actorInformation.setGender(actor.gender);
            actorInformation.setID(actor.id);
			actorInformation.setScore(actor.score);


            tmp_actor.name = actorInformation.getName();
            tmp_actor.gender = actorInformation.getGender();
            tmp_actor.id = actorInformation.getID();
			tmp_actor.score = actorInformation.getScore();
        }

        Debug.Log("Geladene ID: " + tmp_actor.id);
        Debug.Log("Geladener Name: " + tmp_actor.name);
        Debug.Log("Geladener Gender: " + tmp_actor.gender);
		Debug.Log("Geladener Score: " + tmp_actor.score);


    }


    private Actor setActorValuesToSave()
    {
        //set the value that need to be saved
        Actor new_actor = new Actor();

        new_actor.id = size;
        //Debug.Log("Gespeicherte ID: " + tmp_actor.id);

        new_actor.name = tmp_Name;
        //Debug.Log("Gespeicherter Name: " + tmp_actor.name);

        tmp_Gender = actorInformation.getGender();
        new_actor.gender = actorInformation.getGender();
        //Debug.Log("Gespeicherter Gender: " + tmp_actor.gender);

		new_actor.score = 0;

        return new_actor;
    }

    public void saveName()
    {
        if (SceneManager.GetActiveScene().name == "CreateProfile_EnterName")
        {
            if (!CheckTaken(actorInformation.getName()))
            {
                tmp_Name = actorInformation.getName();
                SceneChanger.LoadInScript("CreateProfile_EnterGender");
            }
            else
            {
                my_canvas.gameObject.SetActive(true);
            }
        }
        else
        {
            tmp_Name = actorInformation.getName();
        }


    }

    private bool CheckTaken(string new_Name)
    {
        foreach (Actor loop_actor in all_actors)
        {
            if(loop_actor == null)
            {
                return false;
            }
            else if (loop_actor.name == new_Name)
            {
                return true;
            }
        }
        return false;
    }

	public void saveScore(){
		tmp_score = actorInformation.getScore();
	}


	public void updateScore(string input){
		
		//load separate file "actors.xml" and read the values
		var actorCollection = ActorContainer.Load(path);

		//set the dynamic digital values to the values in the file
		foreach (Actor actor in actorCollection.actors)
		{
			if(actor.name.Equals(input)){
				actor.score = actorInformation.getScore();
			}
		}
		actorCollection.Save(path);

	}

	public void loadProfil(string name)
	{
		//load separate file "actors.xml" and read the values
		var actorCollection = ActorContainer.Load(path);

		//set the dynamic digital values to the values in the file
		foreach (Actor actor in actorCollection.actors)
		{
			if(name.Equals(actor.name)){

			//set Values
			actorInformation.setName(actor.name);
			actorInformation.setGender(actor.gender);
			actorInformation.setID(actor.id);
			actorInformation.setScore(actor.score);


			tmp_actor.name = actorInformation.getName();
			tmp_actor.gender = actorInformation.getGender();
			tmp_actor.id = actorInformation.getID();
			tmp_actor.score = actorInformation.getScore();
			}
		}

		Debug.Log("Geladene ID: " + tmp_actor.id);
		Debug.Log("Geladener Name: " + tmp_actor.name);
		Debug.Log("Geladener Gender: " + tmp_actor.gender);
		Debug.Log("Geladener Score: " + tmp_actor.score);

		login = true;
        SimpleTest.previousWeight = 0.0f;
        firstLogin = true;
	}

}

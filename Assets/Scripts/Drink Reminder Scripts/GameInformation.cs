using UnityEngine;
using System.Collections;

public class GameInformation : MonoBehaviour {

	private static string name;
    private static int gender;
    private static int id;
	private static int score;

	//setter methode of Name
	public void setName(string input){
        name = input;
	}

	//getter methode of Name
	public string getName(){
		return name;
	}

    public void setGender(int input)
    {
        gender = input;
    }

	public void setScore(int input){
		score = input;
	}

    //getter methode of Name
    public int getGender()
    {
        return gender;
    }


    public void setID(int input)
    {
        id = input;
    }

    //getter methode of Name
    public int getID()
    {
        return id;
    }

	public int getScore(){
		return score;
	}
		

}

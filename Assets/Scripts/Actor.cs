using UnityEngine;
using System.Collections;
using System.Xml.Serialization;


public class Actor
{
    [XmlAttribute("ID")]
    public int id;

    [XmlElement("Name")]
    public string name;

    [XmlElement("Gender")]
    public int gender; //0 == noEntry, 1 == male; 2 == female

	[XmlElement("Score")]
	public int score;
}

/*
public class Actor : MonoBehaviour {

    public ActorData data = new ActorData();

    public int id = 000;

    public string name = "actor";

    public int gender = -1; //0 == male; 1 == female

    public void StoreData()
    {
        GameInformation blubaa = new GameInformation();
        data.id = data.id + 1;
        data.name = name;
        //data.name = blubaa.getName();
        data.gender = gender ;
    }

    public void LoadData()
    {
        GameInformation blubaa = new GameInformation();
        blubaa.setName(data.name);
        Debug.Log("so heiße ich: " + data.name);
        id = data.id;
        gender = data.gender;
    }
}

public class ActorData
{
    [XmlAttribute("ID")]
    public int id;

    [XmlElement("Name")]
    public string name;

    [XmlElement("Gender")]
    public int gender; //0 == male; 1 == female
}
*/

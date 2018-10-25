using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;

[XmlRoot("ActorCollection")]
public class ActorContainer
{

    [XmlArray("Actors"), XmlArrayItem("Actor")]
    public List<Actor> actors = new List<Actor>();
    //public List<ActorData> actors = new List<ActorData>();

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ActorContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static ActorContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ActorContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ActorContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ActorContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ActorContainer));
        return serializer.Deserialize(new StringReader(text)) as ActorContainer;
    }

}


    
 
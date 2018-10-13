using UnityEngine;
using System.Collections;

public class BroadcastReceiver : MonoBehaviour {

    AndroidJavaClass jc;
    string javaMessage = "";
    public static bool hasDrunk = false;
    public static bool connected = false;
    public static string currentDrunkTime;
    System.DateTime currentDrinkDate;
    public static int estimatedFillStatus;
    public static int fillStatusDifference;
    string rawValue;

    void Start() {
        // Acces the android java receiver we made
        //jc = new AndroidJavaClass("de.tum.far.receiver.MyReceiver");
        // We call our java class function to create our MyReceiver java object
        //jc.CallStatic("createInstance");
    }

    void Update() {
        // We get the text property of our receiver
        //splitMessage();
        //if (javaMessage == "true" && this.gameObject.GetComponent<timer_controller>().blocked)
          //  this.gameObject.GetComponent<timer_controller>().unTrigger();
        //Debug.Log(javaMessage);
        //GetComponent<TextMesh>().text = javaMessage;
    }

    void splitMessage()
    {
        javaMessage = jc.GetStatic<string>("text");
        //Debug.Log(javaMessage);
        string[] results;
        results = javaMessage.Split('#');
        if (results[0].Substring(13).Equals("false")) //hasGetrunken=true
            hasDrunk = false;
        else
            hasDrunk = true;
        currentDrunkTime = results[1].Substring(12); //currentTime=16/08/2017 10:10:46

        string formatString = "dd'/'MM'/'yyyy' 'HH':'mm':'ss";
        string sampleData = results[1].Substring(12);
        currentDrinkDate = System.DateTime.ParseExact(sampleData,formatString, null);

        if (currentDrinkDate.AddMinutes(1) >= System.DateTime.Now)
            connected = true;

        estimatedFillStatus = int.Parse(results[2].Substring(25)); //estimatedFillStatusInMl=0010
        fillStatusDifference = int.Parse(results[3].Substring(26)); //fillStatusDifferenceInMl=0440
        rawValue = results[4].Substring(9); //rawValue=0567
        //GetComponent<TextMesh>().text = connected + "\n" + "hat getrunken: " + hasDrunk + "\n" + "am: " + currentDrunkTime + "\n" + "es befinden sich derzeit ca: " + estimatedFillStatus +
            //"\n" + "getrunken worden ist ca: " + fillStatusDifference + "\n" + "raw: " + rawValue;
    }
}

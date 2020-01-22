using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mono.Data.Sqlite;
using System.Data;
using System;
using System.IO;

public class db : MonoBehaviour {

    string conn, connString;
    string dbName = "Users.s3db";
    string sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    public GameObject nameObject, surnameObject; //test

    private float currentAmount = 0.0f;


    // Use this for initialization
    void Start () {

        //FOR PC PATH!
        //conn = "URI=file:" + Application.dataPath + "/Plugins/Users.s3db"; //Path to database.

        //DON'T DELETE THIS IS THE PATH FOR ANDROID!



        string filepath = Application.persistentDataPath + "/" + dbName;

        if (!File.Exists(filepath))

        {

            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +

            Application.dataPath + "!/assets/" + dbName);

            // if it doesn't ->

            // open StreamingAssets directory and load the db ->

            string sourcepath = System.IO.Path.Combine(Application.streamingAssetsPath, dbName);

            if (sourcepath.Contains("://"))
            {
                Debug.Log("Copy Database from StreamingAsset Folder: " + sourcepath);
                WWW www = new WWW(sourcepath);
                while (!www.isDone) {; }

                if (string.IsNullOrEmpty(www.error))
                {
                    System.IO.File.WriteAllBytes(filepath, www.bytes);
                }
            }

        }

    


        //string connectionString = @"Data Source=C:\Temp\Test.db3;Pooling=true;FailIfMissing=false;Version=3";

        //ADDITIONAL FOR THE ANDROID PATH!

        connString = "Data Source=" + filepath + ";Version=3;";
        dbconn = (IDbConnection)new SqliteConnection(connString);

        //FOR PC PATH!
        //dbconn = (IDbConnection)new SqliteConnection(conn);


        dbconn.Open(); //Open connection to the database.
         dbcmd = dbconn.CreateCommand();
         sqlQuery = "SELECT * " + "FROM Usersinfo";// table name
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string gender = reader.GetString(2);
            float weight = reader.GetFloat(3);


            Debug.Log("value= " + id + "  name =" + name + "  gender =" + gender + "  weight =" + weight);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

	}
	
	// Update is called once per frame
	void Update () {

        if (GameController.userCreated)
        {
            GameController.userCreated = false;
            insertUserdata();
        }

//        if (SimpleTest.updateWeight)
//        {
//            BroadcastReceiver.hasDrunk = true;
//            if (SimpleTest.previousWeight == 0.0f || ((SimpleTest.previousWeight + 50.0f) - float.Parse(SimpleTest.bluetoothMessage_weight) < 0))
//            {
//                SimpleTest.previousWeight = float.Parse(SimpleTest.bluetoothMessage_weight);
//                SimpleTest.updateWeight = false;
//            }
//            else
//            {
//                SimpleTest.updateWeight = false;
//                currentAmount = SimpleTest.previousWeight - float.Parse(SimpleTest.bluetoothMessage_weight);
//                insertDrinkdata();
//            }
//        }


    }

    public void insertUserdata()
    {
        using (dbconn = new SqliteConnection(connString)) //ANDROID change conn into connString PC: conn
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            string name = GameController.tmp_Name;
            string gender;
            if (GameController.tmp_Gender == 0)
                gender = "male";
            else
                gender = "female";
            //string name = nameObject.GetComponentInChildren<Text>().text;
            //string gender = surnameObject.GetComponentInChildren<Text>().text;
            float weight = GameController.tmp_weight;
            sqlQuery = string.Format("insert into Usersinfo (Name, Gender, Weight) values (\"{0}\",\"{1}\",\"{2}\")", name, gender, weight);
            Debug.Log(name + " " + gender + " " + weight + " were successfully saved into the database!");
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
    }

    public void insertDrinkdata()
    {
        using (dbconn = new SqliteConnection(connString)) //ANDROID change conn into connString PC: conn
        {
            bool currentUser = false;
            int currentUserID = 100;

            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            sqlQuery = "SELECT * " + "FROM Usersinfo";// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                currentUserID = reader.GetInt32(0);
                string userName = reader.GetString(1);
                if (userName.ToLower() == GameController.tmp_Name.ToLower()) {
                    currentUser = true;
                    break;
                }
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;

            if (currentUser)
            {
                using (dbconn = new SqliteConnection(connString)) //ANDROID change conn into connString
                {
                    currentUser = false;
                    dbconn.Open(); //Open connection to the database.
                    dbcmd = dbconn.CreateCommand();
                    string typeOfDrink = "Wasser";
                    float amount = currentAmount;
                    DateTime theTime = DateTime.Now;
                    string datetime = theTime.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
                    sqlQuery = string.Format("insert into Drinkinfo (UserID, TypeOfDrink, Time, Amount) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\")", currentUserID, typeOfDrink, datetime, amount);
                    Debug.Log(currentUserID + " " + typeOfDrink + " " + DateTime.Now + " " + amount + " were successfully saved into the database!");
                    dbcmd.CommandText = sqlQuery;
                    dbcmd.ExecuteScalar();
                    dbconn.Close();
                }
            } else
                Debug.Log("Could not save into Database, cause User wasn't found!");
        }
    }
}

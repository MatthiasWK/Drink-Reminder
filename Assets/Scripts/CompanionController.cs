using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Script that controls what the companion says. 
// Speech is initiatet in other scripts with public functions and then companion either says specific line or cycles through certain lines randomly depending on curent screen.
public class CompanionController : MonoBehaviour {
    GameObject Timer;
    public Text CompanionText;
    public Image CompanionBubble;
    readonly string[] Compliments = { "Super!", "Sehr Gut!", "Gut Gemacht!", "Wow!", "Spitze!" };
    readonly string[] MenuQuotes = { "Drücke 'Spielen' um das Spiel zu starten!",
                                     "Hier kannst du auch die Größe des Spiels ändern",
                                     "Unter 'Optionen' kannst du das Spiel anpassen",
                                     "Falls du Hilfe brauchst, drücke auf den 'Hilfe'-Knopf",
                                     "Drücke 'Abmelden' um den Spieler zu ändern" };
    readonly string[] SettingsQuotes = { "Such dir neue Hntergrundbilder aus",
                                         "Du kannst eigene Bilder für das Spiel verwenden",
                                         "Versuche mal die Objekte anzupassen",
                                         "Falls du Hilfe brauchst, drücke auf den 'Hilfe'-Knopf" };
    readonly string[] CustomizationQuotes = { "Mit 'Bild wählen' kannst du das Bild ändern",
                                              "Bewege die Form um das Objekt anzupassen",
                                              "Drücke 'Speichern' um die Objekte zu speichern",
                                              "Falls du Hilfe brauchst, drücke auf den 'Hilfe'-Knopf" };

    void Start () {
        Timer = GameObject.Find("Timer and Bluetooth");
        if(Timer != null)
            GetComponent<Slider>().maxValue = Timer.GetComponent<timer_controller>().baseTime;

        if (Application.loadedLevelName == "Menue")
            StartCoroutine("SayMenue");
    }
	
	// Update Slider value once per frame
	void Update () {

        if(Timer != null)
            GetComponent<Slider>().value = Timer.GetComponent<timer_controller>().remainder;
        
	}

    /// <summary>
    /// Compliment player
    /// </summary>
    public void SayGreat()
    {
        StopAllCoroutines();
        string s = Compliments[Random.Range(0, Compliments.Length)];
        StartCoroutine(Say(s, 2));
        StartCoroutine(SayGame());
    }

    /// <summary>
    /// Called from other scripts to initiate companion dialog
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    public void StartSay(string text, int time)
    {
        StopAllCoroutines();
        StartCoroutine(Say(text, time));
    }

    /// <summary>
    /// Called from other scripts to toggle speech bubble
    /// </summary>
    /// <param name="s"></param>
    public void ToggleSpeech(bool s)
    {
        CompanionBubble.gameObject.SetActive(s);
        CompanionText.gameObject.SetActive(s);
    }

    /// <summary>
    /// Has Companion say custom line
    /// </summary>
    /// <param name="text"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator Say(string text, int time)
    {
        ToggleSpeech(true);
        CompanionText.text = text;
        yield return new WaitForSeconds(time);
        ToggleSpeech(false);
    }

    private IEnumerator SayMenue()
    {
        StartCoroutine(Say("Wilkommen!", 5));
        yield return new WaitForSeconds(6);
        StartCoroutine(Say("Wähle ein Profil oder erstelle ein neues", 30));
    }
    
    public void StartSayGame()
    {
        StopAllCoroutines();
        StartCoroutine(Say("Viel Spaß!", 5));
        StartCoroutine(SayGame());
    }

    private IEnumerator SayGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            StartCoroutine(Say("Verbinde die markierten Objekte!", 10));
            yield return new WaitForSeconds(20);
            StartCoroutine(Say("Falls du Hilfe brauchst, drücke auf den 'Hilfe'-Knopf", 10));
        }
        
    }

    public void StartSayGameMenu()
    {
        StopAllCoroutines();
        StartCoroutine(Say("Willkommen im Spiele-Menü", 5));
        StartCoroutine(SayGameMenu());
    }

    private IEnumerator SayGameMenu()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            string s = MenuQuotes[Random.Range(0, MenuQuotes.Length)];
            StartCoroutine(Say(s, 5));
        }

    }

    public void StartSaySettings()
    {
        StopAllCoroutines();
        StartCoroutine(Say("Hier kannst du das Aussehen des Spiels ändern", 5));
        StartCoroutine(SaySettings());
    }

    private IEnumerator SaySettings()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            string s = SettingsQuotes[Random.Range(0, SettingsQuotes.Length)];
            StartCoroutine(Say(s, 5));
        }

    }

    public void StartSayCustomization()
    {
        StopAllCoroutines();
        StartCoroutine(Say("Wähle ein eigenes Bild aus!", 5));
        StartCoroutine(SayCustomization());
    }

    private IEnumerator SayCustomization()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            string s = SettingsQuotes[Random.Range(0, SettingsQuotes.Length)];
            StartCoroutine(Say(s, 5));
        }

    }
}

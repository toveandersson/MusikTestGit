using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.PlayerSettings;

public class Logic : MonoBehaviour
{
    SoundLogic soundLogic;
    public GameObject buttonPrefab;

    public Toggle durToggle;
    public Toggle mollToggle;
    public bool dur;
    public bool moll;

    public TextMeshProUGUI startText;
    public TextMeshProUGUI title;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI answerText;

    public string answer;
    int num = 1;
    public bool ool = true;

    public class Content
    {
        public Dictionary<string, string> dur = new Dictionary<string, string>();
        public Dictionary<string, string> moll = new Dictionary<string, string>();
        public Dictionary<string, string> all = new Dictionary<string, string>();
    }

    private Dictionary<string, string> mollparallellDict = new Dictionary<string, string> {
        { "C", "Am" }, { "G", "Em" }, { "D", "Bm" }, { "A", "F#m" }, { "E", "C#m" }, { "B", "G#m" }, { "F#", "D#m" }, { "Gb", "Ebm" }, { "Db", "Bbm" }, { "Ab", "Fm" }, { "Eb", "Cm" }, { "Bb", "Gm" }, { "F", "Dm" } };

    private Dictionary<string, string> spegelIntervallDict = new Dictionary<string, string> {
        { "L2", "S7" }, { "S2", "L7" }, { "L3", "S6" }, { "S3", "L6/S5" }, { "R4", "R5" }, { "S4/L5", "S4/L5" }, { "R5", "R4" }, { "S5/L6", "S3" }, { "S6", "L3" }, { "L7", "S2" }, { "S7", "L2" }};

    private string[] circleOfFifths = { "C", "G", "D", "A", "E", "B", "F#/Gb", "C#/Db", "G#/Ab", "D#/Eb", "A#/Bb", "F" };

    public Content currentButton = new Content();
    public Dictionary<string, string> currentDictionary;

    Content kvinter;
    Content förtecken;
    Content mollparalleller;
    Content spegelIntervall;

    public class Buttons
    {
        public Content kvinter = new Content();
        public Content förtecken = new Content();
        public Content mollparalleller = new Content();
        public Content ljud = new Content();
        public Content spegelIntervall = new Content();
    }


    void Start()
    {
        kvinter = new Content();
        kvinter.dur = new Dictionary<string, string>{
        { "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" }};
        kvinter.moll = new Dictionary<string, string>{
        { "Am", "Em" }, { "Em", "Bm" }, { "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};
        kvinter.all = new Dictionary<string, string>{
        { "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" },{ "Am", "Em" }, { "Em", "Bm" }, 
        { "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};

        förtecken = new Content();
        förtecken.dur = new Dictionary<string, string>{
        { "C", "0" },{ "G", "1" },{ "D", "2" },{ "A", "3" },{ "E", "4" },{ "B", "5" },{ "F#", "6" },{ "Gb", "6" },{ "Db", "5" },{ "Ab", "4" },{ "Eb", "3" },{ "Bb", "2" },{ "F", "1" }};
        förtecken.moll = new Dictionary<string, string> {
        { "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};
        förtecken.all = new Dictionary<string, string>
        {{ "C", "0" },{ "G", "1" },{ "D", "2" },{ "A", "3" },{ "E", "4" },{ "B", "5" },{ "F#", "6" },{ "Gb", "6" },{ "Db", "5" },{ "Ab", "4" },{ "Eb", "3" },{ "Bb", "2" },{ "F", "1" },
        { "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};

        mollparalleller = new Content();
        spegelIntervall = new Content();
        ool = true;

        CreateButton(new Vector3(-2.5f, 4.5f, 0), "Kvinter", kvinter, kvinter.all);
        CreateButton(new Vector3(0, 4.5f, 0), "Förtecken", förtecken, förtecken.all);
        CreateButton(new Vector3(2.5f, 4.5f, 0), "Mollparalleller", mollparalleller, mollparallellDict);
        CreateButton(new Vector3(-2.5f, 3.5f, 0), "Spegelintervall", spegelIntervall, spegelIntervallDict);

        soundLogic = FindAnyObjectByType<SoundLogic>();
        CreateSoundButton();
    }

    public void ScreenClick()
    {
        Debug.Log("screen clicked");
        OnClick();
    }

    private void CreateButton(Vector3 pos, string titleText, Logic.Content button, Dictionary<string,string> currentDict)
    {
        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = titleText;
        GameObject var = Instantiate(buttonPrefab, this.gameObject.transform.parent);
        var.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("button pressed");
            currentButton = button;
            currentDictionary = currentDict;
            title.text = titleText;
            Nollställ();
            OnClick();
        });
        var.transform.position = pos;
    }

    private void CreateSoundButton() {
        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ljud";
        GameObject var = Instantiate(buttonPrefab, this.gameObject.transform.parent);
        var.GetComponent<Button>().onClick.AddListener(() => {
            soundLogic.JoniskIntervall();
            OnClick();
        });
        Nollställ();
        var.transform.position = new Vector3(0f, 3.5f, 0);
    }
    
    public void OnClick()
    {
        Debug.Log("hej" + currentButton + currentDictionary);
        if (currentButton == null) { Debug.Log("currentnull"); return; }
        if (currentButton == soundLogic.ljud)
        {
            Debug.Log("current:: "+currentButton);
            soundLogic.PlayAgain();
            return;
        }
        if (currentDictionary == null) { Debug.Log("currentdictnull"); return; }

        if (ool == false)
        {
            if (dur && currentButton.dur.Count != 0) 
            { 
                currentDictionary = currentButton.dur; 
            }
            else if (moll && currentButton.dur.Count != 0) 
            { 
                currentDictionary = currentButton.moll; 
            }
            else 
            {  
                if (currentButton.all.Count != 0)
                {
                    currentDictionary = currentButton.all;
                    Debug.Log("count är inte 0");
                    foreach (var item in currentDictionary)
                    {
                        Debug.Log(item);
                    }
                }
            }

            ool = true;
            int oldNum = num;
            while (oldNum == num)
            {
                num = UnityEngine.Random.Range(0, currentDictionary.Count);
            }
            Debug.Log("should prompt");

            Prompt(num);
        }
        else
        {
            ool = false;
            if (!(currentDictionary.Count >= num))
            {
                Debug.Log("returning on 2, " + num + currentDictionary.Count);
                return;
            }
            Debug.Log("should answer");
            Answer();
        }
    }

    public void Prompt(int num)
    {
        promptText.text = currentDictionary.ElementAt(num).Key;
        answerText.text = "";
        answer = currentDictionary.ElementAt(num).Value;
    }

    public void Answer()
    {
        answerText.text = answer;
    }

    public void Moll()
    {
        if (mollToggle.isOn == true)
        {
            durToggle.isOn = false;
            dur = false;
            moll = true;
            if (currentButton.moll.Count != 0)
            {
                currentDictionary = currentButton.moll;
            }
        }
        else
        {
            moll = false;
            if (currentButton.all.Count != 0)
            {
                currentDictionary = currentButton.all;
            }
        }
    }

    public void Dur()
    {
        if (durToggle.isOn == true)
        {
            mollToggle.isOn = false;
            dur = true;
            moll = false;
            if (currentButton.dur.Count != 0)
            {
                currentDictionary = currentButton.dur;
            }
        }
        else
        {
            dur = false;
            if (currentButton.all.Count != 0)
            {
                currentDictionary = currentButton.all;
            }
        }
    }

    private void Nollställ()
    {
        answerText.text = "";
        promptText.text = "";
        ool = !ool;
        if (promptText.text == "")
        {
            ool = false;
        }
        else
        {
            ool = true;
        }
    }
}



//kvinter = durKvinter.Concat(mollKvinter).ToDictionary(x => x.Key, x => x.Value);
//förtecken = durFörtecken.Concat(mollFörtecken).ToDictionary(x => x.Key, x => x.Value);

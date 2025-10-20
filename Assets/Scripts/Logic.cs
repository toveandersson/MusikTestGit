using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Logic : MonoBehaviour
{
    SoundLogic soundLogic;

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

    public Content currentButton = new Content();
    public Dictionary<string, string> currentDictionary;

    Content kvinter;
    Content f�rtecken;
    Content mollparalleller;
    Content spegelIntervall;

    public class Buttons
    {
        public Content kvinter = new Content();
        public Content f�rtecken = new Content();
        public Content mollparalleller = new Content();
        public Content ljud = new Content();
        public Content spegelIntervall = new Content();
    }


    void Start()
    {
        soundLogic = FindAnyObjectByType<SoundLogic>();

        kvinter = new Content();
        kvinter.dur = new Dictionary<string, string>{
        { "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" }};
        kvinter.moll = new Dictionary<string, string>{
        { "Am", "Em" }, { "Em", "Bm" }, { "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};
        kvinter.all = new Dictionary<string, string>{
        { "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" },{ "Am", "Em" }, { "Em", "Bm" }, 
        { "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};

        f�rtecken = new Content();
        f�rtecken.dur = new Dictionary<string, string>{
        { "C", "0" },{ "G", "1" },{ "D", "2" },{ "A", "3" },{ "E", "4" },{ "B", "5" },{ "F#", "6" },{ "Gb", "6" },{ "Db", "5" },{ "Ab", "4" },{ "Eb", "3" },{ "Bb", "2" },{ "F", "1" }};
        f�rtecken.moll = new Dictionary<string, string> {
        { "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};
        f�rtecken.all = new Dictionary<string, string>
        {{ "C", "0" },{ "G", "1" },{ "D", "2" },{ "A", "3" },{ "E", "4" },{ "B", "5" },{ "F#", "6" },{ "Gb", "6" },{ "Db", "5" },{ "Ab", "4" },{ "Eb", "3" },{ "Bb", "2" },{ "F", "1" },
        { "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};

        mollparalleller = new Content();
        spegelIntervall = new Content();
        ool = true;
    }

    public void ScreenClick()
    {
        Debug.Log("screen clicked");
        OnClick();
    }

    public void OnClick()
    {
        Debug.Log("hej" + currentButton + currentDictionary);
        if (currentButton == null) { return; }
        if (currentButton == soundLogic.ljud)
        {
            Debug.Log("current:: "+currentButton);
            soundLogic.PlayAgain();
            return;
        }
        if (currentDictionary == null) { return; }

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
                    Debug.Log("count �r inte 0");
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
                num = Random.Range(0, currentDictionary.Count);
            }

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

    public void Kvinter()
    {
        currentButton = kvinter;
        currentDictionary = currentButton.all;
        title.text = "Kvinter";
        Nollst�ll();
    }

    public void F�rtecken()
    {
        currentButton = f�rtecken;
        currentDictionary = currentButton.all;
        title.text = "F�rtecken";
        Nollst�ll();
    }

    public void Mollparalleller()
    {
        currentButton = mollparalleller;
        currentDictionary = mollparallellDict;
        title.text = "Mollparalleller";
        Nollst�ll();
    }

    public void SpegelIntervall()
    {
        currentButton = spegelIntervall;
        currentDictionary = spegelIntervallDict;
        title.text = "Spegelintervall";
        Nollst�ll();
    }

    private void Nollst�ll()
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
//f�rtecken = durF�rtecken.Concat(mollF�rtecken).ToDictionary(x => x.Key, x => x.Value);

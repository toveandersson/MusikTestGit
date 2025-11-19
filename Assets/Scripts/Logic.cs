using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    NoteSheet noteSheet;
    SoundLogic soundLogic;
    public GameObject buttonPrefab;

    public Toggle durToggle;
    public Toggle mollToggle;
    public bool dur;
    public bool moll;

    public TextMeshProUGUI startText;
    public TextMeshProUGUI title;
    public TextMeshProUGUI subTitle;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI answerText;

    public string prompt;
    public string answer;

    int num = 1;
    public bool ool = true;

    int[] currentScaleIntervalsInt;
    private Action clickFunction;

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

    //private string[] circleOfFifths = { "C", "G", "D", "A", "E", "B", "F#/Gb", "C#/Db", "G#/Ab", "D#/Eb", "A#/Bb", "F" };
    private Dictionary<string, int> durFörhållandeICirkeln = new Dictionary<string, int> { { "S2", 2 }, { "S3", 4 }, { "R4", -1 }, { "R5", 1 }, { "S6", 3 }, { "S7", 5 } };

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
        currentScaleIntervalsInt = StaticLibrary.majorScaleIntervals;
        noteSheet = FindAnyObjectByType<NoteSheet>();
        soundLogic = FindAnyObjectByType<SoundLogic>();
        //kvinter = new Content();
        //kvinter.dur = new Dictionary<string, string>{
        //{ "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" }};
        //kvinter.moll = new Dictionary<string, string>{
        //{ "Am", "Em" }, { "Em", "Bm" }, { "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};
        //kvinter.all = new Dictionary<string, string>{
        //{ "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "F#", "C#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" },{ "Am", "Em" }, { "Em", "Bm" }, 
        //{ "Bm", "F#m" }, { "F#m", "C#m" }, { "C#m", "G#m" }, { "G#m", "D#m" }, { "D#m", "A#m" }, { "Ebm", "Bbm" }, { "Bbm", "Fm" }, { "Fm", "Cm" }, { "Cm", "Gm" }, { "Gm", "Dm" }, { "Dm", "Am" }};

        //förtecken = new Content();
        //förtecken.dur = new Dictionary<string, string>{
        //{ "C", "0" },{ "G", "F#" },{ "D", "F# C#" },{ "A", "F# C# G#" },{ "E", "F# C# G#" },{ "B", "F# C# G# D# A#" },{ "F#", "F# C# G# D# A# E#" }, { "C#", "F# C# G# D# A# E# B#" }, {"Cb", "Bb Eb Ab Db Gb Cb Fb" }, { "Gb", "Bb Eb Ab Db Gb Cb" },{ "Db", "Bb Eb Ab Db Gb" },{ "Ab", "Bb Eb Ab Db" },{ "Eb", "Bb Eb Ab" },{ "Bb", "Bb Eb" },{ "F", "Bb" }};
        //förtecken.moll = new Dictionary<string, string> {
        //{ "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};
        //förtecken.all = new Dictionary<string, string>
        //{{ "C", "0" },{ "G", "1" },{ "D", "2" },{ "A", "3" },{ "E", "4" },{ "B", "5" },{ "F#", "6" },{ "Gb", "6" },{ "Db", "5" },{ "Ab", "4" },{ "Eb", "3" },{ "Bb", "2" },{ "F", "1" },
        //{ "Am", "0" },{ "Em", "1" },{ "Bm", "2" },{ "F#m", "3" },{ "C#m", "4" },{ "G#m", "5" },{ "D#m", "6" },{ "Ebm", "6" },{ "Bbm", "5" },{ "Fm", "4" },{ "Cm", "3" },{ "Gm", "2" },{ "Dm", "1" }};

        //mollparalleller = new Content();
        //spegelIntervall = new Content();
        ool = true;

        //CreateButton(new Vector3(-2.5f, 4.5f, 0), "Kvinter", kvinter, kvinter.all);
        //CreateButton(new Vector3(0, 4.5f, 0), "Förtecken", förtecken, förtecken.all);
        //CreateButton(new Vector3(2.5f, 4.5f, 0), "Mollparalleller", mollparalleller, mollparallellDict);
        //CreateButton(new Vector3(-2.5f, 3.5f, 0), "Spegelintervall", spegelIntervall, spegelIntervallDict);

        //Action calculateRandomInterval = CalculateRandomInterval;
        //Action<Action> onClick = OnClick;
        //Action notesOnInstrumentPractice = NotesOnInstrumentPractice;
        //Action<Action> playNote = PlayNote;

        CreateButton(new Vector3(-2.5f, 4.5f, 0), "Intervall", CalculateRandomInterval, OnClick);
        CreateButton(new Vector3(0f, 4.5f, 0), "Notes", NotesOnInstrumentPractice, PlayNote);
        //CreateButton(new Vector3(-2.5f, 4.5f, 0), "Kvinter", kvinter, kvinter.all, durFörhållandeICirkeln);
        //CreateButton(new Vector3(0, 4.5f, 0), "Förtecken", förtecken, förtecken.all, durFörhållandeICirkeln);
        //CreateButton(new Vector3(2.5f, 4.5f, 0), "Mollparalleller", mollparalleller, mollparallellDict, durFörhållandeICirkeln);
        //CreateButton(new Vector3(-2.5f, 3.5f, 0), "Spegelintervall", spegelIntervall, spegelIntervallDict, durFörhållandeICirkeln
        //CreateSoundButton();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ScreenClick();
        }
    }
    public void ScreenClick()
    {
        Debug.Log("screen clicked");
        clickFunction();
    }

    private void AlterMajorScale(int[] notesChanged, int[] changeInHalfSteps)
    {
        //for (int i = 0; i < currentScaleIntervalsInt.Length; i++)
        //{
        //    Debug.Log(currentScaleIntervalsInt[i]);
        //}
        for (int i = 0; i < notesChanged.Length; i++) {
            Debug.Log(currentScaleIntervalsInt[notesChanged[i]-1]);
            (currentScaleIntervalsInt[notesChanged[i]-1]) += changeInHalfSteps[i];
        }
        for (int i = 0; i < currentScaleIntervalsInt.Length; i++)
        {
            Debug.Log(currentScaleIntervalsInt[i]);
        }
    }

    //private void CreateButton(Vector3 pos, string titleText, Logic.Content button, Dictionary<string,string> currentDict, Dictionary<string, int> förhållandeICirkeln)
    //{
    //    buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = titleText;
    //    GameObject var = Instantiate(buttonPrefab, this.gameObject.transform.parent);
    //    //var.GetComponent<Button>().onClick.AddListener(() => {
    //    //    Debug.Log("button pressed");
    //    //    currentButton = button;
    //    //    currentDictionary = currentDict;
    //    //    title.text = titleText;
    //    //    Nollställ();
    //    //    OnClick();
    //    //});
    //    var.GetComponent<Button>().onClick.AddListener(() => {
    //        Debug.Log("button pressed");
    //        //currentButton = button;
    //        num = UnityEngine.Random.Range(0, circleOfFifths.Length -1); // - 1 här?????? ifall längden inte är från 0 vilket jag inte tror
    //        var förhållandeICirkelnKey = spegelIntervallDict.ElementAt(UnityEngine.Random.Range(0, spegelIntervallDict.Count)).Key; //ändra från spegelintervalldict till korrekt med korrekt intervall som finns i förhållande i cirkeln listan!
    //        var hej = förhållandeICirkeln[förhållandeICirkelnKey];
    //        var answer = circleOfFifths[num + förhållandeICirkeln[förhållandeICirkelnKey] % circleOfFifths.Length -1];
    //        currentDictionary = currentDict;
    //        title.text = titleText;
    //        Nollställ();
    //        OnClick();
    //    });
    //    var.transform.position = pos;
    //}

    private void CreateButton(Vector3 pos, string titleText, Action buttonFunction, Action<Action> clickWithButtonFunc)
    {
        var allIntervals = StaticLibrary.intervalsList;
        var majorIntervals = StaticLibrary.majorScaleIntervals;

        buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = titleText;
        GameObject varButton = Instantiate(buttonPrefab, this.gameObject.transform.parent);

        // capture the correct function for this button
        Action buttonAction = delegate {
            clickWithButtonFunc(buttonFunction);
        };

        Button buttonComponent = varButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(delegate {
            buttonFunction();       // run the button's main function
            clickFunction = buttonAction;  // store it globally for ScreenClick
        });

        varButton.transform.position = pos;
        title.text = titleText;
    }



    private void CalculateInterval(int noteNumInCircle, int interval)
    {

    }

    private void CalculateRandomInterval()
    {
        var allIntervalsString = StaticLibrary.intervalsList;

        int oldNum = num;
        while (oldNum == num)
        {
            num = UnityEngine.Random.Range(0, StaticLibrary.circleOfFifths.Length);
        }
        prompt = StaticLibrary.circleOfFifths[num];
        var randomInterval = currentScaleIntervalsInt.ElementAt(UnityEngine.Random.Range(1, currentScaleIntervalsInt.Length));
        var randomIntervalString = StaticLibrary.intervalsList[randomInterval];
        Debug.Log(num +" : "+ StaticLibrary.circleOfFifths[num]);
        var answerIndex = Math.Abs(num + StaticLibrary.intervalsInTheCircle.FirstOrDefault(x => x.Key == randomIntervalString).Value) % (StaticLibrary.circleOfFifths.Length);
        Debug.Log("answ index "+answerIndex);
        answer = StaticLibrary.circleOfFifths[answerIndex];
        subTitle.text = randomIntervalString;
        Nollställ();

        //Debug.Log(randomIntervalString);
        //var intervalNameString= allIntervalsString[StaticLibrary.halfStepsToIntervalName.ElementAt(randomInterval).Key];
        //Debug.Log("intervals: "+StaticLibrary.intervalsInTheCircle.FirstOrDefault(x => x.Key == randomIntervalString));
        //Debug.Log("answer ind: " + answerIndex);
        //Debug.Log("prompt: "+prompt);
        //Debug.Log("answer: "+answer);
        //StaticLibrary.halfStepsToIntervalName.FirstOrDefault(x => x.Value == randomIntervalString).Key % circleOfFifths.Length - 1];
    }

    private void NotesOnInstrumentPractice() 
    {
        var allIntervalsString = StaticLibrary.intervalsList;

        int oldNum = num;
        while (oldNum == num)
        {
            num = UnityEngine.Random.Range(0, StaticLibrary.circleOfFifths.Length);
        }
        prompt = StaticLibrary.circleOfFifths[num];
        //var randomInterval = currentScaleIntervalsInt.ElementAt(UnityEngine.Random.Range(1, currentScaleIntervalsInt.Length - 1));
        //var randomIntervalString = StaticLibrary.intervalsList[randomInterval];
        //var answerIndex = (num + StaticLibrary.intervalsInTheCircle.FirstOrDefault(x => x.Key == randomIntervalString).Value) % (circleOfFifths.Length);
        answer = StaticLibrary.circleOfFifths[num];
        subTitle.text = "";
        //subTitle.text = randomIntervalString;
        Nollställ();

    }
  
    private void PlayNote(Action buttonFunction)
    {
        Debug.Log("sound");
        if (ool == false)
        {
            buttonFunction();
            ool = true;
            Prompt();
            noteSheet.HideNote();
        }
        else
        {
            ool = false;
            soundLogic.PlayNote(answer);
            noteSheet.ShowNote(answer);
        }
    }
    //private void CreateSoundButton() {
    //    buttonPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ljud"; 
    //    GameObject var = Instantiate(buttonPrefab, this.gameObject.transform.parent);
    //    var.GetComponent<Button>().onClick.AddListener(() => {
    //        //soundLogic.JoniskIntervall();
    //        OnClick();
    //    });
    //    Nollställ();
    //    var.transform.position = new Vector3(0f, 3.5f, 0);
    //}
    
    public void OnClick(Action buttonFunction)
    {
        Debug.Log("onclick");
        Debug.Log(clickFunction);
        //if (currentButton == null) { Debug.Log("currentnull"); return; }
        //if (currentButton == soundLogic.ljud)
        //{
        //    Debug.Log("current:: "+currentButton);
        //    soundLogic.PlayAgain();
        //    return;
        //}
        //if (currentDictionary == null) { Debug.Log("currentdictnull"); return; }

        if (ool == false)
        {
            buttonFunction();
            //if (dur && currentButton.dur.Count != 0) 
            //{ 
            //    currentDictionary = currentButton.dur; 
            //}
            //else if (moll && currentButton.dur.Count != 0) 
            //{ 
            //    currentDictionary = currentButton.moll; 
            //}
            //else 
            //{  
            //    if (currentButton.all.Count != 0)
            //    {
            //        currentDictionary = currentButton.all;
            //        Debug.Log("count är inte 0");
            //        foreach (var item in currentDictionary)
            //        {
            //            Debug.Log(item);
            //        }
            //    }
            //}

            ool = true;
            
            Debug.Log("should prompt");

            Prompt();
        }
        else
        {
            ool = false;
            Debug.Log("should answer");
            Answer();
        }
    }

    public void Prompt()
    {
        promptText.text = prompt;
        answerText.text = "";
    }

    public void Answer()
    {
        answerText.text = answer;
    }

    public void Moll()
    {
        Debug.Log("moLLL");
        if (mollToggle.isOn == true)
        {
            durToggle.isOn = false;
            dur = false;
            moll = true;
            int[] notesChanged = { 3, 7 };
            int[] changes = { -1, -1 };

            AlterMajorScale(notesChanged, changes);
            //if (currentButton.moll.Count != 0)
            //{
            //    currentDictionary = currentButton.moll;
            //}
        }
        else
        {
            moll = false;
            //if (currentButton.all.Count != 0)
            //{
            //    currentDictionary = currentButton.all;
            //}
        }
    }

    public void Dur()
    {
        if (durToggle.isOn == true)
        {
            mollToggle.isOn = false;
            dur = true;
            moll = false;
            currentScaleIntervalsInt = StaticLibrary.majorScaleIntervals;
            //if (currentButton.dur.Count != 0)
            //{
            //    currentDictionary = currentButton.dur;
            //}
        }
        else
        {
            dur = false;
            //if (currentButton.all.Count != 0)
            //{
            //    currentDictionary = currentButton.all;
            //}
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

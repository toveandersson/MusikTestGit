using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLogic : MonoBehaviour
{
    Logic logic;
    List<float> frequencies = new List<float>() {
    // Octave 2
    65.41f, 69.30f, 73.42f, 77.78f, 82.41f, 87.31f, 92.50f, 98.00f, 103.83f, 110.00f, 116.54f, 123.47f,
    // Octave 3
    130.81f, 138.59f, 146.83f, 155.56f, 164.81f, 174.61f, 185.00f, 196.00f, 207.65f, 220.00f, 233.08f, 246.94f,
    // Octave 4
    261.63f, 277.18f, 293.66f, 311.13f, 329.63f, 349.23f, 369.99f, 392.00f, 415.30f, 440.00f, 466.16f, 493.88f,
    // Octave 5
    523.25f, 554.37f, 587.33f, 622.25f, 659.25f, 698.46f, 739.99f, 783.99f, 830.61f, 880.00f, 932.33f, 987.77f,
    // Octave 6
    1046.50f, 1108.73f, 1174.66f, 1244.51f, 1318.51f, 1396.91f, 1479.98f, 1567.98f, 1661.22f, 1760.00f, 1864.66f, 1975.53f,
    //Octave 7
    2093.00f, 2217.46f, 2349.32f, 2489.02f, 2637.02f, 2793.83f, 2959.96f, 3135.96f, 3322.44f, 3520.00f, 3729.31f, 3951.07f };
    
    


Dictionary<int, string> intervalNames = new Dictionary<int, string>(){
    { 0, "1" },   { 1, "b2" }, { 2, "2" }, { 3, "b3" }, { 4, "3" }, { 5, "4" },{ 6, "+4 / b5" }, { 7, "5" }, { 8, "+5 / b6" },{ 9, "6" }, { 10, "b7" }, { 11, "7" },{ 12, "8" }};

    List<int> overtoneSeries = new List<int>() { 12, 7, 5, 4, 4 };
    List<int> joniskIntervall = new List<int>() { 2, 4, 5, 7, 9, 11, 12 };

    Sound soundPlayer;
    Sound soundPlayer2;
    Sound[] soundPlayers;
    Sound[] soundPlayers2;

    int step;
    int step2;

    public bool simultaneousNotes;

    private Coroutine CallLjudWithDelayCoroutine;
    private Coroutine StopSoundCoroutineCoroutine;

    //public Content ljud;

    // Start is called before the first frame update
    void Start()
    {
        logic = FindAnyObjectByType<Logic>();
        //ljud = new Content();

        Debug.Log(frequencies.Count);

        soundPlayers = new Sound[5];
        soundPlayers2 = new Sound[5];
        soundPlayer = new GameObject().AddComponent<Sound>();
        soundPlayer2 = new GameObject().AddComponent<Sound>();
        for (int i = 0; i < 5; i++)
        {
            soundPlayers[i] = new GameObject().AddComponent<Sound>();
            soundPlayers[i].transform.parent = soundPlayer.transform;
            soundPlayers2[i] = new GameObject().AddComponent<Sound>();
            soundPlayers2[i].transform.parent = soundPlayer2.transform;
        }

    }

    public void SimultaneousNotes()
    {
        Debug.Log("sim " + simultaneousNotes);
        simultaneousNotes = !simultaneousNotes;
    }

    public void Ljud(Sound soundPlayerVar, int note = 24)
    {
        logic.promptText.text = "";
        logic.title.text = "Ljud";
        if (soundPlayerVar == null || soundPlayer2 == null)
        {
            Debug.Log("soundscript null ");
            return;
        }
        soundPlayerVar.PlayNote(frequencies[note], 1.6f);
        if (soundPlayerVar == soundPlayer)
        {
            AddOvertones(note, soundPlayers);
        }
        else
        {
            AddOvertones(note, soundPlayers2);
        }
    }

    public void PlayNote(string note)
    {
        Debug.Log("note: "+note);
        int index = Array.IndexOf(StaticLibrary.notesInOrder, note);
        Debug.Log("index: " + index);

        if (soundPlayer == null || soundPlayer2 == null)
        {
            Debug.Log("soundscript null ");
            return;
        }
        soundPlayer.PlayNote(frequencies[24+index], 1.6f);
    }

    public void PlayAgain()
    {
        float delay = simultaneousNotes ? 0f : .6f;
        Ljud(soundPlayer, step);
        StartCoroutine(CallLjudWithDelay(delay, step2));
        StartCoroutine(StopSoundCoroutine(soundPlayer2, 1f));
    }

    public void JoniskIntervall()
    {
        //logic.currentButton = ljud;

        soundPlayer.StopPlaying();
        soundPlayer2.StopPlaying();
        if (CallLjudWithDelayCoroutine != null) StopCoroutine(CallLjudWithDelayCoroutine);
        if (StopSoundCoroutineCoroutine != null) StopCoroutine(StopSoundCoroutineCoroutine);

        foreach (var player in soundPlayers)
        {
            player.StopPlaying();
        }
        float delay = simultaneousNotes ? 0f : .6f;
        int modulo = 12;
        int note = 24; // startstep
        int oldStep = step;
        int oldStep2 = step2;
        step = note + joniskIntervall[UnityEngine.Random.Range(0, 7)];
        step2 = note + joniskIntervall[UnityEngine.Random.Range(0, 7)];

        while (step == step2 || (step == oldStep && step2 == oldStep2))
        {
            step = note + joniskIntervall[UnityEngine.Random.Range(0, 7)];
            step2 = note + joniskIntervall[UnityEngine.Random.Range(0, 7)];
        }
        Debug.Log("step 1 " + step + " step 2 " + step2);

        Ljud(soundPlayer, step);
        CallLjudWithDelayCoroutine = StartCoroutine(CallLjudWithDelay(delay, step2));
        StopSoundCoroutineCoroutine = StartCoroutine(StopSoundCoroutine(soundPlayer2, 1f));


        for (int i = 0; i < 12; i++)
        {
            Debug.Log(frequencies[note]);
            note = (note + step) % modulo;
        }

        SoundAnswer(Mathf.Abs(step - step2) % 12);
    }

    private void SoundAnswer(int steps)
    {
        logic.answerText.text = intervalNames[steps];
    }


    IEnumerator CallLjudWithDelay(float delay, int step2)
    {
        yield return new WaitForSeconds(delay);
        Ljud(soundPlayer2, step2);
        StartCoroutine(StopSoundCoroutine(soundPlayer2, 1f));

    }

    private void AddOvertones(int startNote, Sound[] soundPLayers)
    {
        //int note = startNote;
        //int modulo = 24 + 12;
        //for (int i = 0; i < 5; i++)
        //{
        //    step = overtoneSeries[i];
        //    Debug.Log("overtone: " + frequencies[note + overtoneSeries[i] % modulo]);
        //    soundPLayers[i].PlayNote(frequencies[note], Random.Range(0.05f, 0.15f));
        //    note = note + overtoneSeries[i] % modulo;
        //}
    }

    private IEnumerator StopSoundCoroutine(Sound soundPlayer, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundPlayer.StopPlaying();
        foreach (var player in soundPlayers)
        {
            player.StopPlaying();
        }
    }

    private void CircleOfFifths()
    {
        int step = 7;
        int modulo = 12;
        int note = 0; // start
        for (int i = 0; i < 12; i++)
        {
            Debug.Log(frequencies[note]);
            note = (note + step) % modulo;
        }
    }

    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NoteSheet : MonoBehaviour
{
    Logic logic;
    public GameObject sheetBackground;
    public GameObject line;
    public GameObject wholeNote;
    public GameObject wholeNote2;
    public GameObject b;
    private TextMeshProUGUI bTextMeshProUGUI;
    public Vector3 noteCenter = new Vector3(120, 0, 0);
    public Vector3 firstC;
    private float noteDistance = 8;
    public float noteSheetWidth = 100f;
    public float lineHeight = 3f;
    RectTransform thisRect;
    private GameObject[] extraLines;
    
    void Start()
    {
        extraLines = new GameObject[0];
        logic = FindAnyObjectByType<Logic>();
        thisRect = GetComponent<RectTransform>();
        bTextMeshProUGUI = b.GetComponent<TextMeshProUGUI>();
        firstC = noteCenter + new Vector3(noteCenter.x, noteCenter.y - (noteDistance * 3 * 2), 0);
        RectTransform sheetRt = sheetBackground.GetComponent<RectTransform>();
        sheetRt.anchoredPosition = noteCenter;
        sheetRt.sizeDelta = new Vector2(noteSheetWidth + 20, noteDistance * 12 * 2);

        //sheetBackground
        for (int i = 0; i < 11; i++) {
            GameObject lineObj = Instantiate(line, Vector3.zero, Quaternion.identity, transform);
            RectTransform lineRt = lineObj.GetComponent<RectTransform>();

            lineRt.sizeDelta = new Vector2(noteSheetWidth, lineHeight);
            lineRt.anchoredPosition = new Vector2(noteCenter.x, firstC.y - (noteDistance * 4) + noteDistance * i * 2);
            if (i < 3 || i > 7)
            {
                lineRt.sizeDelta = new Vector2(noteDistance*2, lineHeight);
                extraLines.Append<GameObject>(lineObj);
            }
        }
        HideSystem();
    }

    public void ShowNote(string note, GameObject noteObj, int offset = 0, int flatOrSharp = 1)
    {
        thisRect.anchoredPosition = new Vector3(noteCenter.x + offset, noteCenter.y);    
        //var noteC = noteCenter.x + offset;

       // noteCenter = new Vector3(noteC, noteCenter.y);
        int index = Array.IndexOf(StaticLibrary.notesInOrder, note);
        int sheetIndex = Array.IndexOf(logic.currentScaleIntervalsInt, index);
        ShowSystem(offset);
        noteObj.SetActive(true);
        if (sheetIndex == -1)
        {
            Debug.Log("note was not in currentscale: " + index + " " + StaticLibrary.notesInOrder[index]);
            if (flatOrSharp == 0)
            {
                flatOrSharp = UnityEngine.Random.Range(1, 3);
            }
            if (flatOrSharp == 1)
            {
                bTextMeshProUGUI.text = "b";
                sheetIndex = Array.IndexOf(logic.currentScaleIntervalsInt, index+1);
            }
            else if (flatOrSharp == 2)
            {
                bTextMeshProUGUI.text = "#";
                sheetIndex = Array.IndexOf(logic.currentScaleIntervalsInt, index-1);
            }
            b.SetActive(true);
        }

        List<(int shift, int weight)> choices = new();
        if (sheetIndex - 7 >= -4)
            choices.Add((-7, 14));     // 14%
        if (sheetIndex + 10 <= 6+10)
            choices.Add((10, 13));     // 13%
        choices.Add((UnityEngine.Random.Range(0, 2) * 7, 73));

        int totalWeight = choices.Sum(c => c.weight);
        int r = UnityEngine.Random.Range(0, totalWeight);

        int cumulative = 0;
        foreach (var (shift, weight) in choices)
        {
            cumulative += weight;
            if (r < cumulative)
            {
                sheetIndex += shift;
                break;
            }
        }


        //int probabilityIndex = UnityEngine.Random.Range(0, 100);
        //if (sheetIndex -7 > -4)
        //{
        //    if (probabilityIndex < 14) // probability for lower than C
        //    {
        //        sheetIndex -= 7;
        //    }
        //}
        //else if(sheetIndex + 7 < 11)
        //{
        //    if (probabilityIndex < 13) // probability for higher than C
        //    {
        //        sheetIndex += 7*2;
        //    }
        //}
        //else
        //{
        //    sheetIndex += 7 * UnityEngine.Random.Range(0, 2);
        //}
       
        RectTransform bRect = b.GetComponent<RectTransform>();
        bRect.sizeDelta = new Vector2(noteDistance * 2, noteDistance * 2);
        bRect.anchoredPosition = new Vector2(noteCenter.x - noteDistance * 2, sheetIndex * noteDistance + firstC.y);

        // if - 7 eller + 7 = inom range med extended - 4 och höjd +3? (med C) eller +2 : gör detta (kanske en 50/50 sannolikhet på detta!)
        RectTransform rt = noteObj.GetComponent<RectTransform>();
        Debug.Log("note pos " + new Vector2(noteCenter.x, sheetIndex * noteDistance + firstC.y));
        rt.anchoredPosition = new Vector2(noteCenter.x, sheetIndex * noteDistance + firstC.y);
        rt.sizeDelta = new Vector2(noteDistance * 2, noteDistance * 2);

        //var noteCAgain = noteCenter.x - offset;
        //noteCenter = new Vector3(noteC, noteCenter.y);
    }

    public void ShowTwoNotes(string note, string note2, int offset = 0, int flatOrSharp = 1)
    {
        ShowNote(note, wholeNote, offset, flatOrSharp);
        ShowNote(note2, wholeNote2, offset, flatOrSharp);
    }

    public void HideSystem()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void HideNote() { 
        b.SetActive(false);
        wholeNote.SetActive(false);
        wholeNote2.SetActive(false);
    }



    public void ShowSystem(int offset)
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
        b.SetActive(false);
        wholeNote2.SetActive(false);
    }
}

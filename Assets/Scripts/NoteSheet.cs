using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NoteSheet : MonoBehaviour
{
    public GameObject sheetBackground;
    public GameObject line;
    public GameObject wholeNote;
    public GameObject b;
    private TextMeshProUGUI bTextMeshProUGUI;
    public Vector3 noteCenter = new Vector3(150, 0, 0);
    public Vector3 firstC;
    private float noteDistance = 8;
    public float noteSheetWidth = 100f;
    public float lineHeight = 3f;
    
    void Start()
    {
        bTextMeshProUGUI = b.GetComponent<TextMeshProUGUI>();
        firstC = noteCenter + new Vector3(noteCenter.x, noteCenter.y - (noteDistance * 3 * 2), 0);
        RectTransform sheetRt = sheetBackground.GetComponent<RectTransform>();
        sheetRt.anchoredPosition = noteCenter;
        sheetRt.sizeDelta = new Vector2(noteSheetWidth + 20, noteDistance * 10 * 2);

        //sheetBackground
        for (int i = 0; i < 9; i++) {
            GameObject lineObj = Instantiate(line, Vector3.zero, Quaternion.identity, transform);
            RectTransform lineRt = lineObj.GetComponent<RectTransform>();

            lineRt.sizeDelta = new Vector2(noteSheetWidth, lineHeight);

            // anchored position (UI position)
            lineRt.anchoredPosition = new Vector2(noteCenter.x, firstC.y - (noteDistance * 2) + noteDistance * i * 2);
            if (i < 2 || i > 6)
            {
                lineRt.sizeDelta = new Vector2(noteDistance*2, lineHeight); 
            }
        }
    }

    public void ShowNote(string note, int flatOrSharp = 0)
    {
        int index = Array.IndexOf(StaticLibrary.notesInOrder, note);
        int noteIndex = Array.IndexOf(StaticLibrary.majorScaleIntervals, index);
        ShowSystem();
        if (noteIndex == -1)
        {
            if (flatOrSharp == 0)
            {
                flatOrSharp = UnityEngine.Random.Range(1, 3);
            }
            if (flatOrSharp == 1)
            {
                bTextMeshProUGUI.text = "b";
                noteIndex = Array.IndexOf(StaticLibrary.majorScaleIntervals, index+1);
            }
            else if (flatOrSharp == 2)
            {
                bTextMeshProUGUI.text = "#";
                noteIndex = Array.IndexOf(StaticLibrary.majorScaleIntervals, index-1);
            }
            b.SetActive(true);
            RectTransform bRect = b.GetComponent<RectTransform>();
            bRect.sizeDelta = new Vector2(noteDistance * 2, noteDistance * 2);
            bRect.anchoredPosition = new Vector2(noteCenter.x - noteDistance * 2, noteIndex * noteDistance + firstC.y);
        }
        //index += 12*UnityEngine.Random.Range(0, 2);
        PlaceNote(noteIndex);
    }
    private void PlaceNote(int noteIndex)
    {
        RectTransform rt = wholeNote.GetComponent<RectTransform>();
        Debug.Log("note pos "+ new Vector2(noteCenter.x, noteIndex * noteDistance + firstC.y));
        rt.anchoredPosition = new Vector2(noteCenter.x, noteIndex * noteDistance + firstC.y);
        rt.sizeDelta = new Vector2(noteDistance * 2, noteDistance * 2);
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
    }

    public void ShowSystem()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(true);
        }
        b.SetActive(false);
    }
}

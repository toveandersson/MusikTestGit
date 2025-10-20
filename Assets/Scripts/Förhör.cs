using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Förhör : MonoBehaviour
{
    private Dictionary<string, string> kvinter = new Dictionary<string, string>{
        { "C", "G" }, { "G", "D" }, { "D", "A" }, { "A", "E" }, { "E", "B" }, { "B", "F#" }, { "Gb", "Db" }, { "Db", "Ab" }, { "Ab", "Eb" }, { "Eb", "Bb" }, { "Bb", "F" }, { "F", "C" }};
    private Dictionary<string, int> förtecken = new Dictionary<string, int>{
        { "C", 0}, { "G", 1 }, { "D", 2}, { "A", 3}, { "E", 4}, { "B", 5}, { "F#",6 }, { "Gb",6 }, { "Db", 5}, { "Ab", 4}, { "Eb", 3}, { "Bb", 2}, { "F", 1}};
    public Button kvintKnapp;
    public Button förteckenKnapp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

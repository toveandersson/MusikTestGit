using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyLib 
{
    //°5
    public static string[] circleOfFifths = { "C", "G", "D", "A", "E", "B", "F#/Gb", "C#/Db", "G#/Ab", "D#/Eb", "A#/Bb", "F" };
    public static string[] intervalsList = { "P1", "m2", "M2", "m3", "M3", "P4", "A4/d5", "P5", "m6", "M6", "m7", "M7", "P8" };
    public static string[] notesInOrder = new string[] { "C", "C#/Db", "D", "D#/Eb", "E", "F", "F#/Gb", "G", "G#/Ab", "A", "A#/Bb", "B" };
    //public static Dictionary<int, string> halfStepsToIntervalName = new Dictionary<int, string> { { 0, "P1" }, { 1, "m2" }, { 2, "M2" }, { 3, "m3" }, { 4, "M3" }, { 5, "P4" }, { 6, "A4/d5" }, { 7, "P5" }, { 8, "m6" }, { 9, "M6" }, { 10, "m7" }, { 11, "M7" }, { 12, "P8" } };


    public static int[] majorScaleIntervals = { 0, 2, 4, 5, 7, 9, 11 };   //alterera för att få olika skalor

    public static string[] major = { "" };

    public static Dictionary<string, int> intervalsInTheCircle = new Dictionary<string, int> { { "m2", -5 }, { "M2", 2 }, { "m3", -3 }, { "M3", 4 }, { "P4", -1 }, { "A4/d5", 6 }, { "P5", 1 }, { "m6", -4 }, { "M6", 3 }, { "m7", -2 }, { "M7", 5 } };


    //public static 
    //intervall in circle 
}

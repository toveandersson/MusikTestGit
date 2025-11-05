using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenCircle : MonoBehaviour
{
    public GameObject circle;
    public GameObject cancel;
    bool ool = true;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            circle.SetActive(ool);
            cancel.SetActive(ool);
            ool = !ool;
        }
    }
}

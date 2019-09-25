using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}

using System;
using TMPro;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    public TextMeshProUGUI InGameConsoleText;

    private void Start()
    {
        InGameConsoleText = GetComponent<TextMeshProUGUI>();
    }

    public void Print(string input)
    {
        InGameConsoleText.text = input;
    }
}

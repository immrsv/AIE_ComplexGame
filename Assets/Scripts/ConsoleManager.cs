using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConsoleManager : MonoBehaviour {

    public string BoxTitle;
    public UnityEngine.UI.Text TextWidget;
    public int MaxLines = 10;

    private List<string> lines = new List<string>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddLine(string line)
    {
        lines.Add(Time.realtimeSinceStartup.ToString("N3") + ": " + line);

        while (lines.Count > MaxLines)
            lines.RemoveAt(0);

        var sb = new StringBuilder(BoxTitle);

        foreach (var l in lines)
            sb.Append("\n" + l);

        TextWidget.text = sb.ToString();
    }
}

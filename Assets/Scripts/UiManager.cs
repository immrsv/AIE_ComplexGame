using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class UiManager: MonoBehaviour {

    public Dictionary<string, UnityEngine.UI.Text> Widget = new Dictionary<string, UnityEngine.UI.Text>();
    public Dictionary<string, System.Text.StringBuilder> History = new Dictionary<string, StringBuilder>();

    public GameObject SelectedObject { get; set; }

	// Use this for initialization
	void Start () {
		
        foreach ( var widget in GetComponentsInChildren<UnityEngine.UI.Text>() ) {
            Widget.Add(widget.gameObject.name, widget);
            History.Add(widget.gameObject.name, new StringBuilder());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddLine(string widget, string line)
    {
        History[widget].Append("\n" + line);

        if (History[widget].ToString().Count(m => m == '\n') > 20)
            History[widget].Remove(0, History[widget].ToString().IndexOf('\n', 1));

        Widget[widget].text = History[widget].ToString();
    }
}

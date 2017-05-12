using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

[DisallowMultipleComponent]
public class UiManager : MonoBehaviour {

    public static UiManager Instance { get; protected set; }

    public Queue<string> ConsoleHistory = new Queue<string>();

    public TextAsset Readme;
    public GameObject Mothership;
    public UnityEngine.UI.Text MothershipObjectStatus;
    public UnityEngine.UI.Text SelectedObjectStatus;

    public bool IsUpdateRequired { get; set; }

    private GameObject _SelectedObject;
    public GameObject SelectedObject {
        get {
            return _SelectedObject;
        }
        set {
            if (_SelectedObject != null) {
                _SelectedObject.BroadcastMessage("OnSelectionEnd", options: SendMessageOptions.DontRequireReceiver);
            }

            _SelectedObject = value;

            if (_SelectedObject != null) {
                SelectedObjectAgent = _SelectedObject.GetComponent<AgentAI.Agent>();
                SelectedObjectContainers = _SelectedObject.GetComponent<Resources.ContainerCollection>();
                _SelectedObject.BroadcastMessage("OnSelectionStart",options: SendMessageOptions.DontRequireReceiver);
            }
        }
    }


    private AgentAI.Agent MothershipAgent;
    private AgentAI.Agent SelectedObjectAgent;

    private Resources.ContainerCollection MothershipContainers;
    private Resources.ContainerCollection SelectedObjectContainers;


    // Use this for initialization
    void Start() {
        Instance = this;


        MothershipAgent = Mothership.GetComponent<AgentAI.Agent>();
        MothershipContainers = Mothership.GetComponent<Resources.ContainerCollection>();

        IsUpdateRequired = true;
    }

    // Update is called once per frame
    void Update() {

        if (IsUpdateRequired) {
            //IsUpdateRequired = false;

            if (SelectedObject != null) {
                switch (SelectedObject.tag) {
                    case "Droneship":
                        SelectedObjectStatus.text = "Selected: " + SelectedObject.name + "\n" + SelectedObjectAgent.Log + "\n" + SelectedObjectContainers.StatusAsString();
                        break;
                    case "Asteroid":
                        SelectedObjectStatus.text = "Selected: " + SelectedObject.name + "\n" + SelectedObjectContainers.StatusAsString();
                        break;
                    case "Enemy":
                        SelectedObjectStatus.text = "Selected: " + SelectedObject.name + "\n" + SelectedObjectAgent.Log;
                        break;
                    default:
                        SelectedObjectStatus.text = Readme.text;
                        break;
                }
            } else {
                SelectedObjectStatus.text = Readme.text;
            }

            MothershipObjectStatus.text = MothershipAgent.Log + "\n" + MothershipContainers.StatusAsString();
        }
    }

}

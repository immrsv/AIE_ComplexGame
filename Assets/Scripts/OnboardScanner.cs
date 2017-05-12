using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class OnboardScanner : MonoBehaviour {

    public class Result {
        public string IncludesTag;
        public List<GameObject> Items;
    }
    public enum TaglistMode {
        Whitelist,
        Blacklist
    }

    public float ScanDelay = 20;
    public float ScanRange = 20;
    public bool UseSingleBroadcast;

    public TaglistMode FilterMode;
    public List<string> TagList = new List<string>();

    private float NextScan;

    private DebugFlag.DebugLevel DebugLevel;

	// Use this for initialization
	void Start () {
        NextScan = Time.realtimeSinceStartup + ScanDelay;

        var db = GetComponent<DebugFlag>();
        DebugLevel = db == null ? 0 : db.Level;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup < NextScan)
            return;


        NextScan = Time.realtimeSinceStartup + ScanDelay;

        var hits = Physics.OverlapSphere(transform.position, ScanRange)
                    .Where(m => (FilterMode == TaglistMode.Whitelist && TagList.Contains(m.gameObject.tag)) || (FilterMode == TaglistMode.Blacklist && !TagList.Contains(m.gameObject.tag)))
                    .Select(m => m.gameObject)
                    .ToList();

        if (DebugLevel.HasAny(DebugFlag.DebugLevel.Information))
            Debug.Log(gameObject.name + " scanned " + hits.Count + " responsive tags");

        if (UseSingleBroadcast) {
            BroadcastMessage("OnScannerDetect", new Result { IncludesTag = "All", Items = hits }, options: SendMessageOptions.DontRequireReceiver);
        } else {
            var sets = hits.GroupBy(m => m.tag);
            foreach ( var x in sets ) {
                BroadcastMessage("OnScannerDetect", new Result { IncludesTag = x.Key, Items = x.ToList()}, options: SendMessageOptions.DontRequireReceiver);
            }
        }

    }
}

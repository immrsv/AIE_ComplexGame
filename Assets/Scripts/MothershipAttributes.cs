﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Resources;

public class MothershipAttributes : MonoBehaviour {
    
    private ResourceContainer CargoBay;

	// Use this for initialization
	void Start () {
        CargoBay = GetComponent<ContainerCollection>()["CargoBay"];
	}
	
	// Update is called once per frame
	void Update () {
	}
}

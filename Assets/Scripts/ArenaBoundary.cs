using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBoundary : MonoBehaviour {

    void OnTriggerExit(Collider col) {

        Destroy(col.gameObject);
    }
}

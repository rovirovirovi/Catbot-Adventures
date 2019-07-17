using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName="Objects/Dialog")]
public class Dialog : ScriptableObject {
    public List<string> dialogueParts;
}
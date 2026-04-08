using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Data")]
public class Data : ScriptableObject
{
    public List<StateBox> states = new List<StateBox>();
    public List<Connections> connections = new List<Connections>();
}

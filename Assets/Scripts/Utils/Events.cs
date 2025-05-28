using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "My Objects/EventTypes")]
public class Events : ScriptableObject
{
    public Dictionary<string, float> types;
    public string a;
    public List<string> dsa;
}

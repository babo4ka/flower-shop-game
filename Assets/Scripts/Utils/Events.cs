using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public static readonly Dictionary<string, float> events = new() {
        { "поломка кассы", 200f }, { "поломка стола", 250 }
    };
}

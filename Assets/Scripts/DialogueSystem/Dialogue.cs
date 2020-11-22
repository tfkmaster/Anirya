using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public string[] names;
    [TextArea(3, 10)]
    public string[] sentences;
}

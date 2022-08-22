using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Junk", menuName = "Junk")]
public class Junk : ScriptableObject
{
    public string junkName;
    public float baseValue;
    public Sprite junkImage;
}

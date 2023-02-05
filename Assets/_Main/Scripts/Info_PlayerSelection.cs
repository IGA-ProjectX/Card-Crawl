using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selection", menuName = "Scriptable Objs/Player Selection")]
public class Info_PlayerSelection : ScriptableObject
{
    public List<SpecialCard> effects = new List<SpecialCard>();
}

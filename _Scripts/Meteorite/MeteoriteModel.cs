using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteModel : MonoBehaviour
{
    public MeteoriteStats meteoriteStats;

    public int Damage => meteoriteStats != null ? meteoriteStats.damage : 25;
    public float Size => meteoriteStats != null ? meteoriteStats.size : 1f;
    public float Speed => meteoriteStats != null ? meteoriteStats.speed : 5f;
    public Color Color => meteoriteStats != null ? meteoriteStats.color : Color.grey;
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewMeteoriteStats", menuName = "Meteorite/Stats", order = 1)]
public class MeteoriteStats : ScriptableObject
{
    public int damage;
    public float size;
    public float speed;
    public Color color;
}

using UnityEngine;

[CreateAssetMenu]
public class ProjectileInfo : ScriptableObject {
    public SoundInfo hitSound;
    public int speed;
    public int damage;
    [Range(0,100)]
    public int slowDown = 0;
}

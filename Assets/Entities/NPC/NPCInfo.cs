using UnityEngine;

[CreateAssetMenu]
public class NPCInfo : ScriptableObject
{
    public SoundInfo[] sounds;
    public SoundInfo deadSound;
    public SoundInfo fireSound;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;
    public AnimationClip walkTop;
    public AnimationClip walkBottom;
    public GameObject projectile;
    public float speed = 0.4f;
    public bool movable = true;
    public int awaranessRadiusLow = 10;
    public int awaranessRadiusHigh = 7;
    public int health;
}

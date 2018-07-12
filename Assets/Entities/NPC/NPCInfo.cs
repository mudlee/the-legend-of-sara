using UnityEngine;

[CreateAssetMenu]
public class NPCInfo : ScriptableObject
{
    public SoundInfo[] sounds;
    public SoundInfo deadSound;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;
    public AnimationClip walkTop;
    public AnimationClip walkBottom;
}

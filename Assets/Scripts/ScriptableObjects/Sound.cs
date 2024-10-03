using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    public enum AudioTypes
    {
        SFX,
        Music
    }
    public AudioTypes AudioType;
    public AudioClip Clip;
    public bool Loop = false;
    public bool RandomizePitch = false;
    [Range(0f, 1f)]
    public float RandomPitchRangeModifier = 0.1f;
    [Range(0.1f, 3f)]
    public float Volume = 1f;
    [Range(0.1f, 2f)]
    public float Pitch = 1f;

}
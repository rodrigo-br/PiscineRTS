using UnityEngine;

[CreateAssetMenu()]
public class SoundsCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] GamePlayMusic;
    public SoundSO[] UIMusic;
    public SoundSO[] StartScreenMusic;
    [Header("SFX")]
    public SoundSO[] Running;
}
using System;
using UnityEngine;


public enum SoundType
{
    MainThemes,
    InTurnMusic,
    ButtonSound,
    SwordSwing,
    SwordHit,
    ShotArrow,
    ArrowHit,
    ShotFireball,
    FireballHit,
    ShotElectric,
    ElectricHit,
    SpiderSound,
    SpiderAttack,
    EscButtonSound,
    RoomSelectionSound
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Start() {
         audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType type, float volume=1) {
        //instance.audioSource.PlayOneShot(instance.soundList[(int)type], volume);

        AudioClip[] clips = soundList[(int)type].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(randomClip, volume);
    }

    // public void PlaySoundWrapper(SoundType type, float volume = 1)   //Boyle yapinca button uzerinden onclick ile cagrilmiyor
    // {
    //     PlaySound(type, volume);
    // }

    public void PlaySoundWrapper(int type) //Button testi icin lazim sonra SILINECEK
    {
        PlaySound((SoundType)type);
    }
    
#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList {
        
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;

}

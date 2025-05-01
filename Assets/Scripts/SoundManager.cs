using System;
using System.Collections;
using UnityEngine;


public enum SoundType
{
    SwordSwing,
    SmiteSwordSwing,
    SwordHit,
    HealHitSound,
    FireballSent,
    FireballHit,
    LightningHit,
    RotateHit,
    VillageTakeDamageSound,
    EmptyPlayerEffectSound,
    HealPlayerEffectSound,
    NormalSwordSwingPlayerEffectSound,
    SmiteSwordSwingPlayerEffectSound,
    FireballPlayerEffectSound,
    LightningPlayerEffectSound,
    EnemyWizardSkillHitSound,
    SpiderSound,
    SpiderAttack,
    SpiderWebAttack,
    ArrowSent,
    ArrowHit,
    JumpWalkSound,
    NormalWalkSound,
    MainThemes,
    InTurnMusic,
    ButtonFireSound,
    PopupNPopupContinueSound,
    PopupBackSound,
    RoomSelectionSound,
    PlayerSelectSound,
    TurnSwitchSound,
    VillageInstantiateSound,
    CharacterNEnemyInstantiateSound,
    WaterInstantiateSound,
    MountainInstantiateSound,
    GameOverSound,
    YouWinSound,
    MissionInLevelCompleteSound,
    SwitchingToLevelSelectionSound,
    BumpSound,
    GameStartWalkSound,
    DrumTakeDamageSound,
    InTurnEnemyInstantiateSound,
    MaskSoundEffect,
    BigStatueEyeOpenSoundEffect,
    LevelChangeSnowWalkSound
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : Singleton<SoundManager>
{
    private static SoundManager instance;
    
    public float MainThemeSoundVolume = 1f;
    public bool ContinueMainTheme = true;
    
    [SerializeField] 
    private AudioSource audioSource;
    
    [SerializeField] 
    private SoundList[] soundList;

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

    public void StartMainTheme() {
        StartCoroutine(PlayMainTheme());
    }

    IEnumerator PlayMainTheme()
    {
        PlaySound(SoundType.MainThemes, MainThemeSoundVolume);

        yield return new WaitForSeconds(222f);

        if (ContinueMainTheme) {
            PlayMainTheme();
        }
    }
    
    public void StopMainThemeTimed()
    {
        // Schedule the stop calls
        Timed.Run(() => StopCoroutine(PlayMainTheme()), 19.5f);
        Timed.Run(() => audioSource.Stop(), 19.5f);

        // Start fading volume down over 5.5 seconds
        StartCoroutine(FadeVolumeOverTime(19.5f));
        ContinueMainTheme = false;
    }

    private IEnumerator FadeVolumeOverTime(float duration)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        audioSource.volume = startVolume;
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

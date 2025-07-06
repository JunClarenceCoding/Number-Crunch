using UnityEngine;
using System.Collections;
using System.Media;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip BattleBackground;
    public AudioClip ButtonClick;
    public AudioClip ButtonNumbers;
    public AudioClip WinSound;
    public AudioClip RewardSound;
    public AudioClip WrongClock;
    public AudioClip TimesUp;
    public AudioClip RunningSound;
    public AudioClip GameoverSFX;
    public AudioClip LevelUpSound;
    public AudioClip AchievementBtnClick;
    public AudioClip PurchaseItem;
    public AudioClip CorrectAnswer;
    public AudioClip WrongAnswer;
    public  AudioClip Countdown;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip noDamage;
    public AudioClip hitSFX;
    public  AudioClip defendSFX;

    [Header("Audio Clips Tutorial Part1 NPC")]
    public AudioClip[] tutorialPart1Clips;

    [Header("Audio Clips Tutorial Part2 NPC")]
    public AudioClip[] tutorialPart2Clips;

    [Header("Audio Clips Tutorial Part3 NPC")]
    public AudioClip[] tutorialPart3Clips;

    [Header("Audio Clips Tutorial Part4 NPC")]
    public AudioClip[] tutorialPart4Clips;

    [Header("Music Volume Settings")]
    [SerializeField] private float loweredMusicVolume = 0.2f; 
    private float originalMusicVolume;
    private Coroutine restoreMusicVolumeCoroutine;

    private void Start()
    {
        if (musicSource != null)
        {
            originalMusicVolume = musicSource.volume; 
            PlayMusic(); 
        }
    }
    public void PlayMusic()
    {
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }
    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    public void StopStartMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.clip = background;
            musicSource.Stop();
        }  
    }
    public void playSFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void clickButton()
    {
        playSFX(ButtonClick);
    }
    public void clickButtonNumbers()
    {
        playSFX(ButtonNumbers);
    }
    public void TriggerWinSound()
    {
        musicSource.mute = true;
        playSFX(WinSound);
    }
    public void playWinSoundTest()
    {
        StartCoroutine(triggerWinSoundTest());
    }
    private IEnumerator triggerWinSoundTest()
    {
        musicSource.mute = true;
        playSFX(WinSound);
        yield return new WaitForSeconds(WinSound.length);
        musicSource.mute = false;
    }
    public void triggerGameoverSFX()
    {
        StartCoroutine(triggerGameoverTest());
    }
    private IEnumerator triggerGameoverTest()
    {
        musicSource.mute = true;
        playSFX(GameoverSFX);
        yield return new WaitForSeconds(GameoverSFX.length);
        musicSource.mute = false;
    }
    public void PlayRewardSoundWithUnmute()
    {
        StartCoroutine(PlayRewardWithMute());
    }
    public void PlayRewardSound()
    {
        StartCoroutine(PlayRewardWithUnMute());
    }
    private IEnumerator PlayRewardWithUnMute()
    {
        playSFX(RewardSound);
        yield return new WaitForSeconds(RewardSound.length);
        musicSource.mute = false;
    }
    private IEnumerator PlayRewardWithMute()
    {
        musicSource.mute = true;
        playSFX(RewardSound);
        yield return new WaitForSeconds(RewardSound.length);
        musicSource.mute = false;
    }
    public void playLevelUpSound()
    {
        StartCoroutine(playLevelUpWithMute());
    }
    private IEnumerator playLevelUpWithMute()
    {
        musicSource.mute = true;
        playSFX(LevelUpSound);
        yield return new WaitForSeconds(LevelUpSound.length);
        musicSource.mute = false;
    }
    public void PlayBattleBackground()
    {
        if (BattleBackground != null && musicSource != null)
        {
            musicSource.Stop(); 
            musicSource.clip = BattleBackground;
            musicSource.Play();
        }
    }
    public void PlayBackgroundMusic()
    {
        if (background != null && musicSource != null)
        {
            musicSource.Stop();
            musicSource.clip = background;
            musicSource.Play();
        }
    }
    public void triggerWrongClock()
    {
        playSFX(WrongClock);
    }
    public void triggerTimesUp()
    {
        playSFX(TimesUp);
    }
    public void triggerRun()
    {
        if (SFXSource != null && RunningSound != null)
        {
            SFXSource.clip = RunningSound;
            SFXSource.loop = true; 
            SFXSource.Play();
        }
    }
    public void stopRun()
    {
        if (SFXSource != null && SFXSource.clip == RunningSound && SFXSource.isPlaying)
        {
            SFXSource.Stop();
            SFXSource.clip = null; 
        }
    }
    public void clickAchivement()
    {
        playSFX(AchievementBtnClick);
    }
    public void successPurchase()
    {
        playSFX(PurchaseItem);
    }
    public void triggerWrongAnswer()
    {
        playSFX(WrongAnswer);
    }
    public void triggerCorrectAnswer()
    {
        playSFX(CorrectAnswer);
    }
    public void playCountdown()
    {
        StartCoroutine(playCountdownWithDelay());
    }
    private IEnumerator playCountdownWithDelay()
    {
        musicSource.mute = true;
        playSFX(Countdown);
        yield return new WaitForSeconds(Countdown.length);
        musicSource.mute = false;
    }
    public void triggerAttack()
    {
        playSFX(attack1);
    }
    public void triggerAttack1()
    {
        playSFX(attack2);
    }
    public void triggerNoDamage()
    {
        playSFX(noDamage);
    }
    public void getHit()
    {
        playSFX(hitSFX);
    }
    public void getDefend()
    {
        playSFX(defendSFX);
    }
    public void PlaySFXWithPriority(AudioClip clip)
    {
        if (clip == null || SFXSource == null || musicSource == null) return;
        SFXSource.Stop();
        SFXSource.loop = false;

        // Lower the music volume
        musicSource.volume = loweredMusicVolume;

        // Play the new SFX
        SFXSource.clip = clip;
        SFXSource.Play();

        // Stop any active coroutine and start a new one
        if (restoreMusicVolumeCoroutine != null)
        {
            StopCoroutine(restoreMusicVolumeCoroutine);
        }
        restoreMusicVolumeCoroutine = StartCoroutine(RestoreMusicVolumeAfterSFX(clip.length));
    }
    private IEnumerator RestoreMusicVolumeAfterSFX(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        // Restore the original music volume
        musicSource.volume = originalMusicVolume;

        restoreMusicVolumeCoroutine = null;
    }
    public void StopCurrentSFX()
    {
        if (SFXSource != null && SFXSource.isPlaying)
        {
            SFXSource.Stop(); 
        }
        if (restoreMusicVolumeCoroutine != null)
        {
            StopCoroutine(restoreMusicVolumeCoroutine);
            restoreMusicVolumeCoroutine = null;
        }

        // Restore the original music volume immediately
        musicSource.volume = originalMusicVolume;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource PauseMenuSource;

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource LoopedMusicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource VoiceSource;

    [SerializeField] private AudioClip AwakeSound;
    [SerializeField] private AudioClip StartSound;
    [SerializeField] private AudioClip LoopedSound;
    [SerializeField] private AudioClip HoverSound;
    [SerializeField] private AudioClip ClickSound;
    [SerializeField] private AudioClip ErrorSound;
    [SerializeField] private AudioClip BuySound;
    [SerializeField] private AudioClip LockedDoorSound;

    [SerializeField] private AudioClip MerchantMusic;
    [SerializeField] private AudioClip SaferoomMusic;
    [SerializeField] private AudioClip PauseMusic;
    [SerializeField] private AudioClip ChaseMusic;
    [SerializeField] private AudioClip SuspenseMusic;
    [SerializeField] private AudioClip JackMusic;
    [SerializeField] private AudioClip DeathMusic;
    [SerializeField] private AudioClip CreditsMusic;

    [SerializeField] private AudioClip Footstep1;
    [SerializeField] private AudioClip Footstep2;
    [SerializeField] private AudioClip Footstep3;
    [SerializeField] private AudioClip Footstep4;
    [SerializeField] private AudioClip Footstep5;
    [SerializeField] private AudioClip Footstep6;
    [SerializeField] private AudioClip Footstep7;
    [SerializeField] private AudioClip Footstep8;
    [SerializeField] private AudioClip Footstep9;
    [SerializeField] private AudioClip Footstep10;
    [SerializeField] private AudioClip Footstep11;
    [SerializeField] private AudioClip Footstep12;
    [SerializeField] private AudioClip Footstep13;
    [SerializeField] private AudioClip Footstep14;
    [SerializeField] private AudioClip Footstep15;
    [SerializeField] private AudioClip Footstep16;
    [SerializeField] private AudioClip Footstep17;
    [SerializeField] private AudioClip Footstep18;
    [SerializeField] private AudioClip Footstep19;
    [SerializeField] private AudioClip Footstep20;
    [SerializeField] private AudioClip Footstep21;

    [SerializeField] private AudioClip UseItem;
    [SerializeField] private AudioClip PickItem;

    [SerializeField] private AudioClip DoorOpen;

    [SerializeField] private AudioClip GoldSound;
    [SerializeField] private AudioClip TreasureSound;
    [SerializeField] private AudioClip CheatSound;

    [SerializeField] private AudioClip FlashbangSound;
    [SerializeField] private AudioClip EarRingingSound;
    [SerializeField] private AudioClip HandgrenadeSound;

    [SerializeField] private AudioClip PistolEquip;
    [SerializeField] private AudioClip PistolShot;
    [SerializeField] private AudioClip PistolMagOut;
    [SerializeField] private AudioClip PistolMagIn;
    [SerializeField] private AudioClip PistolSlideBack;
    [SerializeField] private AudioClip PistolSlideForward;
    [SerializeField] private AudioClip PistolEmpty;

    [SerializeField] private AudioClip RifleShot;
    [SerializeField] private AudioClip RifleMagOut;
    [SerializeField] private AudioClip RifleMagIn;
    [SerializeField] private AudioClip RifleSlideBack;
    [SerializeField] private AudioClip RifleSlideForward;
    [SerializeField] private AudioClip RifleFinishReload;
    [SerializeField] private AudioClip RifleEmpty;

    [SerializeField] private AudioClip ShotgunEquip;
    [SerializeField] private AudioClip ShotgunShot;
    [SerializeField] private AudioClip ShotgunShellIn;
    [SerializeField] private AudioClip ShotgunEmpty;

    [SerializeField] private AudioClip MagnumEquip;
    [SerializeField] private AudioClip MagnumShot;
    [SerializeField] private AudioClip MagnumReload;
    [SerializeField] private AudioClip MagnumEmpty;

    [SerializeField] private AudioClip MaleAlerted1;
    [SerializeField] private AudioClip MaleAlerted2;
    [SerializeField] private AudioClip MaleAlerted3;
    [SerializeField] private AudioClip MaleAlerted4;
    [SerializeField] private AudioClip MaleAlerted5;
    [SerializeField] private AudioClip MaleAlerted6;
    [SerializeField] private AudioClip MaleAlerted7;
    [SerializeField] private AudioClip MaleAlerted8;
    [SerializeField] private AudioClip MaleAlerted9;
    [SerializeField] private AudioClip MaleAlerted10;
    [SerializeField] private AudioClip MaleAlerted11;

    [SerializeField] private AudioClip FemaleAlerted1;
    [SerializeField] private AudioClip FemaleAlerted2;
    [SerializeField] private AudioClip FemaleAlerted3;

    [SerializeField] private AudioClip MaleEnemyIdle1;
    [SerializeField] private AudioClip MaleEnemyIdle2;
    [SerializeField] private AudioClip MaleEnemyIdle3;
    [SerializeField] private AudioClip MaleEnemyIdle4;

    [SerializeField] private AudioClip MaleHit1;
    [SerializeField] private AudioClip MaleHit2;
    [SerializeField] private AudioClip MaleHit3;
    [SerializeField] private AudioClip MaleHit4;

    [SerializeField] private AudioClip MaleDeath1;
    [SerializeField] private AudioClip MaleDeath2;
    [SerializeField] private AudioClip MaleDeath3;
    [SerializeField] private AudioClip MaleDeath4;

    [SerializeField] private AudioClip FemaleHit1;
    [SerializeField] private AudioClip FemaleHit2;
    [SerializeField] private AudioClip FemaleHit3;
    [SerializeField] private AudioClip FemaleHit4;

    [SerializeField] private AudioClip FemaleDeath1;
    [SerializeField] private AudioClip FemaleDeath2;

    private bool jackFightStarted = false;

    public void PlayClickSound()
    {
        SFXSource.PlayOneShot(ClickSound);
    }

    public void PlayHoverSound()
    {
        SFXSource.PlayOneShot(HoverSound);
    }

    public void PlayAwakeSound()
    {
        MusicSource.clip = AwakeSound;
        MusicSource.Play();
    }

    public void PlayStartSound()
    {
        MusicSource.PlayOneShot(StartSound);
    }

    public void PlayLoopedMainMenuMusic()
    {
        LoopedMusicSource.clip = LoopedSound;
        LoopedMusicSource.Play();
    }

    public void PlaySaferoomMusic()
    {
        LoopedMusicSource.clip = SaferoomMusic;
        LoopedMusicSource.Play();
    }

    public void StopMusic()
    {
        LoopedMusicSource.Stop();
        MusicSource.Stop();
    }

    public void PlayFootstepSound()
    {
        int random = Random.Range(0, 20);
        switch (random)
        {
            case 0:
                SFXSource.PlayOneShot(Footstep1);
                break;
            case 1:
                SFXSource.PlayOneShot(Footstep2);
                break;
            case 2:
                SFXSource.PlayOneShot(Footstep3);
                break;
            case 3:
                SFXSource.PlayOneShot(Footstep4);
                break;
            case 4:
                SFXSource.PlayOneShot(Footstep5);
                break;
            case 5:
                SFXSource.PlayOneShot(Footstep6);
                break;
            case 6:
                SFXSource.PlayOneShot(Footstep7);
                break;
            case 7:
                SFXSource.PlayOneShot(Footstep8);
                break;
            case 8:
                SFXSource.PlayOneShot(Footstep9);
                break;
            case 9:
                SFXSource.PlayOneShot(Footstep10);
                break;
            case 10:
                SFXSource.PlayOneShot(Footstep11);
                break;
            case 11:
                SFXSource.PlayOneShot(Footstep12);
                break;
            case 12:
                SFXSource.PlayOneShot(Footstep13);
                break;
            case 13:
                SFXSource.PlayOneShot(Footstep14);
                break;
            case 14:
                SFXSource.PlayOneShot(Footstep15);
                break;
            case 15:
                SFXSource.PlayOneShot(Footstep16);
                break;
            case 16:
                SFXSource.PlayOneShot(Footstep17);
                break;
            case 17:
                SFXSource.PlayOneShot(Footstep18);
                break;
            case 18:
                SFXSource.PlayOneShot(Footstep19);
                break;
            case 19:
                SFXSource.PlayOneShot(Footstep20);
                break;
            case 20:
                SFXSource.PlayOneShot(Footstep21);
                break;
        }
    }

    public void PlayUseItemSound()
    {
        SFXSource.PlayOneShot(UseItem);
    }

    public void PlayPickItemSound()
    {
        SFXSource.PlayOneShot(PickItem);
    }

    public void PlayDoorOpenSound()
    {
        SFXSource.PlayOneShot(DoorOpen);
    }

    public void PlayPistolEquip()
    {
        SFXSource.PlayOneShot(PistolEquip);
    }

    public void PlayPistolShot()
    {
        SFXSource.PlayOneShot(PistolShot);
    }

    public void PlayPistolMagOut()
    {
        SFXSource.PlayOneShot(PistolMagOut);
    }

    public void PlayPistolMagIn()
    {
        SFXSource.PlayOneShot(PistolMagIn);
    }

    public void PlayPistolSlideBack()
    {
        SFXSource.PlayOneShot(PistolSlideBack);
    }

    public void PlayPistolSlideForward()
    {
        SFXSource.PlayOneShot(PistolSlideForward);
    }

    public void PlayPistolEmpty()
    {
        SFXSource.PlayOneShot(PistolEmpty);
    }

    public void PlayRifleShot()
    {
        SFXSource.PlayOneShot(RifleShot);
    }

    public void PlayRifleFinishReload()
    {
        SFXSource.PlayOneShot(RifleFinishReload);
    }
    public void PlayRifleMagOut()
    {
        SFXSource.PlayOneShot(RifleMagOut);
    }

    public void PlayRifleMagIn()
    {
        SFXSource.PlayOneShot(RifleMagIn);
    }

    public void PlayRifleSlideBack()
    {
        SFXSource.PlayOneShot(RifleSlideBack);
    }

    public void PlayRifleSlideForward()
    {
        SFXSource.PlayOneShot(RifleSlideForward);
    }


    public void PlayRifleEmpty()
    {
        SFXSource.PlayOneShot(RifleEmpty);
    }

    public void PlayShotgunEquip()
    {
        SFXSource.PlayOneShot(ShotgunEquip);
    }

    public void PlayShotgunShellIn()
    {
        SFXSource.PlayOneShot(ShotgunShellIn);
    }

    public void PlayShotgunShot()
    {
        SFXSource.PlayOneShot(ShotgunShot);
    }

    public void PlayShotgunEmpty()
    {
        SFXSource.PlayOneShot(ShotgunEmpty);
    }

    public void PlayMagnumEquip()
    {
        SFXSource.PlayOneShot(MagnumEquip);
    }

    public void PlayMagnumShot()
    {
        SFXSource.PlayOneShot(MagnumShot);
    }

    public void PlayMagnumReload()
    {
        SFXSource.PlayOneShot(MagnumReload);
    }

    public void PlayMagnumEmpty()
    {
        SFXSource.PlayOneShot(MagnumEmpty);
    }

    public void PlayFashbangSound()
    {
        SFXSource.PlayOneShot(FlashbangSound);
    }

    public void PlayEarRingingSound()
    {
        SFXSource.PlayOneShot(EarRingingSound);
    }

    public void PlayHandgrenadeSound()
    {
        SFXSource.PlayOneShot(HandgrenadeSound);
    }

    public void PlayLockedDoorSound()
    {
        SFXSource.PlayOneShot(LockedDoorSound);
    }

    public void PlayErrorSound()
    {
        SFXSource.PlayOneShot(ErrorSound);
    }

    public void PlayBuySound()
    {
        SFXSource.PlayOneShot(BuySound);
    }

    public void PlayDeathMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBetweenMusic(DeathMusic, 1.5f));
    }

    public void PlayCreditsMusic()
    {
        LoopedMusicSource.clip = CreditsMusic;
        LoopedMusicSource.Play();
    }

    public void OpenStore()
    {
        if (!jackFightStarted)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBetweenMusic(MerchantMusic, 1.5f));
        }
    }

    public void CloseStore()
    {
        if (!jackFightStarted)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBetweenMusic(SaferoomMusic, 1.5f));
        }
    }

    public void EnterSuspenseArea()
    {
        if (!jackFightStarted)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBetweenMusic(SuspenseMusic, 1.5f));
        }
    }

    public void EnterEnemyArea()
    {
        if (!jackFightStarted)
        {
            StopAllCoroutines();
            StartCoroutine(FadeBetweenMusic(ChaseMusic, 0.5f));
        }
    }

    public void LeaveEnemeyArea()
    {
        if (!jackFightStarted) { 
            StopAllCoroutines();
            StartCoroutine(FadeBetweenMusic(SaferoomMusic, 2.5f));
        }
    }

    public void PlayCheatSound()
    {
        SFXSource.PlayOneShot(CheatSound);
    }

    public IEnumerator FadeBetweenMusic(AudioClip newClip, float fadeTime)
    {
        float timeElapsed = 0;

        if(newClip == SaferoomMusic)
        {
            while (timeElapsed < fadeTime)
            {
                LoopedMusicSource.volume = Mathf.Lerp(0.25f, 0, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            LoopedMusicSource.Stop();
            LoopedMusicSource.clip = newClip;
            LoopedMusicSource.Play();
            LoopedMusicSource.volume = 0.25f;
        }

        else { 
            fadeTime /= 2;
            while (timeElapsed < fadeTime)
            {
                LoopedMusicSource.volume = Mathf.Lerp(0.25f, 0, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            LoopedMusicSource.Stop();
            timeElapsed = 0;
            LoopedMusicSource.clip = newClip;
            LoopedMusicSource.Play();

            fadeTime *= 2;
            while (timeElapsed < fadeTime)
            {
                LoopedMusicSource.volume = Mathf.Lerp(0, 0.25f, timeElapsed / fadeTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void PlayMaleAlertedSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 11);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(MaleAlerted1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(MaleAlerted2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(MaleAlerted3);
                break;
            case 3:
                EnemyAudioSource.PlayOneShot(MaleAlerted4);
                break;
            case 4:
                EnemyAudioSource.PlayOneShot(MaleAlerted5);
                break;
            case 5:
                EnemyAudioSource.PlayOneShot(MaleAlerted6);
                break;
            case 6:
                EnemyAudioSource.PlayOneShot(MaleAlerted7);
                break;
            case 7:
                EnemyAudioSource.PlayOneShot(MaleAlerted8);
                break;
            case 8:
                EnemyAudioSource.PlayOneShot(MaleAlerted9);
                break;
            case 9:
                EnemyAudioSource.PlayOneShot(MaleAlerted10);
                break;
            case 10:
                EnemyAudioSource.PlayOneShot(MaleAlerted11);
                break;
        }
    }

    public void PlayJackSoundtrack()
    {
        LoopedMusicSource.clip = JackMusic;
        LoopedMusicSource.Play();
        jackFightStarted = true;
    }

    public void JackDefeated()
    {
        jackFightStarted = false;
        LoopedMusicSource.Stop();
        StopAllCoroutines();
        StartCoroutine(FadeBetweenMusic(SaferoomMusic, 0.5f));
    }

    public void PlayPauseSoundtrack()
    {
        PauseMenuSource.clip = PauseMusic;
        PauseMenuSource.Play();
        LoopedMusicSource.Pause();
        MusicSource.Pause();
        VoiceSource.Pause();
    }

    public void Unpause()
    {
        PauseMenuSource.Stop();
        LoopedMusicSource.UnPause();
        MusicSource.UnPause();
        VoiceSource.UnPause();
    }

    public void PlayFemaleAlertedSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(FemaleAlerted1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(FemaleAlerted2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(FemaleAlerted3);
                break;
        }
    }

    public void PlayMaleEnemyIdleSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(MaleEnemyIdle1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(MaleEnemyIdle2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(MaleEnemyIdle3);
                break;
            case 3:
                EnemyAudioSource.PlayOneShot(MaleEnemyIdle4);
                break;
        }
    }

    public void PlayMaleEnemyHitSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(MaleHit1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(MaleHit2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(MaleHit3);
                break;
            case 3:
                EnemyAudioSource.PlayOneShot(MaleHit4);
                break;
        }
    }

    public void PlayFemaleEnemyHitSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(FemaleHit1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(FemaleHit2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(FemaleHit3);
                break;
            case 3:
                EnemyAudioSource.PlayOneShot(FemaleHit4);
                break;
        }
    }

    public void PlayMaleEnemyDeathSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(MaleDeath1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(MaleDeath2);
                break;
            case 2:
                EnemyAudioSource.PlayOneShot(MaleDeath3);
                break;
            case 3:
                EnemyAudioSource.PlayOneShot(MaleDeath4);
                break;
        }
    }

    public void PlayFemaleEnemyDeathSound(AudioSource EnemyAudioSource)
    {
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                EnemyAudioSource.PlayOneShot(FemaleDeath1);
                break;
            case 1:
                EnemyAudioSource.PlayOneShot(FemaleDeath2);
                break;
        }
    }

    public void PlayGoldPickupSound()
    {
        SFXSource.PlayOneShot(GoldSound);
    }

    public void PlayTreasurePickupSound()
    {
        SFXSource.PlayOneShot(TreasureSound);
    }

    public void PlayInapplicableSound()
    {
        throw new System.NotImplementedException();
    }
}

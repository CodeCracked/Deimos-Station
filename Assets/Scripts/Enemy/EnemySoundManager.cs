using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    public AudioSource Source;

    public void PlaySound(AudioClipPool sound, float maxDistance, float volume)
    {
        Source.maxDistance = maxDistance;
        Source.volume = volume;
        Source.PlayOneShot(sound.Random);
    }
}
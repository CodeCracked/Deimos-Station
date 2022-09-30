using System.Collections;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
	public AudioClipPool Sound;
	public AudioSource Source;
    public PlayerMotor PlayerMotor;
    [Range(0.0f, 2.0f)] public float Interval = 0.5f;

    private Coroutine _coroutine;

    public void OnEnable()
    {
        _coroutine = StartCoroutine(Footsteps_Coroutine());
    }
    public void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator Footsteps_Coroutine()
    {
        while (true)
        {
            if (PlayerMotor.OnGround && PlayerMotor.Moving) Source.PlayOneShot(Sound.Random);
            yield return new WaitForSeconds(Interval);
        }
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip Pool", menuName = "Custom/Audio Clip Pool")]
public class AudioClipPool : ScriptableObject
{
	public AudioClip[] Pool;
	public AudioClip Random { get { return Pool[UnityEngine.Random.Range(0, Pool.Length)]; } }
}
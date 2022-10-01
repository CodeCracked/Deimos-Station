using UnityEngine;

public class FootstepEmitter : MonoBehaviour
{
    public AudioSource FootstepSource;
    public AudioClipPool Footsteps;
    public float VelocityPerFootstep = 2.5f;

    private Vector2 _previousPosition;
    private float _timer;

    public void Awake()
    {
        _previousPosition = new(transform.position.x, transform.position.z);
    }

    public void Update()
    {
        Vector2 position = new(transform.position.x, transform.position.z);
        float velocity = (position - _previousPosition).magnitude;
        _previousPosition = position;

        _timer += velocity / VelocityPerFootstep;
        if (_timer >= 1.0f)
        {
            FootstepSource.PlayOneShot(Footsteps.Random);
            _timer = 0;
        }
    }
}

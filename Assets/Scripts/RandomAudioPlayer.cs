using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private bool _randomizePitch;
    [SerializeField] private float _pitchRandomRange = 0.2f;
    [SerializeField] private float _playDelay = 0;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        int randomIndex = Random.Range(0, _audioClips.Length);

        _audioSource.pitch = _randomizePitch ? Random.Range(1.0f - _pitchRandomRange, 1.0f + _pitchRandomRange) : 1.0f;
        _audioSource.clip = _audioClips[randomIndex];
        _audioSource.PlayDelayed(_playDelay);
    }
}

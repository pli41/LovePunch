using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour {


    public static void PlaySound( AudioSource _source, AudioClip _sound)
    {       
        if (_source != null && _sound != null )
        {
            _source.Stop();
            _source.clip = _sound;
            _source.Play();
        }
    }

    public static void PlayRandomSound(AudioSource _source, AudioClip[] _sounds)
    {
        AudioClip _sound = _sounds[(int)Random.Range(0, _sounds.Length-1)];
        if (_source != null && _sound != null)
        {
            _source.Stop();
            _source.clip = _sound;
            _source.Play();
        }
    }
    
}

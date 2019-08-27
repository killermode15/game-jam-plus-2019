using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Valve.VR.InteractionSystem;

[System.Serializable]
public class AudioData
{
    public string Identifier;

    [HideIf("UseSnapshot")]
    public bool UseListOfSound = false;

    public bool UseSnapshot = false;

    [ShowIf("UseListOfSound")]
    public List<AudioClip> AudioClips;
    [HideIf("UseSnapshot"), HideIf("UseListOfSound")]
    public AudioClip AudioClip;
    [ShowIf("UseSnapshot")]
    public AudioMixerSnapshot MixerSnapshot;

    public AudioSource PlayAudio(bool play = true, bool destroyAfter = false, bool randomPitch = false)
    {
        if (!UseSnapshot)
        {
            AudioSource source = new GameObject("Source").AddComponent<AudioSource>();

            if (UseListOfSound)
            {
                if (AudioClips.Count == 0)
                {
                    Debug.LogWarning("List of sounds is empty.");
                    GameObject.Destroy(source.gameObject);
                    return null;
                }

                int idx = Random.Range(0, AudioClips.Count);
                source.clip = AudioClips[idx];
            }
            else
            {
                source.clip = AudioClip;
            }

            source.gameObject.name = "[GENERATED]" + source.name;
            if (play)
            {
                source.Play();
            }

            if (destroyAfter)
            {
                GameObject.Destroy(source.gameObject, source.clip.length);
            }

            if (randomPitch)
            {
                float curPitch = source.pitch;
                source.pitch = curPitch + Random.Range(-0.25f, 0.25f);
            }

            return source;
        }
        else
        {
            Debug.LogWarning("Using PlayAudio() while Use Snapshot is true");
            return null;
        }
    }

    public void PlaySnapshot(float timeToReach)
    {
        if (UseSnapshot)
        {
            MixerSnapshot.TransitionTo(timeToReach);
        }
        else
        {
            Debug.LogWarning("Using PlaySnapshot() while Use Snapshot is false");
        }
    }
}

[CreateAssetMenu(fileName = "Audio Manager")]
public class AudioManager : ScriptableObject
{
    public List<AudioData> AudioData;

    public AudioData GetAudio(string identifier)
    {
        return AudioData.Find(x => x.Identifier.ToLower() == identifier.ToLower());
    }
}
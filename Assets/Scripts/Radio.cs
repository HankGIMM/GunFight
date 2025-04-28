using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Radio : MonoBehaviour
{
    public AudioMixerSnapshot defaultSnapshot; // Default audio snapshot
    public AudioMixerSnapshot radioActiveSnapshot; // Snapshot when near the radio
    public float transitionTime = 1.0f; // Time to transition between snapshots

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Switch to the radio active snapshot
            radioActiveSnapshot.TransitionTo(transitionTime);
            Debug.Log("Player entered radio range. Switching to RadioActive snapshot.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Switch back to the default snapshot
            defaultSnapshot.TransitionTo(transitionTime);
            Debug.Log("Player left radio range. Switching to Default snapshot.");
        }
    }
}

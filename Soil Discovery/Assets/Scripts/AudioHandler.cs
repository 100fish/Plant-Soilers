using UnityEngine;
using FMODUnity;

public class AudioHandler : MonoBehaviour
{
    PathFollow MovementScript;
    StudioEventEmitter emitter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emitter = gameObject.GetComponent<StudioEventEmitter>();
        MovementScript = gameObject.GetComponent<PathFollow>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FadeOutSound()
    {
        emitter.SetParameter("LoopGravel", 1f);
    }

    public void FadeInSound()
    {
        emitter.Stop();
        emitter.SetParameter("LoopGravel", 0f);
        emitter.Play();
    }

    public void StopSoundImmediate()
    {

    }
}

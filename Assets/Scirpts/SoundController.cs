using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;

    public AudioSource audioSource;
    [SerializeField]
    private AudioClip jumpAudio, landAudio, moveAudio, hurtAudio, collectAudio, wallJumpAudio;

    private void Awake()
    {
        instance = this;
    }

    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();
    }

    public void LandAudio()
    {
        audioSource.clip = landAudio;
        audioSource.Play();
    }

    public void MoveAudio()
    {
        audioSource.clip = moveAudio;
        audioSource.Play();
    }

    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();
    }

    public void CollectAudio()
    {
        audioSource.clip = collectAudio;
        audioSource.Play();
    }

    public void WallJumpAudio()
    {
        audioSource.clip = wallJumpAudio;
        audioSource.Play();
    }
}

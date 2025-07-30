using System;
using UnityEngine;
using UnityEngine.Audio;

/**
 * 推进器音频控制
 */
public class BoosterAudio : MonoBehaviour
{
    private AudioSource audioSource; // 音频源

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 获取音频源组件
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //推进器显示了
        PlayAudio(); // 播放音频
    }

    private void OnDisable()
    {
        //推进器隐藏了
        StopAudio(); // 停止音频
    }

    private void PlayAudio()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    private void PauseAudio()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    private void ResumeAudio()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause(); // 恢复暂停
    }
}

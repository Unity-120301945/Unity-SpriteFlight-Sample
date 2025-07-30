using System;
using UnityEngine;

/**
 * 飞船被毁灭时播放音频
 */
public class PlaySoundOnDestroy : MonoBehaviour
{
    private GameObject audioPrefab;// 被毁灭音频预制件
    private AudioSource audioSource; // 被毁灭音频源

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 查找场景中被毁灭音频预制件
        audioPrefab = GameObject.FindWithTag("DestroyAudio");
        audioSource = audioPrefab.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        // 当飞船被销毁时，播放被毁灭音频
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found on the DestroyAudio prefab.");
            return;
        }
        PlayAudio();
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

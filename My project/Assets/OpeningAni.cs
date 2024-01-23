using UnityEngine;
using UnityEngine.Video;

public class OpeningAni : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 用于播放开场动画的VideoPlayer组件
    public AudioSource audioSource; // 用于播放背景音乐的AudioSource组件

    void Start()
    {
        // 暂停背景音乐
        audioSource.Pause();

        // 开始播放开场动画
        videoPlayer.Play();

        // 当开场动画播放结束时，恢复背景音乐，并禁用播放视频的游戏对象
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // 恢复背景音乐
        audioSource.Play();

        // 禁用播放视频的游戏对象
        videoPlayer.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liluo.BiliBiliLive;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Video;

public class GiftAni : MonoBehaviour
{
    public AudioSource bgm; // 游戏的背景音乐
    public VideoPlayer cutscenePlayer; // 用于播放过场动画的VideoPlayer组件
    public GameObject screen; // 用于显示过场动画的游戏对象

    // ...

    public async void GetGift(BiliBiliLiveGiftData data)
    {
        Debug.Log($"<color=#FEA356>礼物</color> 用户名: {data.username}, 礼物名: {data.giftName}, 数量: {data.num}, 总价: {data.total_coin}");

        // 收到礼物，播放过场动画
        cutscenePlayer.Play();
        // 暂停背景音乐
        bgm.Pause();
        cutscenePlayer.loopPointReached += OnVideoEnded;

    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // 恢复背景音乐
        bgm.Play();

        // 禁用播放视频的游戏对象
        cutscenePlayer.gameObject.SetActive(false);
    }
}





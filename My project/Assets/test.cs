using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Liluo.BiliBiliLive;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class test : MonoBehaviour
{
    public Image img;
    public int RoomID;
    public string danmuContent;
    IBiliBiliLiveRequest req;


    public AudioSource bgm; // 游戏的背景音乐
    public VideoPlayer cutscenePlayer; // 用于播放过场动画的VideoPlayer组件
    public GameObject screen; // 用于显示过场动画的游戏对象

    async void Start()
    {
        // 创建一个监听对象
        req = await BiliBiliLive.Connect(RoomID);
        req.OnDanmuCallBack += GetDanmu;
        req.OnGiftCallBack += GetGift;
        req.OnSuperChatCallBack += GetSuperChat;
        bool flag = true;
        req.OnRoomViewer += number =>
        {
        	// 仅首次显示
            if (flag) Debug.Log($"当前房间人数为: {number}");
        };

        Loom.Initialize();
    }

    /// <summary>
    /// 接收到礼物的回调
    /// </summary>
    public async void GetGift(BiliBiliLiveGiftData data)
    {
        Debug.Log($"<color=#FEA356>礼物</color> 用户名: {data.username}, 礼物名: {data.giftName}, 数量: {data.num}, 总价: {data.total_coin}");

        cutscenePlayer.gameObject.SetActive(true);
        // 收到礼物，播放过场动画
        cutscenePlayer.Play();
        // 暂停背景音乐
        bgm.Pause();
        cutscenePlayer.loopPointReached += OnVideoEnded;
        //img.sprite = await BiliBiliLive.GetHeadSprite(data.userId);
    }
    void OnVideoEnded(VideoPlayer vp)
    {
        // 恢复背景音乐
        bgm.Play();

        // 禁用播放视频的游戏对象
        cutscenePlayer.gameObject.SetActive(false);
    }

    /// <summary>
    /// 接收到弹幕的回调
    /// </summary>
    public delegate void AUpdatedEventHandler(string newValue);
    public event AUpdatedEventHandler OnAUpdated;
    public async void GetDanmu(BiliBiliLiveDanmuData data)
    {
        Debug.Log($"<color=#60B8E0>弹幕</color> 用户名: {data.username}, 内容: {data.content}, 舰队等级: {data.guardLevel}");
        //danmuContent = data.content;
        Loom.QueueOnMainThread(() => { GameObject.FindObjectOfType<ChatScript>().SendDataFromBlibili(data.username); });
        Loom.QueueOnMainThread(()=> { GameObject.FindObjectOfType<ChatScript>().SendDataFromBlibili(data.content); });
        Loom.QueueOnMainThread(() => { FindObjectOfType<DanmuTextSpawner>().SpawnText(data.content); });
        //img.sprite = await BiliBiliLive.GetHeadSprite(data.userId);


        // 生成3D文字块
        //DanmuTextSpawner textSpawner = FindObjectOfType<DanmuTextSpawner>();
       
    }

    /// <summary>
    /// 接收到SC的回调
    /// </summary>
    public async void GetSuperChat(BiliBiliLiveSuperChatData data)
    {
        Debug.Log($"<color=#FFD766>SC</color> 用户名: {data.username}, 内容: {data.content}, 金额: {data.price}");
        img.sprite = await BiliBiliLive.GetHeadSprite(data.userId);
    }

    private void OnApplicationQuit()
    {
        req.DisConnect();
    }
}

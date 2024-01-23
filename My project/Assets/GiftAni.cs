using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liluo.BiliBiliLive;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Video;

public class GiftAni : MonoBehaviour
{
    public AudioSource bgm; // ��Ϸ�ı�������
    public VideoPlayer cutscenePlayer; // ���ڲ��Ź���������VideoPlayer���
    public GameObject screen; // ������ʾ������������Ϸ����

    // ...

    public async void GetGift(BiliBiliLiveGiftData data)
    {
        Debug.Log($"<color=#FEA356>����</color> �û���: {data.username}, ������: {data.giftName}, ����: {data.num}, �ܼ�: {data.total_coin}");

        // �յ�������Ź�������
        cutscenePlayer.Play();
        // ��ͣ��������
        bgm.Pause();
        cutscenePlayer.loopPointReached += OnVideoEnded;

    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // �ָ���������
        bgm.Play();

        // ���ò�����Ƶ����Ϸ����
        cutscenePlayer.gameObject.SetActive(false);
    }
}





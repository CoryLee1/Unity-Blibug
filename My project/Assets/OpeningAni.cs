using UnityEngine;
using UnityEngine.Video;

public class OpeningAni : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ���ڲ��ſ���������VideoPlayer���
    public AudioSource audioSource; // ���ڲ��ű������ֵ�AudioSource���

    void Start()
    {
        // ��ͣ��������
        audioSource.Pause();

        // ��ʼ���ſ�������
        videoPlayer.Play();

        // �������������Ž���ʱ���ָ��������֣������ò�����Ƶ����Ϸ����
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        // �ָ���������
        audioSource.Play();

        // ���ò�����Ƶ����Ϸ����
        videoPlayer.gameObject.SetActive(false);
    }
}

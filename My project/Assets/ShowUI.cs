using System.Collections;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public GameObject uiElement; // ��Ҫ��ʾ�����ص�UIԪ��

    // ʹ��Э��������ʱ��
    IEnumerator Start()
    {
        while (true)
        {
            // �ȴ�6����
            yield return new WaitForSeconds(4 * 60);

            // ��ʾUIԪ��
            uiElement.SetActive(true);

            // �ȴ�10��
            yield return new WaitForSeconds(10);

            // ����UIԪ��
            uiElement.SetActive(false);
        }
    }
}

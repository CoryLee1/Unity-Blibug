using System.Collections;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public GameObject uiElement; // 需要显示和隐藏的UI元素

    // 使用协程来处理时间
    IEnumerator Start()
    {
        while (true)
        {
            // 等待6分钟
            yield return new WaitForSeconds(4 * 60);

            // 显示UI元素
            uiElement.SetActive(true);

            // 等待10秒
            yield return new WaitForSeconds(10);

            // 隐藏UI元素
            uiElement.SetActive(false);
        }
    }
}

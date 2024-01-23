using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Liluo.BiliBiliLive;
using UnityEngine.SceneManagement;

public class ChatScript : MonoBehaviour
{
    public Image img;
    public int RoomID;
    public string danmuContent;
    IBiliBiliLiveRequest req;
    public static ChatScript Instance;

    float curTime = 0;
    private void Awake()
    {
        Instance = this;
    }

    //API key
    [SerializeField]private string m_OpenAI_Key= "填写你的Key";
    // 定义Chat API的URL
    //private string m_ApiUrl = "https://api.openai.com/v1/completions";
    private string m_ApiUrl = "https://cory.dorakoch.com/v1/completions";
    //配置参数
    [SerializeField]private GetOpenAI.PostData m_PostDataSetting;
    //聊天UI层
    [SerializeField]private GameObject m_ChatPanel;
    //输入的信息
    [SerializeField]private InputField m_InputWord;
    //返回的信息
    [SerializeField]private Text m_TextBack;
    //播放设置
    [SerializeField]private Toggle m_PlayToggle;
    /// <summary>
    /// 百度语音识别
    /// </summary>
    [SerializeField]private BaiduSpeechSample m_SpeechSample;

    //gpt-3.5-turbo
    [SerializeField] public GptTurboScript m_GptTurboScript;


    private test m_TestInstance;

    public BlibugController bc;
    private void Start()
    {
        test m_TestInstance = FindObjectOfType<test>();
        m_PostDataSetting.prompt = m_TestInstance.danmuContent + m_lan + " ";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendDataFromBlibili("敲敲机子");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SendDataFromBlibili("打游戏");
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }


        if(bc.isAwake)
        {
            curTime += Time.deltaTime;
            if (!m_WriteState && curTime >= 10f)
            {
                string _msg = "Current Tone：" + bc.tempPromote + "现在开始直播讲讲故事吧，Blibug！" + m_lan+"开始直播吧，Blibug，分享一些最近的故事和下班后的情况吧（一定要按你说过的故事把话题聊深入哦），可以念弹幕用户ID和内容哦";
                foreach (string s in m_ChatHistory)//insert the chathistory to affect the outputs
                {
                    Debug.Log(s);
                    _msg += " " + s;
                    Debug.Log(_msg);

                }

                Debug.Log("开始请求OpenAI！！！");
                curTime = 0;
                StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));
            }
            else
            { 

            }
        }
    }

    public void SendData(string _data)
    {
        if (_data.Equals(""))
            return;

        // 记录聊天
      
        m_ChatHistory.Add(_data);
        Debug.Log(m_ChatHistory);

        string _msg = m_PostDataSetting.prompt + m_lan + " " + _data+ danmuContent;
        Debug.Log(_msg+"开始请求OpenAI！！！");
        StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));
        m_TextBack.text = "...";
    }

    //发送信息
    public void SendData()
    {
        if(m_InputWord.text.Equals(""))
            return;

        //记录聊天
        m_ChatHistory.Add(m_InputWord.text);

        string _msg=m_PostDataSetting.prompt+m_lan+" "+m_InputWord.text;
        Debug.Log(_msg);
        //string _msg =m_lan + " " + m_InputWord.text;
        //发送数据
        //StartCoroutine (GetPostData (_msg,CallBack));
        StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));

        m_InputWord.text="";
        m_TextBack.text="...";

        
    }

    public void SendDataFromBlibili(string danmucontent)
    {
        if (danmucontent.Equals(""))
            return;
      
        if(danmucontent.Contains("敲敲机子") || danmucontent.Contains("敲敲机子"))
        {
            Debug.Log("我醒了！");
            bc.WakeUp();
            m_WriteState = true;
            StartCoroutine(SetTextPerWord("埃?埃?我在哪?? ?啊啊，你好啊。。。 我是Blibug"));
        }
        else if (bc.isAwake)
        {
            bc.OnBulletHit();
            m_PostDataSetting.prompt = bc.bilipromote;
            //记录聊天

            if (m_ChatHistory.Count >= 5)
            {
                m_ChatHistory.Clear();
            }
            m_ChatHistory.Add(danmucontent);
            Debug.Log("对话历史更新了："+m_ChatHistory);

           
            //string _msg =m_lan + " " + m_InputWord.text;
            //发送数据
            //StartCoroutine (GetPostData (_msg,CallBack));
            //if (!m_WriteState && curTime >=8f)
            //{
            //    string _msg = m_PostDataSetting.prompt + m_lan;
            //    foreach (string s in m_ChatHistory)
            //    {
            //        _msg += " " + s;
            //    }

            //    Debug.Log("开始请求OpenAI！！！");
            //    curTime = 0;
            //    StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));
            //}
            //else
            //{
              
            //}


            m_InputWord.text = "";
            m_TextBack.text = "...";
        }



    }

    //public void SendDataFromBlibili(string danmucontent)
    //{
    //    if (danmucontent.Equals(""))
    //        return;

    //    if (danmucontent.Contains("敲敲机子") || danmucontent.Contains("敲敲机子"))
    //    {
    //        Debug.Log("我醒了！");
    //        bc.WakeUp();
    //        m_WriteState = true;
    //        StartCoroutine(SetTextPerWord("埃?埃?我在哪?? ?啊啊，你好啊。。。 我是Blibug"));
    //    }
    //    else if (bc.isAwake && (danmucontent.Contains("@Blibug") || danmucontent.Contains("@blibug")))
    //    {
    //        bc.OnBulletHit();
    //        m_PostDataSetting.prompt = bc.bilipromote;
    //        //记录聊天
    //        m_ChatHistory.Add(danmucontent);

    //        string _msg = m_PostDataSetting.prompt + m_lan + " " + danmucontent;
    //        //string _msg =m_lan + " " + m_InputWord.text;
    //        //发送数据
    //        //StartCoroutine (GetPostData (_msg,CallBack));
    //        if (m_WriteState)
    //        {

    //        }
    //        else
    //        {
    //            Debug.Log("开始请求OpenAI！！！");
    //            StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));
    //        }


    //        m_InputWord.text = "";
    //        m_TextBack.text = "...";
    //    }


    //    if (!bc.isAwake)
    //        return;






    //}
    //发送信息
    //   public void SendData(string _data)
    //   {
    //记录聊天
    //      m_ChatHistory.Add(_data);

    //       string _msg = m_PostDataSetting.prompt + m_lan + " " + _data;
    //string _msg =m_lan + " " + m_InputWord.text;
    //发送数据
    //StartCoroutine (GetPostData (_msg,CallBack));
    //       StartCoroutine(m_GptTurboScript.GetPostData(_msg, m_OpenAI_Key, CallBack));

    //m_InputWord.text = "";
    //       m_TextBack.text = "...";


    //   }


    //AI回复的信息
    private void CallBack(string _callback){
        _callback=_callback.Trim();
        m_TextBack.text="";
        //开始逐个显示返回的文本

        if(!m_WriteState)
        {
            m_WriteState = true;
            StartCoroutine(SetTextPerWord(_callback));
            //记录聊天
            m_ChatHistory.Add(_callback);

            if (m_PlayToggle.isOn)
            {
                StartCoroutine(Speek(_callback));
            }
        }
       

       
       

    }


    private IEnumerator Speek(string _msg){
        yield return new WaitForEndOfFrame();
        //播放合成并播放音频
        m_SpeechSample.Speek(_msg);
    }

    //查设置
	private IEnumerator GetPostData(string _postWord,System.Action<string> _callback)
	{
        using(UnityWebRequest request = new UnityWebRequest (m_ApiUrl, "POST")){   
        GetOpenAI.PostData _postData = new GetOpenAI.PostData
		{
			model = m_PostDataSetting.model,
			prompt = _postWord,
            //max_tokens = m_PostDataSetting.max_tokens,
            max_tokens = 60,
            temperature =m_PostDataSetting.temperature,
            top_p=m_PostDataSetting.top_p,
            frequency_penalty=m_PostDataSetting.frequency_penalty,
            presence_penalty=m_PostDataSetting.presence_penalty,
            stop=m_PostDataSetting.stop
		};

		string _jsonText = JsonUtility.ToJson (_postData);
		byte[] data = System.Text.Encoding.UTF8.GetBytes (_jsonText);
		request.uploadHandler = (UploadHandler)new UploadHandlerRaw (data);
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer ();

		request.SetRequestHeader ("Content-Type","application/json");
		request.SetRequestHeader("Authorization",string.Format("Bearer {0}",m_OpenAI_Key));

		yield return request.SendWebRequest ();

		if (request.responseCode == 200) {
			string _msg = request.downloadHandler.text;
			GetOpenAI.TextCallback _textback = JsonUtility.FromJson<GetOpenAI.TextCallback> (_msg);
			if (_textback!=null && _textback.choices.Count > 0) {
                    
                string _backMsg=Regex.Replace(_textback.choices [0].text, @"[\r\n]", "").Replace("？","");
                _callback(_backMsg);
			}
		
		}
        }

		
	}


    #region 文字逐个显示
    //逐字显示的时间间隔
    [SerializeField]private float m_WordWaitTime=0.2f;
    //是否显示完成
    [SerializeField]private bool m_WriteState=false;
    private IEnumerator SetTextPerWord(string _msg){
        int currentPos=0;
        while(m_WriteState){
            yield return new WaitForSeconds(m_WordWaitTime);
            currentPos++;
            //更新显示的内容
            m_TextBack.text=_msg.Substring(0,currentPos);

            m_WriteState=currentPos<_msg.Length;
            if(!m_WriteState)
            {
                bc.GoDefaultState();
            }

        }
    }

    #endregion


    #region 聊天记录
    //保存聊天记录
    [SerializeField]private List<string> m_ChatHistory;
    //缓存已创建的聊天气泡
    [SerializeField]private List<GameObject> m_TempChatBox;
    //聊天记录显示层
    [SerializeField]private GameObject m_HistoryPanel;
    //聊天文本放置的层
    [SerializeField]private RectTransform m_rootTrans;
    //发送聊天气泡
    [SerializeField]private ChatPrefab m_PostChatPrefab;
    //回复的聊天气泡
    [SerializeField]private ChatPrefab m_RobotChatPrefab;
    //滚动条
    [SerializeField]private ScrollRect m_ScroTectObject;
    //获取聊天记录
    public void OpenAndGetHistory(){
        m_ChatPanel.SetActive(false);
        m_HistoryPanel.SetActive(true);

        ClearChatBox();
        StartCoroutine(GetHistoryChatInfo());
    }
    //返回
    public void BackChatMode(){
        m_ChatPanel.SetActive(true);
        m_HistoryPanel.SetActive(false);
    }

    //清空已创建的对话框
    private void ClearChatBox(){
        while(m_TempChatBox.Count!=0){
            if(m_TempChatBox[0]){
                Destroy(m_TempChatBox[0].gameObject);
                m_TempChatBox.RemoveAt(0);
            }
        }
        m_TempChatBox.Clear();
    }

    //获取聊天记录列表
    private IEnumerator GetHistoryChatInfo()
    {

        yield return new WaitForEndOfFrame();

       for(int i=0;i<m_ChatHistory.Count;i++){
        if(i%2==0){
            ChatPrefab _sendChat=Instantiate(m_PostChatPrefab,m_rootTrans.transform);
            _sendChat.SetText(m_ChatHistory[i]);
            m_TempChatBox.Add(_sendChat.gameObject);
            continue;
        }

         ChatPrefab _reChat=Instantiate(m_RobotChatPrefab,m_rootTrans.transform);
        _reChat.SetText(m_ChatHistory[i]);
        m_TempChatBox.Add(_reChat.gameObject);
       }

        //重新计算容器尺寸
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_rootTrans);
        StartCoroutine(TurnToLastLine());
    }

    private IEnumerator TurnToLastLine(){
        yield return new WaitForEndOfFrame();
         //滚动到最近的消息
        m_ScroTectObject.verticalNormalizedPosition=0;
    }


    #endregion


    #region 切换妹子
    //Lo娘
    [SerializeField]private GameObject m_LoGirl;
    [SerializeField]private GameObject m_Girl;
    [SerializeField]private string m_lan="使用中文回答然后开始直播讲个自己的故事吧";
    //
    public void SetLoGirlShowed(GameObject _settingPanel){
        if(!m_LoGirl.activeSelf)
        {
            m_LoGirl.SetActive(true);
            m_Girl.SetActive(false);
        }
        //m_AzurePlayer.SetSound("zh-CN-XiaoyiNeural");

        _settingPanel.SetActive(false);
    }
    //zh-CN-XiaoxiaoNeural
    public void SetGirlShowed(GameObject _settingPanel){
        if(!m_Girl.activeSelf)
        {
            m_LoGirl.SetActive(false);
            m_Girl.SetActive(true);
        }
         //m_AzurePlayer.SetSound("zh-CN-liaoning-XiaobeiNeural");

        _settingPanel.SetActive(false);
    }

    #endregion


}

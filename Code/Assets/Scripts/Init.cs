using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using SA.Android.App;
using CodeStage.AntiCheat.Detectors;
using System.Text;
using UnityEngine.UI;
using Game.Data;
using Newtonsoft.Json.Linq;
using AnyThinkAds.Api;

using TapSDK.Core;
using TapSDK.Login;

public class Init : MonoBehaviour
{
    public enum UILayer
    {
        Top = 0,
        Center,
        Bottom,
    }

    public RuleType RuleType = RuleType.Normal;

    // public Transform LoadingPage;
    private const string BuglyAppIDForAndroid = "abc";

    public GameProcessor Game;

    public Transform Tran_Loading;

    public Text Txt_Memo;
    // public Transform MapRoot;

    public Transform Bottom;
    public Transform Center;
    public Transform Top;


    private Dictionary<UILayer, List<string>> allWindows = new Dictionary<UILayer, List<string>>()
    {
        {
            UILayer.Bottom, new List<string>()
            {
                "Home/View_Battle",
                "Home/View_Bag",
                "Home/View_Skill",
                "Home/View_Forge",
                "Home/View_More",
                "Home/View_TopStatu",
                "Home/View_BottomNavBar",
            }
        },
        {
            UILayer.Center, new List<string>()
            {
                "Window/Defend/Dialog_Defend",
                "Window/Dialog_OfflineExp",
                "Window/Setting/Dialog_Settings",

                "Window/Relic/Dialog_Relic",
                "Window/SoulRing/Dialog_SoulRing",
                "Window/Talent/Dialog_Talent",
                "Window/Fashion/Dialog_Fashion",
                "Home/Bag/Dialog_Attr",
                "Window/Pet/Dialog_Pet",

                "Window/More/Dialog_Mine",

                "Window/Achievement/Dialog_Achievement",
                "Window/Store/Dialog_Store",
            }
        },
        {
            UILayer.Top,  new List<string>()
            {
                "GameItem/Dialog_Detail_Select",
                "GameItem/Detail_Normal",
                "GameItem/Detail_Equip",
                "GameItem/Detail_Equip_Special",
                "GameItem/Dialog_Detail_Pet",

                "Window/Festive/Dialog_FloatButtons",
                "Window/Loading",
                "Window/Dialog_Drop",
                "Window/Dialog_SecondaryConfirmation",
            }
        }
    };

    void Awake()
    {
        DontDestroyOnLoad(this);
        Log.ILog = new ANLogger();
    }

    // Start is called before the first frame update
    void Start()
    {
        //保持屏幕常亮
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Log.Debug("Demo Start()");

        //InitBuglySDK();
        //Log.Debug("Init bugly sdk done");
        //BuglyAgent.SetScene(0);

        //InitTapSDK();

        InitTaku();

        //AsyncTapAccount();

        AsyncStartAsync();
    }

    private void InitTaku()
    {
        string placementId = "";
        //（可选配置）设置自定义的Map信息，可匹配后台配置的广告商顺序的列表（App纬度）
        //注意：调用此方法会清除setChannel()、setSubChannel()方法设置的信息，如果有设置这些信息，请在调用此方法后重新设置
        ATSDKAPI.initCustomMap(new Dictionary<string, string>() { { "unity3d_data", "test_data" } });

        //（可选配置）设置自定义的Map信息，可匹配后台配置的广告商顺序的列表（Placement纬度）
        ATSDKAPI.setCustomDataForPlacementID(new Dictionary<string, string>() { { "unity3d_data_pl", "test_data_pl" } }, placementId);

        //（可选配置）设置渠道的信息，开发者可以通过该渠道信息在后台来区分看各个渠道的广告数据
        //注意：如果有使用initCustomMap()方法，必须在initCustomMap()方法之后调用此方法
        ATSDKAPI.setChannel("unity3d_test_channel");

        //（可选配置）设置子渠道的信息，开发者可以通过该渠道信息在后台来区分看各个渠道的子渠道广告数据
        //注意：如果有使用initCustomMap()方法，必须在initCustomMap()方法之后调用此方法
        ATSDKAPI.setSubChannel("unity3d_test_subchannel");

        //设置开启Debug日志（强烈建议测试阶段开启，方便排查问题）
        ATSDKAPI.setLogDebug(true);

        //（必须配置）SDK的初始化
        ATSDKAPI.initSDK("a6a59c554cc937", "a1bf8be0a390efa79934bd981449f3ec6");//Use your own app_id & app_key here

        Debug.Log("Taku Init Success");
    }


    private async Task AsyncStartAsync()
    {
        this.Tran_Loading.gameObject.SetActive(true);

        long currentTimeSecond = 0;  //最后加载网络时间

        if (ConfigHelper.Channel == ConfigHelper.Channel_Tap)
        {
            //currentTimeSecond = TimeHelper.ClientNowSeconds();

            //Log.Debug("local time:" + currentTimeSecond);

            var timeTaks = TimeCheatingDetector.GetOnlineTimeTask("https://www.baidu.com/");
            await timeTaks;
            currentTimeSecond = (long)timeTaks.Result.onlineSecondsUtc;
        }
        else
        {
            var timeTaks = TimeCheatingDetector.GetOnlineTimeTask("https://www.baidu.com/");
            await timeTaks;
            currentTimeSecond = (long)timeTaks.Result.onlineSecondsUtc;
            Log.Debug("net time:" + currentTimeSecond);
        }

        AppHelper.StartTime = currentTimeSecond;

        this.LoadConfig();  //先加载配置

        User_Data_Manager.Load();  //再加载存档

        //再加载QQ-net数据
        try
        {
            if (ConfigHelper.Channel != ConfigHelper.Channel_Tap && User_Data_Manager.Data.Account != "")
            {
                this.Txt_Memo.text = "加载服务器数据中...";

                StartCoroutine(NetworkHelper.Loading(
                    (WebResultWrapper result) =>
                    {
                        if (result.Code == StatusMessage.OK)
                        {
                            JToken AtrList = result.Extend.SelectToken("LoadingData");
                            User_Data_Manager.NetData = AtrList.ToObject<Loading_Data>();

                            JToken store = result.Extend.SelectToken("StoreData");
                            User_Data_Manager.StoreData = store.ToObject<Store_Data>();

                            StartCoroutine(AsyncLoadWindows(currentTimeSecond));
                        }
                        else
                        {
                            this.Txt_Memo.text = "加载服务器数据失败...";
                            StartCoroutine(AsyncLoadWindows(currentTimeSecond));
                        }

                    },
                     () =>
                     {
                         this.Txt_Memo.text = "加载服务器数据失败...";
                         StartCoroutine(AsyncLoadWindows(currentTimeSecond));
                     }));
            }
            else
            {
                StartCoroutine(AsyncLoadWindows(currentTimeSecond));
            }
        }
        catch (Exception ex)
        {
            this.Txt_Memo.text = "加载服务器数据失败...";
            StartCoroutine(AsyncLoadWindows(currentTimeSecond));
        }

    }

    private void LoadConfig()
    {
        ConfigComponentNew.Load();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator AsyncLoadWindows(long currentTimeSecond)
    {
        //GameObject loadingPage = null;

        var layers = Enum.GetValues(typeof(UILayer));
        foreach (UILayer layer in layers)
        {
            allWindows.TryGetValue(layer, out var windowTypes);
            foreach (var winType in windowTypes)
            {
                var request = Resources.LoadAsync<GameObject>($"Prefab/{winType}");
                yield return request;
                if (request.asset != null)
                {
                    GameObject win = GameObject.Instantiate(request.asset as GameObject);
                    switch (layer)
                    {
                        case UILayer.Bottom:
                            win.transform.SetParent(Bottom, false);
                            break;
                        case UILayer.Center:
                            win.transform.SetParent(Center, false);
                            break;
                        case UILayer.Top:
                            win.transform.SetParent(Top, false);
                            break;
                    }
                    win.transform.localPosition = Vector3.zero;
                    //var isLoading = winType == "Window/Loading";
                    //if (isLoading)
                    //{
                    //    loadingPage = win;
                    //}
                    win.gameObject.SetActive(false);
                }
                else
                {
                    Log.Error($"窗口：{winType}不存在");
                }
            }
        }

        yield return null;
        //loadingPage.gameObject.SetActive(false);

        this.Tran_Loading.gameObject.SetActive(false);

        Game.Init(currentTimeSecond);

        yield return null;
        var mapRoot = GameObject.FindObjectOfType<View_Battle>();

        yield return new WaitForSeconds(1f);
        Game.LoadMap(RuleType.Normal, mapRoot.transform, null);

        Game.LoadMin();
    }



    private IEnumerator IE_DelayAction(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }

    void InitBuglySDK()
    {
        // TODO NOT Required. Set the crash reporter type and log to report
        // BuglyAgent.ConfigCrashReporter (1, 2);

        // TODO NOT Required. Enable debug log print, please set false for release version
#if DEBUG
        BuglyAgent.ConfigDebugMode(true);
#endif
        BuglyAgent.ConfigDebugMode(true);
        // TODO NOT Required. Register log callback with 'BuglyAgent.LogCallbackDelegate' to replace the 'Application.RegisterLogCallback(Application.LogCallback)'
        // BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);

        // BuglyAgent.ConfigDefault ("Bugly", null, "ronnie", 0);

        BuglyAgent.InitWithAppId(BuglyAppIDForAndroid);

        // TODO Required. If you do not need call 'InitWithAppId(string)' to initialize the sdk(may be you has initialized the sdk it associated Android or iOS project),
        // please call this method to enable c# exception handler only.
        BuglyAgent.EnableExceptionHandler();

        // TODO NOT Required. If you need to report extra data with exception, you can set the extra handler
        //        BuglyAgent.SetLogCallbackExtrasHandler (MyLogCallbackExtrasHandler);

        BuglyAgent.PrintLog(LogSeverity.LogInfo, "Init the bugly sdk");
    }

    //private void InitTapSDK()
    //{
    //    var config = new TapConfig.Builder()
    //        .ClientID("hljkzf86szjm1drye1") // 必须，开发者中心对应 Client ID
    //        .ClientToken("wmXYPPVmuLxj71r9FJOEafO5XudtQ3Qry6LjMy0W") // 必须，开发者中心对应 Client Token
    //        .ServerURL("https://hljkzf86.cloud.tds1.tapapis.cn") // 必须，开发者中心 > 你的游戏 > 游戏服务 > 基本信息 > 域名配置 > API
    //        .RegionType(RegionType.CN) // 非必须，CN 表示中国大陆，IO 表示其他国家或地区
    //        .ConfigBuilder();
    //    TapBootstrap.Init(config);


    //}

    //private async Task AsyncTapAccount()
    //{
    //    var currentUser = await TDSUser.GetCurrent();

    //    if (null != currentUser)
    //    {
    //        UserData.tapAccount = currentUser.ObjectId;
    //    }

    //    AsyncStartAsync();
    //}
}

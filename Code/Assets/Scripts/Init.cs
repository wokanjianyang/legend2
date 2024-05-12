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
using System.Text;

public class Init : MonoBehaviour
{
    public enum UILayer
    {
        Top = 0,
        Center,
        Bottom,

        Map,
    }



    [LabelText("战斗模式")]
    public RuleType RuleType = RuleType.Normal;

    [LabelText("加载界面")]
    // public Transform LoadingPage;
    private const string BuglyAppIDForAndroid = "ff5ed4ccb9";

    public GameProcessor Game;

    // public Transform MapRoot;

    public Loading loading;

    public Transform Bottom;
    public Transform Center;
    public Transform Top;

    private Dictionary<UILayer, List<string>> allWindows = new Dictionary<UILayer, List<string>>()
    {
        {
            UILayer.Bottom, new List<string>()
            {
                "View_Map",
                "View_Bag",
                "View_Skill",
                "View_EndlessTower",
                "View_Forge",
                "View_More",
            }
        },
        {
            UILayer.Center, new List<string>()
            {
                "View_TopStatu",
                "View_BottomNavBar",
                "Dialog_Detail_Select",
                "Dialog_Detail",
                "Dialog_EquipDetail",
                "Dialog_Exclusive_Detail",
                "Dialog_Defend",
                "Dialog_OfflineExp",
                "Dialog_Settings",

                "Dialog_SoulRing",
                "Dialog_Achievement",
                "Dialog_Fashion",
                "Dialog_Attr",
            }
        },
        {
            UILayer.Top, new List<string>()
            {
                "Dialog_FloatButtons",
                "Dialog_SecondaryConfirmation",
            }
        },
        {
            UILayer.Map,new List<String>(){
              "Map_EquipCopy",
              "Map_Phantom",
              "Map_BossFamily",
              "Map_AnDian",
              "Map_Defend",
              "Map_HeroPhantom",
              "Map_Infinite",
            }
        }
    };

    void Awake()
    {
        DontDestroyOnLoad(this);
        Log.ILog = new UnityLogger();
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

        //AsyncTapAccount();

        AsyncStartAsync();
    }

    private void AsyncStartAsync()
    {
        loading.SetText("正在加载配置");
        ConfigComponentNew.Load();

        loading.SetText("正在获取网络时间");
        StartCoroutine(NetworkHelper.CheckTime(
          (long time) =>
          {
              UserData.StartTime = time / 1000;

              loading.SetText("获取时间成功，正在加载界面");

              StartCoroutine(AsyncLoadWindows(time));
          },
          () =>
          {
              loading.SetText("获取网络时间失败，请重启");
          }
          ));
    }

    private IEnumerator AsyncLoadWindows(long currentTimeSecond)
    {
        var layers = Enum.GetValues(typeof(UILayer));

        foreach (UILayer layer in layers)
        {
            allWindows.TryGetValue(layer, out List<string> windowTypes);
            foreach (string winName in windowTypes)
            {
                string winPath = "Window";

                switch (layer)
                {
                    case UILayer.Map:
                        winPath = "Map";
                        break;
                }

                ResourceRequest request = Resources.LoadAsync<GameObject>($"Prefab/{winPath}/{winName}");

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
                        case UILayer.Map:
                            win.transform.SetParent(Bottom, false);
                            break;
                    }
                    win.transform.localPosition = Vector3.zero;

                    win.gameObject.SetActive(false);
                }
                else
                {
                    Log.Error($"窗口：Prefab/{winPath}/{winName} 不存在");
                }
            }
        }

        yield return null;
        loading.gameObject.SetActive(false);
        Game.Init(currentTimeSecond);

        yield return null;
        var mapRoot = GameObject.FindObjectOfType<ViewBattleProcessor>();

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum AdStateEnum
    {
        Show = 1, /* �����ʾ */
        Close = 2, /* ���ر� */
        Click = 3, /* ����� */
        VideoComplete = 4, /* ������� */
        SkippedVideo = 5, /* �������� */
        NotSupport = 6, /* ��治֧�� */
        LoadFail = 7, /* ������ʧ�� */
        Reward = 8, /* ������Ƶ���� */
    }
    public enum AdTypeEnum
    {

        Interstitial = 1, //����
        RewardVideo = 2, //������Ƶ
        BannerExpress = 3, //banner
        FullScreenVideo = 4, //ȫ����Ƶ
        NativeExpress = 5, //ԭ��
        Splash = 6, //����
        Draw = 7 //Draw
    }
}

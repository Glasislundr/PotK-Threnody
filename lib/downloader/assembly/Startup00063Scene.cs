// Decompiled with JetBrains decompiler
// Type: Startup00063Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using DeviceKit;
using GameCore;
using Gsc.Device;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Startup00063Scene : MonoBehaviour
{
  [SerializeField]
  private UIRoot uiRoot;

  private void Awake()
  {
    ModalWindow.setupRootPanel(this.uiRoot);
    MypageScene.ClearCache();
    MypageRootMenu.ClearCache();
    GuildUtil.ClearCache();
    MasterDataCache.CacheClear();
    Singleton<ResourceManager>.GetInstance().ClearPathCache();
    OnDemandDownload.InitVariable();
    Singleton<ResourceManager>.GetInstance().ClearCache();
    Bootstrap.RebootGSCC();
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<NGSceneManager>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<CommonRoot>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<TutorialRoot>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<NGSoundManager>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<ExploreSceneManager>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<ExploreLotteryCore>.GetInstanceOrNull());
    Startup00063Scene.destroyIfExists((MonoBehaviour) Singleton<ExploreDataManager>.GetInstanceOrNull());
  }

  private static void destroyIfExists(MonoBehaviour instance)
  {
    if (!Object.op_Inequality((Object) instance, (Object) null))
      return;
    ((Component) instance).gameObject.SingletonDestory();
  }

  public void Mail()
  {
    string helpContactAddress = Consts.GetInstance().HELP_CONTACT_ADDRESS;
    string str1 = Consts.Format(Consts.GetInstance().HELP_CONTACT_TITLE, (IDictionary) new Dictionary<string, string>()
    {
      {
        "short_id",
        Persist.userInfo.Data.userId
      }
    });
    string str2 = string.Format("{0}/{1}/{2}/{3}/{4}", (object) DeviceInfo.DeviceModel, (object) DeviceInfo.DeviceVendor, (object) DeviceInfo.OperatingSystem, (object) DeviceInfo.ProcessorType, (object) DeviceInfo.SystemMemorySize);
    string str3 = Consts.Format(Consts.GetInstance().HELP_CONTACT_MAIL_BODY, (IDictionary) new Dictionary<string, string>()
    {
      {
        "ver",
        Revision.DLCVersion
      },
      {
        "player_id",
        ""
      },
      {
        "short_id",
        Persist.userInfo.Data.userId
      },
      {
        "agent",
        str2
      }
    }).Replace("\n", "%0A");
    App.LaunchMailer(helpContactAddress, str1, str3);
  }
}

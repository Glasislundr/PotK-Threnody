// Decompiled with JetBrains decompiler
// Type: SeaGlobalMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class SeaGlobalMenu : MonoBehaviour
{
  [SerializeField]
  private CommonSeaHeader owner;
  [SerializeField]
  private GameObject infoNew;
  [SerializeField]
  private GameObject presentNew;
  [SerializeField]
  private GameObject menuBadge;
  [SerializeField]
  private GameObject missionBadge;
  [SerializeField]
  private GameObject gachaNewBadge;
  [SerializeField]
  private GameObject limitedShopSeaButtonNewIcon;
  [SerializeField]
  private GameObject newbiePacksIcon;
  [SerializeField]
  private GameObject bikkuriIcon;

  private void OnEnable()
  {
    bool flag = false;
    try
    {
      flag = ((IEnumerable<OfficialInformationArticle>) SMManager.Get<OfficialInformationArticle[]>()).Any<OfficialInformationArticle>((Func<OfficialInformationArticle, bool>) (w => !Persist.infoUnRead.Data.GetUnRead(w)));
    }
    catch
    {
      Persist.infoUnRead.Delete();
    }
    this.infoNew.SetActive(flag);
    PlayerPresent[] source = SMManager.Get<PlayerPresent[]>();
    this.presentNew.SetActive(source != null && ((IEnumerable<PlayerPresent>) source).Any<PlayerPresent>((Func<PlayerPresent, bool>) (p => !p.received_at.HasValue)));
    this.menuBadge.SetActive(Singleton<NGGameDataManager>.GetInstance().ReceivedFriendRequestCount > 0);
    this.missionBadge.SetActive(SMManager.Get<Player>().is_open_mission);
    DateTime? gachaLatestStartTime = Singleton<NGGameDataManager>.GetInstance().gachaLatestStartTime;
    DateTime dateTime1 = new DateTime();
    try
    {
      dateTime1 = Persist.lastAccessTime.Data.gachaRootLastAccessTime;
    }
    catch
    {
      Persist.lastAccessTime.Delete();
    }
    GameObject gachaNewBadge = this.gachaNewBadge;
    DateTime? nullable = gachaLatestStartTime;
    DateTime dateTime2 = dateTime1;
    int num = nullable.HasValue ? (nullable.GetValueOrDefault() > dateTime2 ? 1 : 0) : 0;
    gachaNewBadge.SetActive(num != 0);
    this.StartCoroutine(this.ShowIconNewOnShopButton());
    this.UpdateBikkuriIcon();
    this.UpdateNewbiePacksIcon();
  }

  private IEnumerator ShowIconNewOnShopButton()
  {
    if (Singleton<NGGameDataManager>.GetInstance().receivableGift)
      this.limitedShopSeaButtonNewIcon.SetActive(false);
    else if (Singleton<NGGameDataManager>.GetInstance().newbiePacks)
    {
      this.limitedShopSeaButtonNewIcon.SetActive(false);
    }
    else
    {
      IEnumerator e = ServerTime.WaitSync();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.limitedShopSeaButtonNewIcon.SetActive(ShopCommon.IsNewLimitedShop(ServerTime.NowAppTime()));
    }
  }

  private void UpdateNewbiePacksIcon()
  {
    if (Singleton<NGGameDataManager>.GetInstance().newbiePacks)
    {
      this.newbiePacksIcon.SetActive(true);
      this.limitedShopSeaButtonNewIcon.SetActive(false);
      this.bikkuriIcon.SetActive(false);
    }
    else
      this.newbiePacksIcon.SetActive(false);
  }

  public void UpdateBikkuriIcon()
  {
    if (Singleton<NGGameDataManager>.GetInstance().receivableGift)
    {
      this.bikkuriIcon.SetActive(true);
      this.newbiePacksIcon.SetActive(false);
      this.limitedShopSeaButtonNewIcon.SetActive(false);
    }
    else
      this.bikkuriIcon.SetActive(false);
  }

  private bool ClearSceneStacks(bool clearAll = false, string clearStackName = null)
  {
    bool flag1 = false;
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    if (!string.IsNullOrEmpty(clearStackName))
    {
      if (instance.sceneName == clearStackName)
      {
        flag1 = true;
        instance.destroyCurrentScene();
      }
      if (instance.clearStack(clearStackName) > 0)
        flag1 = true;
    }
    bool flag2 = false;
    NGSceneBase sceneBase = instance.sceneBase;
    if (Object.op_Inequality((Object) sceneBase, (Object) null))
      sceneBase.IsPush = true;
    if (clearAll)
    {
      instance.destroyLoadedScenes();
    }
    else
    {
      flag2 = instance.clearStackBeforeTopGlobalBack();
      instance.destoryNonStackScenes();
    }
    Singleton<NGGameDataManager>.GetInstance().clearScenePopupRecovery();
    return !flag1 && flag2;
  }

  private bool IsPushAndSet()
  {
    NGSceneBase sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase;
    if (Object.op_Inequality((Object) sceneBase, (Object) null))
    {
      if (sceneBase.IsPush)
        return true;
      sceneBase.IsPush = true;
    }
    return false;
  }

  private bool IsPush
  {
    get
    {
      NGSceneBase sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase;
      return Object.op_Inequality((Object) sceneBase, (Object) null) && sceneBase.IsPush;
    }
    set
    {
      NGSceneBase sceneBase = Singleton<NGSceneManager>.GetInstance().sceneBase;
      if (Object.op_Equality((Object) sceneBase, (Object) null) || sceneBase.IsPush == value)
        return;
      sceneBase.IsPush = value;
    }
  }

  public void IbtnSeaAlbum()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "sea030_album";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || !Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = true;
      Sea030AlbumScene.ChangeScene(this.ClearSceneStacks(clearStackName: clearStackName));
    }
    else
      this.IsPush = false;
  }

  public void IbtnSeaHome()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    if (Singleton<NGSceneManager>.GetInstance().sceneName != "sea030_home" || !Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = true;
      this.ClearSceneStacks(true);
      Sea030HomeScene.ChangeScene(false);
    }
    else
      this.IsPush = false;
  }

  public void IbtnSeaQuest()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "sea030_quest";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || !Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = true;
      Sea030_questScene.ChangeScene(this.ClearSceneStacks(clearStackName: clearStackName), forceInitialize: true);
    }
    else
      this.IsPush = false;
  }

  public void IbtnEventQuest()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "quest002_17";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      Quest00217Scene.ChangeScene(this.ClearSceneStacks(clearStackName: clearStackName));
    }
    else
      this.IsPush = false;
  }

  public void IbtnHome()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    this.ClearSceneStacks(true);
    MypageScene.ChangeScene(MypageRootMenu.Mode.STORY);
  }

  public void IbtnInfo()
  {
    string str = "mypage001_8_1";
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    if (Singleton<NGSceneManager>.GetInstance().sceneName != str || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      bool isStack = this.ClearSceneStacks(clearStackName: str);
      Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
    }
    else
      this.IsPush = false;
  }

  public void IbtnPresent()
  {
    if (this.IsPushAndSet())
      return;
    string str = "mypage001_7";
    this.owner.CloseMenu();
    if (Singleton<NGSceneManager>.GetInstance().sceneName != str || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      bool isStack = this.ClearSceneStacks(clearStackName: str);
      Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
    }
    else
      this.IsPush = false;
  }

  public void IbtnMenu()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string str = "story001_9_1";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != str || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      bool isStack = this.ClearSceneStacks(clearStackName: str);
      Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
    }
    else
      this.IsPush = false;
  }

  public void IbtnMission()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    Singleton<CommonRoot>.GetInstance().DailyMissionController.Show();
  }

  public void IbtnGacha()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string str = "gacha006_3";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != str || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      bool isStack = this.ClearSceneStacks(clearStackName: str);
      Singleton<NGSceneManager>.GetInstance().changeScene(str, isStack);
    }
    else
      this.IsPush = false;
  }

  public void IbtnShop()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "shop007_Top";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      ShopTopScene.ChangeScene(this.ClearSceneStacks(clearStackName: clearStackName));
    }
    else
      this.IsPush = false;
  }

  public void IbtnBringUp()
  {
    if (!(((object) Singleton<NGSceneManager>.GetInstance().sceneBase).GetType() != typeof (Unit004UnitTrainingListScene)) || this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    Singleton<NGGameDataManager>.GetInstance().IsSea = false;
    this.ClearSceneStacks();
    Unit004UnitTrainingListScene.changeScene(true);
  }

  public void IbtnTeam()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "unit004_6_0822_sea";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || !Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = true;
      Unit0046Scene.changeScene(this.ClearSceneStacks(clearStackName: clearStackName));
    }
    else
      this.IsPush = false;
  }

  public void IbtnGuild()
  {
    if (this.IsPushAndSet())
      return;
    this.owner.CloseMenu();
    string clearStackName = "guild028_1";
    if (Singleton<NGSceneManager>.GetInstance().sceneName != clearStackName || Singleton<NGGameDataManager>.GetInstance().IsSea)
    {
      Singleton<NGGameDataManager>.GetInstance().IsSea = false;
      this.ClearSceneStacks(clearStackName: clearStackName);
      MypageScene.ChangeScene(MypageRootMenu.Mode.GUILD);
    }
    else
      this.IsPush = false;
  }

  private void Update()
  {
  }

  private void onBackButton() => this.owner.CloseMenu();
}

// Decompiled with JetBrains decompiler
// Type: DailyMission0271PanelRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class DailyMission0271PanelRoot : MonoBehaviour
{
  public GameObject panel;
  private DailyMission0271PanelRoot.DailyMissionView viewModel;
  private PlayerBingoPanel playerBingoPanel;
  public static bool isBreakPanel;
  public static bool isAnimationEnd;
  private DailyMission0271Menu _menu;
  private DailyMission0271MissionRoot missionRoot;

  private DailyMission0271Menu menu
  {
    get
    {
      if (Object.op_Equality((Object) this._menu, (Object) null))
        this._menu = ((Component) ((Component) this).gameObject.transform.root).GetComponent<DailyMission0271Menu>();
      return this._menu;
    }
  }

  public IEnumerator Init(
    PlayerBingoPanel pbp,
    DailyMission0271MissionRoot missionRoot,
    GameObject prefab)
  {
    DailyMission0271PanelRoot mission0271PanelRoot = this;
    mission0271PanelRoot.playerBingoPanel = pbp;
    mission0271PanelRoot.viewModel = new DailyMission0271PanelRoot.DailyMissionView(pbp);
    mission0271PanelRoot.missionRoot = missionRoot;
    DailyMission0271Panel script = prefab.CloneAndGetComponent<DailyMission0271Panel>(((Component) mission0271PanelRoot).gameObject);
    IEnumerator e = script.Init(mission0271PanelRoot.viewModel);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    mission0271PanelRoot.panel = ((Component) script).gameObject;
    EventDelegate eventDelegate1 = new EventDelegate((MonoBehaviour) mission0271PanelRoot, "onPopup");
    EventDelegate eventDelegate2 = new EventDelegate((MonoBehaviour) mission0271PanelRoot, "onClear");
    script.popupButton.onClick.Add(eventDelegate1);
    script.clearButton.onClick.Add(eventDelegate2);
  }

  public void onPopup()
  {
    if (this.menu.CheckIsPush() || this.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.openDetailPopup());
  }

  public void onClear()
  {
    if (this.menu.CheckIsPush() || this.menu.IsPushAndSet())
      return;
    this.StartCoroutine(this.doLockItemChange(this.clearMission()));
  }

  private IEnumerator openDetailPopup()
  {
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_027_3__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    GameObject pObj = Singleton<PopupManager>.GetInstance().open(result, isNonSe: true, isNonOpenAnime: true);
    pObj.SetActive(false);
    e = pObj.GetComponent<DailyMission0271DetailPopup>().Init(this.viewModel, this.panel.GetComponent<DailyMission0271Panel>());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().startOpenAnime(pObj);
  }

  private IEnumerator doLockItemChange(IEnumerator e)
  {
    this.menu.isLockItemChange = true;
    while (e.MoveNext())
      yield return e.Current;
    this.menu.isLockItemChange = false;
  }

  private IEnumerator clearMission()
  {
    DailyMission0271PanelRoot mission0271PanelRoot = this;
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.loadingMode = 1;
    common.isLoading = true;
    Future<WebAPI.Response.BingoReceiveReward> receiveF = WebAPI.BingoReceiveReward(mission0271PanelRoot.playerBingoPanel.bingo_id, mission0271PanelRoot.playerBingoPanel.panel_id, (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = receiveF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.BingoReceiveReward r = receiveF.Result;
    if (r != null)
    {
      mission0271PanelRoot.menu.isPlayingEffect = true;
      common.isLoading = false;
      common.loadingMode = 0;
      common.isTouchBlock = true;
      // ISSUE: reference to a compiler-generated method
      PlayerBingo playerBingo = ((IEnumerable<PlayerBingo>) receiveF.Result.player_bingo).FirstOrDefault<PlayerBingo>(new Func<PlayerBingo, bool>(mission0271PanelRoot.\u003CclearMission\u003Eb__14_1));
      if (playerBingo != null)
        mission0271PanelRoot.menu.SetPlayerBingoData(playerBingo);
      Future<GameObject> breakPanelF = Res.Prefabs.dailymission027_1.ef_clear_misson_panel.Load<GameObject>();
      e1 = breakPanelF.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject breakPanelAnimation = breakPanelF.Result.Clone(((Component) ((Component) mission0271PanelRoot).transform.parent).gameObject.transform);
      breakPanelAnimation.transform.localScale = new Vector3(490f, 460f);
      breakPanelAnimation.GetComponent<Animation>().Play();
      NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
      if (Object.op_Inequality((Object) sm, (Object) null))
        sm.playSE("SE_0580");
      ((Component) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent()).GetComponentInChildren<CommonHeaderExp>().SetIsButtonEnable(false);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
      while (!DailyMission0271PanelRoot.isBreakPanel)
        yield return (object) new WaitForSeconds(0.3f);
      mission0271PanelRoot.panel.SetActive(false);
      while (!DailyMission0271PanelRoot.isAnimationEnd)
        yield return (object) new WaitForSeconds(0.3f);
      Object.Destroy((Object) breakPanelAnimation);
      Future<GameObject> clearPrefab = Res.Prefabs.battle.DailyMission_Clear.Load<GameObject>();
      e1 = clearPrefab.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      GameObject prefab = clearPrefab.Result.Clone();
      if (Object.op_Inequality((Object) prefab.GetComponent<UIWidget>(), (Object) null))
        ((UIRect) prefab.GetComponent<UIWidget>()).alpha = 0.0f;
      e1 = prefab.GetComponent<DailyMission0271Clear>().SetClearBonus(mission0271PanelRoot.viewModel.rewards);
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
      if (Object.op_Inequality((Object) sm, (Object) null))
        sm.playSE("SE_0534");
      yield return (object) new WaitForSeconds(1.5f);
      common.isTouchBlock = false;
      while (Singleton<PopupManager>.GetInstance().isOpen)
        yield return (object) null;
      if (r.is_complete_receive)
      {
        mission0271PanelRoot.missionRoot.CompRewardEffect();
      }
      else
      {
        common.isTouchBlock = false;
        mission0271PanelRoot.menu.isPlayingEffect = false;
      }
      ((Component) Singleton<CommonRoot>.GetInstance().GetNormalHeaderComponent()).GetComponentInChildren<CommonHeaderExp>().SetIsButtonEnable(true);
      Singleton<CommonRoot>.GetInstance().SetFooterEnable(true);
      DailyMission0271PanelRoot.isBreakPanel = false;
      DailyMission0271PanelRoot.isAnimationEnd = false;
      Object.Destroy((Object) mission0271PanelRoot.panel);
    }
  }

  public class DailyMissionView
  {
    public string name;
    public string detail;
    public string progressText;
    public string scene;
    public int? arg1;
    public bool isClear;
    public int bingo_id;
    public int panel_id;
    public int reward_id;

    public MasterDataTable.BingoRewardGroup[] rewards
    {
      get
      {
        return ((IEnumerable<MasterDataTable.BingoRewardGroup>) MasterData.BingoRewardGroupList).Where<MasterDataTable.BingoRewardGroup>((Func<MasterDataTable.BingoRewardGroup, bool>) (x => x.reward_group_id == this.reward_id)).ToArray<MasterDataTable.BingoRewardGroup>();
      }
    }

    public DailyMissionView(PlayerBingoPanel playerBingoPanel)
    {
      BingoMission bingoMission = MasterData.BingoMission[playerBingoPanel.bingo_panel_id];
      this.reward_id = bingoMission.reward_group_id;
      this.isClear = playerBingoPanel.is_open;
      this.bingo_id = playerBingoPanel.bingo_id;
      this.panel_id = playerBingoPanel.panel_id;
      this.name = bingoMission.name;
      this.detail = bingoMission.detail;
      this.progressText = Consts.Format(Consts.GetInstance().DAILY_MISSION_PROGRESS_FMT, (IDictionary) new Hashtable()
      {
        {
          (object) "count",
          (object) playerBingoPanel.count
        },
        {
          (object) "max",
          (object) bingoMission.clear_count
        }
      });
      this.scene = bingoMission.scene_name;
      this.arg1 = bingoMission.scene_arg;
    }
  }
}

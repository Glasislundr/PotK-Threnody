// Decompiled with JetBrains decompiler
// Type: DailyMission0271MissionRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class DailyMission0271MissionRoot : MonoBehaviour
{
  [SerializeField]
  private GameObject selectCompleteReward;
  [SerializeField]
  private UIScrollView scrollView;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private GameObject panelRoot;
  [SerializeField]
  private UI2DSprite compRewardImage;
  [SerializeField]
  private DailyMission0271PanelRoot[] panelRoots;
  [SerializeField]
  private UIButton CompleateRewardBtn;
  private PlayerBingo playerBingo;
  private GameObject panelPrefab;
  private GameObject compleateMissonSelectPrefab;
  private GameObject compleateMissionRewardDetailPrefab;
  private DailyMission0271Menu _menu;

  public DailyMission0271Menu menu
  {
    get
    {
      if (Object.op_Equality((Object) this._menu, (Object) null))
        this._menu = ((Component) ((Component) this).gameObject.transform.root).GetComponent<DailyMission0271Menu>();
      return this._menu;
    }
  }

  public IEnumerator InitPrefabs()
  {
    Future<GameObject> compleateMissonSelectPrefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/dailymission027_1/vscroll_complete_reward_list");
    IEnumerator e = compleateMissonSelectPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.compleateMissonSelectPrefab = compleateMissonSelectPrefabF.Result;
    Future<GameObject> panelPrefabF = Res.Prefabs.dailymission027_1.dir_Beginner_Mission.Load<GameObject>();
    e = panelPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.panelPrefab = panelPrefabF.Result;
    Future<GameObject> prefabF = Res.Prefabs.popup.popup_000_7_4_2__anim_popup01.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.compleateMissionRewardDetailPrefab = prefabF.Result;
  }

  public IEnumerator SetMissionData(PlayerBingo playerBingoData)
  {
    this.playerBingo = playerBingoData;
    IEnumerator e;
    if (this.playerBingo.selected_group_reward_id == 0)
    {
      ((UIButtonColor) this.CompleateRewardBtn).isEnabled = false;
      e = this.SetSelectCompReward();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      ((UIButtonColor) this.CompleateRewardBtn).isEnabled = true;
      e = this.SetPanel();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private IEnumerator SetSelectCompReward()
  {
    DailyMission0271MissionRoot missionRoot = this;
    ((Component) missionRoot.grid).transform.Clear();
    missionRoot.selectCompleteReward.SetActive(true);
    missionRoot.panelRoot.SetActive(false);
    string[] strArray = missionRoot.playerBingo.bingo.complete_reward_group_ids.Split(',');
    for (int index = 0; index < strArray.Length; ++index)
    {
      string s = strArray[index];
      int id = 0;
      ref int local = ref id;
      if (int.TryParse(s, out local))
      {
        IOrderedEnumerable<MasterDataTable.BingoRewardGroup> source = ((IEnumerable<MasterDataTable.BingoRewardGroup>) MasterData.BingoRewardGroupList).Where<MasterDataTable.BingoRewardGroup>((Func<MasterDataTable.BingoRewardGroup, bool>) (x => x.reward_group_id == id)).OrderBy<MasterDataTable.BingoRewardGroup, int>((Func<MasterDataTable.BingoRewardGroup, int>) (x => x.ID));
        if (source.Count<MasterDataTable.BingoRewardGroup>() > 0)
        {
          IEnumerator e = missionRoot.compleateMissonSelectPrefab.CloneAndGetComponent<DailyMission0271SelectCompRewardList>(((Component) missionRoot.grid).gameObject).Init(source.First<MasterDataTable.BingoRewardGroup>(), missionRoot, missionRoot.compleateMissionRewardDetailPrefab);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
    }
    strArray = (string[]) null;
    missionRoot.grid.Reposition();
    missionRoot.scrollView.ResetPosition();
  }

  private IEnumerator SetPanel()
  {
    DailyMission0271MissionRoot mission0271MissionRoot = this;
    mission0271MissionRoot.panelRoot.SetActive(true);
    mission0271MissionRoot.selectCompleteReward.SetActive(false);
    ((Component) mission0271MissionRoot.compRewardImage).gameObject.SetActive(false);
    // ISSUE: reference to a compiler-generated method
    IOrderedEnumerable<MasterDataTable.BingoRewardGroup> source = ((IEnumerable<MasterDataTable.BingoRewardGroup>) MasterData.BingoRewardGroupList).Where<MasterDataTable.BingoRewardGroup>(new Func<MasterDataTable.BingoRewardGroup, bool>(mission0271MissionRoot.\u003CSetPanel\u003Eb__17_0)).OrderBy<MasterDataTable.BingoRewardGroup, int>((Func<MasterDataTable.BingoRewardGroup, int>) (x => x.ID));
    IEnumerator e;
    if (source.Count<MasterDataTable.BingoRewardGroup>() > 0)
    {
      Future<Sprite> BackgroundImageF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("PanelMission/Background/{0}", (object) source.First<MasterDataTable.BingoRewardGroup>().background_image_name));
      e = BackgroundImageF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = BackgroundImageF.Result;
      if (Object.op_Inequality((Object) result, (Object) null))
      {
        ((Component) mission0271MissionRoot.compRewardImage).gameObject.SetActive(true);
        mission0271MissionRoot.compRewardImage.sprite2D = result;
        UI2DSprite compRewardImage1 = mission0271MissionRoot.compRewardImage;
        Rect textureRect1 = result.textureRect;
        int width = (int) ((Rect) ref textureRect1).width;
        ((UIWidget) compRewardImage1).width = width;
        UI2DSprite compRewardImage2 = mission0271MissionRoot.compRewardImage;
        Rect textureRect2 = result.textureRect;
        int height = (int) ((Rect) ref textureRect2).height;
        ((UIWidget) compRewardImage2).height = height;
      }
      BackgroundImageF = (Future<Sprite>) null;
    }
    e = mission0271MissionRoot.createBingoMission(mission0271MissionRoot.playerBingo.player_bingo_panel);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) null;
  }

  private IEnumerator createBingoMission(PlayerBingoPanel[] panels)
  {
    DailyMission0271MissionRoot missionRoot = this;
    for (int i = 0; i < panels.Length; ++i)
    {
      ((Component) missionRoot.panelRoots[i]).transform.Clear();
      PlayerBingoPanel panel = panels[i];
      if (!panel.is_reward_get)
      {
        if (missionRoot.panelRoots.Length <= i)
          break;
        IEnumerator e = missionRoot.panelRoots[i].Init(panel, missionRoot, missionRoot.panelPrefab);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }

  public void SetCompReward(int reward_group_id)
  {
    this.StartCoroutine(this.SelectCompReward(reward_group_id));
  }

  private IEnumerator SelectCompReward(int reward_group_id)
  {
    DailyMission0271MissionRoot mission0271MissionRoot = this;
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.BingoSelectComplete> future = WebAPI.BingoSelectComplete(mission0271MissionRoot.playerBingo.bingo_id, reward_group_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
    }));
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    // ISSUE: reference to a compiler-generated method
    PlayerBingo playerBingo = ((IEnumerable<PlayerBingo>) future.Result.player_bingo).FirstOrDefault<PlayerBingo>(new Func<PlayerBingo, bool>(mission0271MissionRoot.\u003CSelectCompReward\u003Eb__20_1));
    if (playerBingo != null)
    {
      mission0271MissionRoot.menu.SetPlayerBingoData(playerBingo);
      e = mission0271MissionRoot.SetMissionData(playerBingo);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
      MypageScene.ChangeScene();
    Singleton<CommonRoot>.GetInstance().isLoading = false;
    Singleton<CommonRoot>.GetInstance().loadingMode = 0;
  }

  public void CompRewardEffect()
  {
    this.menu.isPlayingEffect = true;
    this.StartCoroutine(this.playCompRewardEffect(((IEnumerable<MasterDataTable.BingoRewardGroup>) MasterData.BingoRewardGroupList).Where<MasterDataTable.BingoRewardGroup>((Func<MasterDataTable.BingoRewardGroup, bool>) (x => x.reward_group_id == this.playerBingo.selected_group_reward_id)).ToArray<MasterDataTable.BingoRewardGroup>()));
  }

  private IEnumerator playCompRewardEffect(MasterDataTable.BingoRewardGroup[] Rewards)
  {
    CommonRoot common = Singleton<CommonRoot>.GetInstance();
    common.isTouchBlock = true;
    NGSoundManager sm = Singleton<NGSoundManager>.GetInstance();
    yield return (object) new WaitForSeconds(0.2f);
    this.menu.IsPush = true;
    Future<GameObject> clearPrefab2 = Res.Prefabs.battle.DailyMission_Complete.Load<GameObject>();
    IEnumerator e = clearPrefab2.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab2 = clearPrefab2.Result.Clone();
    if (Object.op_Inequality((Object) prefab2.GetComponent<UIWidget>(), (Object) null))
      ((UIRect) prefab2.GetComponent<UIWidget>()).alpha = 0.0f;
    e = prefab2.GetComponent<DailyMission0271Clear>().SetClearBonus(Rewards);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<PopupManager>.GetInstance().open(prefab2, isCloned: true);
    if (Object.op_Inequality((Object) sm, (Object) null))
      sm.playSE("SE_0535");
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) new WaitForSeconds(0.5f);
    if (((IEnumerable<PlayerBingo>) SMManager.Get<PlayerBingo[]>()).Where<PlayerBingo>((Func<PlayerBingo, bool>) (x => !x.is_end)).ToArray<PlayerBingo>().Length != 0)
    {
      e = this.menu.Init();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.menu.isPlayingEffect = false;
    }
    else
      MypageScene.ChangeScene();
    common.isTouchBlock = false;
  }
}

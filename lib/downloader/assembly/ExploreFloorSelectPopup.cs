// Decompiled with JetBrains decompiler
// Type: ExploreFloorSelectPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class ExploreFloorSelectPopup : BackButtonMenuBase
{
  [SerializeField]
  private NGxScroll mScroll;

  public IEnumerator Initialize()
  {
    ExploreFloorSelectPopup basePopup = this;
    Future<GameObject> prefabF = new ResourceObject("Prefabs/explore033_Top/dir_FloorSelect_btn").Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    ExploreFloor frontFloor = MasterData.ExploreFloor[Singleton<ExploreDataManager>.GetInstance().FrontFloorId];
    ExploreFloor[] array = ((IEnumerable<ExploreFloor>) MasterData.ExploreFloorList).Where<ExploreFloor>((Func<ExploreFloor, bool>) (x => x.period_id == frontFloor.period_id)).OrderBy<ExploreFloor, int>((Func<ExploreFloor, int>) (x => x.floor)).ToArray<ExploreFloor>();
    int index;
    for (index = 0; index < array.Length; ++index)
    {
      GameObject gameObject = result.Clone();
      basePopup.mScroll.Add(gameObject, true);
      gameObject.GetComponent<ExploreFloorSelectButton>().Initialize(basePopup, array[index].ID);
      if (array[index].ID == frontFloor.ID)
        break;
    }
    if (index >= 6)
      basePopup.mScroll.ResolvePosition(new Vector2(0.0f, 1f));
    else
      basePopup.mScroll.ResolvePosition(new Vector2(0.0f, 0.0f));
  }

  public void OnFloorMoveButton(int floorId)
  {
    if (floorId == Singleton<ExploreDataManager>.GetInstance().FloorData.ID || this.IsPushAndSet())
      return;
    PopupCommonNoYes.Show("階層選択確認", string.Format("{0}階に移動しますか？\n\n到達した階層に\n自由に移動できます", (object) MasterData.ExploreFloor[floorId].floor), (Action) (() => this.StartCoroutine(this.MoveFloor(floorId))), (Action) (() => this.IsPush = false), callbackAfterClose: true);
  }

  private IEnumerator MoveFloor(int floorId)
  {
    CommonRoot commonRoot = Singleton<CommonRoot>.GetInstance();
    ExploreSceneManager exploreSceneMgr = Singleton<ExploreSceneManager>.GetInstance();
    commonRoot.isTouchBlock = true;
    exploreSceneMgr.Pause(true);
    Singleton<PopupManager>.GetInstance().closeAll(true);
    yield return (object) new WaitWhile((Func<bool>) (() => Singleton<PopupManager>.GetInstance().isOpen));
    exploreSceneMgr.ScreenEffect.TransitionFullIn();
    yield return (object) exploreSceneMgr.ScreenEffect.WaitForTransitionFull();
    int preMode = commonRoot.loadingMode;
    commonRoot.loadingMode = 3;
    commonRoot.isLoading = true;
    bool saveFailed = false;
    yield return (object) Singleton<ExploreDataManager>.GetInstance().MoveFloor(floorId, (Action) (() => saveFailed = true));
    exploreSceneMgr.SetReloadDirty();
    yield return (object) Singleton<NGSceneManager>.GetInstance().destroyLoadedScenesImmediate();
    if (saveFailed)
      MypageScene.ChangeScene();
    else
      Explore033TopScene.changeScene(false, false);
    commonRoot.loadingMode = preMode;
  }

  public void OnFloorInfoButton(int floorId)
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.OpenFloorInfoPopup(floorId));
  }

  private IEnumerator OpenFloorInfoPopup(int floorId)
  {
    ExploreFloorSelectPopup floorSelectPopup = this;
    Future<GameObject> loader = Singleton<ResourceManager>.GetInstance().Load<GameObject>("Prefabs/explore033_Top/popup_RewardDetails");
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = Singleton<PopupManager>.GetInstance().open(loader.Result).GetComponent<ExploreFloorInfoPopup>().Initialize(floorId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    yield return (object) new WaitForSeconds(0.1f);
    floorSelectPopup.IsPush = false;
  }

  public void OnCloseButton()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().dismiss();
  }

  public override void onBackButton() => this.OnCloseButton();
}

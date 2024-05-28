// Decompiled with JetBrains decompiler
// Type: Quest052171Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Earth;
using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Quest052171Menu : BackButtonMenuBase
{
  [SerializeField]
  private UILabel TxtTitle;
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private UIGrid grid;
  private DateTime serverTime;
  private GameObject ScrollPrefab;

  public IEnumerator Init()
  {
    EarthDataManager earthDataManager = Singleton<EarthDataManager>.GetInstanceOrNull();
    if (!Object.op_Equality((Object) earthDataManager, (Object) null))
    {
      this.TxtTitle.SetText(this.GetTitle());
      foreach (Component component in ((Component) this.grid).transform)
        Object.Destroy((Object) component.gameObject);
      ((Component) this.grid).gameObject.SetActive(false);
      IEnumerator e;
      if (Object.op_Equality((Object) this.ScrollPrefab, (Object) null))
      {
        Future<GameObject> ScrollPrefabF = Res.Prefabs.quest052_17_1.scroll.Load<GameObject>();
        e = ScrollPrefabF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.ScrollPrefab = ScrollPrefabF.Result;
        ScrollPrefabF = (Future<GameObject>) null;
      }
      foreach (EarthExtraQuest enableEarthExtraQuest in earthDataManager.GetEnableEarthExtraQuestList())
      {
        EarthExtraQuest quest = enableEarthExtraQuest;
        MasterDataTable.EarthQuestKey questKey = ((IEnumerable<MasterDataTable.EarthQuestKey>) MasterData.EarthQuestKeyList).FirstOrDefault<MasterDataTable.EarthQuestKey>((Func<MasterDataTable.EarthQuestKey, bool>) (x => x.quest_id == quest.ID));
        if (questKey != null)
        {
          e = this.ScrollInit(quest, questKey, earthDataManager.IsKeyQuestOpen(questKey.ID), this.ScrollPrefab);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
        }
      }
      yield return (object) null;
      this.scrollview.ResetPosition();
      ((Component) this.grid).gameObject.SetActive(true);
      this.grid.Reposition();
      this.scrollview.ResetPosition();
    }
  }

  public IEnumerator ScrollInit(
    EarthExtraQuest quest,
    MasterDataTable.EarthQuestKey questKey,
    bool isPlay,
    GameObject prefab)
  {
    Quest052171Menu menu = this;
    GameObject gameObject = Object.Instantiate<GameObject>(prefab);
    gameObject.transform.parent = ((Component) menu.grid).transform;
    gameObject.transform.localScale = Vector3.one;
    gameObject.transform.localPosition = Vector3.zero;
    IEnumerator e = gameObject.GetComponent<Quest052171Scroll>().InitScroll(quest, questKey, isPlay, menu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void Foreground() => Debug.Log((object) "click default event Foreground");

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().sceneBase.IsPush = true;
    Singleton<NGSceneManager>.GetInstance().destroyLoadedScenes();
    Singleton<NGSceneManager>.GetInstance().clearStack();
    Mypage051Scene.ChangeScene(false);
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnEvent() => Debug.Log((object) "click default event IbtnEvent");

  public virtual void VScrollBar() => Debug.Log((object) "click default event VScrollBar");

  private string GetTitle() => Consts.GetInstance().QUEST_00217_KEY_TITLE;

  public void StartAPI_QuestRelease(EarthExtraQuest quest, MasterDataTable.EarthQuestKey questKey, bool canPlay)
  {
    this.StartCoroutine(this.QuestRelease(quest, questKey, canPlay));
  }

  private IEnumerator QuestRelease(EarthExtraQuest quest, MasterDataTable.EarthQuestKey questKey, bool canPlay)
  {
    EarthDataManager instance = Singleton<EarthDataManager>.GetInstance();
    if (canPlay)
    {
      ModalWindow.Show(Consts.GetInstance().QUEST_052171_RELEASEFAILED_POPUP_TITLE, Consts.GetInstance().QUEST_052171_RELEASEFAILED_POPUP_DESCRIPTION, (Action) (() => Singleton<PopupManager>.GetInstance().onDismiss()));
    }
    else
    {
      int num = 0;
      Earth.EarthQuestKey earthQuestKey = ((IEnumerable<Earth.EarthQuestKey>) Singleton<EarthDataManager>.GetInstance().GetQuestKeys()).Where<Earth.EarthQuestKey>((Func<Earth.EarthQuestKey, bool>) (x => x.keyID == questKey.ID)).FirstOrDefault<Earth.EarthQuestKey>();
      if (earthQuestKey != null)
        num = earthQuestKey.quantity;
      if (num < quest.use_key_num)
      {
        ModalWindow.Show(Consts.GetInstance().QUEST_052171_RELEASEFAILED_POPUP_TITLE2, Consts.GetInstance().QUEST_052171_RELEASEFAILED_POPUP_DESCRIPTION2, (Action) (() => Singleton<PopupManager>.GetInstance().onDismiss()));
      }
      else
      {
        instance.KeyQuestOpen(questKey.ID, quest.use_key_num);
        IEnumerator e = this.Init();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }
  }
}

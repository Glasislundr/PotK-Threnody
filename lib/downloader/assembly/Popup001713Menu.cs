// Decompiled with JetBrains decompiler
// Type: Popup001713Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class Popup001713Menu : BackButtonMenuBase
{
  public UILabel TxtDescription;
  private bool IsPresent;
  private Mypage0017Menu menu0017;
  private List<PlayerPresent> receiveList = new List<PlayerPresent>();

  public IEnumerator Init(Mypage0017Menu menu, PlayerPresent[] presents)
  {
    this.menu0017 = menu;
    this.IsPresent = false;
    Tuple<List<PlayerPresent>, bool> receiveList = Mypage0017Menu.createReceiveList(presents);
    this.receiveList = receiveList.Item1;
    int num = receiveList.Item2 ? 1 : 0;
    if (this.receiveList.Where<PlayerPresent>((Func<PlayerPresent, bool>) (x => x.reward_type_id.HasValue)).Count<PlayerPresent>() != 0)
      this.IsPresent = true;
    if (num == 0)
    {
      this.TxtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_001713_DESCRIPT_TEXT, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) this.receiveList.Count
        }
      }));
    }
    else
    {
      this.TxtDescription.SetTextLocalize(Consts.Format(Consts.GetInstance().POPUP_001715_DESCRIPT_TEXT, (IDictionary) new Hashtable()
      {
        {
          (object) "Count",
          (object) this.receiveList.Count
        }
      }));
      yield break;
    }
  }

  private IEnumerator ReceiveAll()
  {
    Popup001713Menu popup001713Menu = this;
    Singleton<PopupManager>.GetInstance().onDismiss(true);
    Singleton<CommonRoot>.GetInstance().loadingMode = 1;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.PresentRead> receive = WebAPI.PresentRead(popup001713Menu.receiveList.Select<PlayerPresent, int>((Func<PlayerPresent, int>) (x => x.id)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (error =>
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = receive.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (receive.Result != null)
    {
      e = OnDemandDownload.WaitLoadHasUnitResource(false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (receive.Result.is_success)
      {
        popup001713Menu.StartCoroutine(popup001713Menu.menu0017.UpdateList(SMManager.Get<PlayerPresent[]>()));
        List<PlayerPresent> readList = new List<PlayerPresent>();
        popup001713Menu.receiveList.ForEach((Action<PlayerPresent>) (x => ((IEnumerable<PlayerPresent>) ((IEnumerable<PlayerPresent>) receive.Result.player_presents).Where<PlayerPresent>((Func<PlayerPresent, bool>) (y => x.id == y.id)).ToArray<PlayerPresent>()).ForEach<PlayerPresent>((Action<PlayerPresent>) (z =>
        {
          if (!z.received_at.HasValue)
            return;
          readList.Add(z);
        }))));
        Future<GameObject> popupPrefabF;
        if (!popup001713Menu.IsPresent)
        {
          popupPrefabF = Res.Prefabs.popup.popup_001_7_3__anim_popup01.Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          GameObject result = popupPrefabF.Result;
          Popup00173Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup00173Menu>();
          popup001713Menu.StartCoroutine(component.Init(readList.ToArray()));
          popupPrefabF = (Future<GameObject>) null;
        }
        else
        {
          e = popup001713Menu.CharacterStoryPopup(receive.Result.unlock_quests);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          if (popup001713Menu.receiveList.Count != readList.Count)
          {
            popupPrefabF = Res.Prefabs.popup.popup_001_7_3__anim_popup01.Load<GameObject>();
            e = popupPrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject result = popupPrefabF.Result;
            Popup00173Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Popup00173Menu>();
            popup001713Menu.StartCoroutine(component.Init(readList.ToArray(), popup001713Menu.receiveList.Count - readList.Count, true));
            popupPrefabF = (Future<GameObject>) null;
          }
          else
          {
            popupPrefabF = Res.Prefabs.popup.popup_001_7_1__anim_popup01.Load<GameObject>();
            e = popupPrefabF.Wait();
            while (e.MoveNext())
              yield return e.Current;
            e = (IEnumerator) null;
            GameObject result = popupPrefabF.Result;
            Mypage00171Menu component = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Mypage00171Menu>();
            popup001713Menu.StartCoroutine(component.Init(popup001713Menu.receiveList.ToArray()));
            popupPrefabF = (Future<GameObject>) null;
          }
        }
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
      else
      {
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      }
    }
  }

  public void IbtnYes()
  {
    this.StartCoroutine(this.ReceiveAll());
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void IbtnNo() => Singleton<PopupManager>.GetInstance().onDismiss();

  public override void onBackButton() => this.IbtnNo();

  public IEnumerator CharacterStoryPopup(UnlockQuest[] unlockQuests)
  {
    if (unlockQuests != null && unlockQuests.Length != 0)
    {
      Future<GameObject> prefab = Res.Prefabs.battle.popup_020_11_2__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      UnlockQuest[] unlockQuestArray = unlockQuests;
      for (int index = 0; index < unlockQuestArray.Length; ++index)
      {
        QuestCharacterS quest = MasterData.QuestCharacterS[unlockQuestArray[index].quest_s_id];
        Singleton<NGSoundManager>.GetInstance().playSE("SE_1028", delay: 0.8f);
        Battle020112Menu o = this.OpenPopup(prefab.Result).GetComponent<Battle020112Menu>();
        e = o.Init(quest, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        bool isFinished = false;
        o.SetCallback((Action) (() => isFinished = true));
        while (!isFinished)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.6f);
        o = (Battle020112Menu) null;
      }
      unlockQuestArray = (UnlockQuest[]) null;
      unlockQuests = (UnlockQuest[]) null;
    }
  }

  public GameObject OpenPopup(GameObject original)
  {
    GameObject gameObject = Singleton<PopupManager>.GetInstance().open(original);
    ((Component) gameObject.transform.parent.Find("Popup Mask")).gameObject.GetComponent<TweenAlpha>().to = 0.75f;
    return gameObject;
  }
}

// Decompiled with JetBrains decompiler
// Type: Quest002171Scroll
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
public class Quest002171Scroll : BannerBase
{
  [SerializeField]
  private UISprite Clear;
  [SerializeField]
  private UISprite New;
  [SerializeField]
  private SpriteDecimalControl possessionDigit;
  [SerializeField]
  private GameObject possessionObj;
  [SerializeField]
  private GameObject timeText;
  private bool canPlay;

  public bool CanPlay => this.canPlay;

  public bool IsBackToKeyQuest { get; set; }

  public IEnumerator InitScroll(
    PlayerQuestGate[] keyQuests,
    DateTime serverTime,
    bool isBackToKeyQuest = true)
  {
    Quest002171Scroll quest002171Scroll = this;
    quest002171Scroll.IsBackToKeyQuest = isBackToKeyQuest;
    int questKeyId = keyQuests[0].quest_key_id;
    quest002171Scroll.canPlay = ((IEnumerable<PlayerQuestGate>) keyQuests).Any<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x =>
    {
      if (!x.in_progress)
        return false;
      DateTime dateTime = serverTime;
      DateTime? endAt = x.end_at;
      return endAt.HasValue && dateTime < endAt.GetValueOrDefault();
    }));
    string path = quest002171Scroll.SetSpritePath(questKeyId, quest002171Scroll.canPlay);
    IEnumerator e = quest002171Scroll.SetAndCreate_BannerSprite(path, quest002171Scroll.IdleSprite);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    PlayerQuestKey[] source = SMManager.Get<PlayerQuestKey[]>();
    PlayerQuestKey playerQuestKey = new PlayerQuestKey();
    if (source != null || source.Length != 0)
      playerQuestKey = ((IEnumerable<PlayerQuestKey>) source).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == keyQuests[0].quest_key_id)).FirstOrDefault<PlayerQuestKey>();
    if (playerQuestKey != null || source.Length != 0)
      quest002171Scroll.SetPossession(playerQuestKey.quantity);
    else
      quest002171Scroll.SetPossession(0);
    quest002171Scroll.SetScrollButtonCondition(keyQuests, quest002171Scroll.canPlay, serverTime);
    if (quest002171Scroll.canPlay)
    {
      quest002171Scroll.EndTime = ((IEnumerable<PlayerQuestGate>) keyQuests).First<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (y => y.in_progress)).end_at.Value;
      quest002171Scroll.SetTime(serverTime, CampaignQuest.RankingEventTerm.normal);
      quest002171Scroll.timeText.SetActive(true);
    }
    else
      quest002171Scroll.timeText.SetActive(false);
  }

  private string SetSpritePath(int id, bool canplay)
  {
    return BannerBase.GetSpriteIdlePath(id, BannerBase.Type.quest_lock, QuestExtra.SeekType.None, canplay);
  }

  private IEnumerator SetAndCreate_BannerSprite(string path, UI2DSprite obj)
  {
    if (!Singleton<ResourceManager>.GetInstance().Contains(path))
    {
      Debug.LogWarning((object) path);
      path = string.Format("Prefabs/Banners/ExtraQuest/M/1/Specialquest_idle");
    }
    Future<Texture2D> future = Singleton<ResourceManager>.GetInstance().Load<Texture2D>(path);
    IEnumerator e = future.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Texture2D result = future.Result;
    if (!Object.op_Equality((Object) result, (Object) null))
    {
      Sprite sprite = Sprite.Create(result, new Rect(0.0f, 0.0f, (float) ((Texture) result).width, (float) ((Texture) result).height), new Vector2(0.5f, 0.5f), 1f, 100U, (SpriteMeshType) 0);
      ((Object) sprite).name = ((Object) result).name;
      obj.sprite2D = sprite;
    }
  }

  private void SetScrollButtonCondition(
    PlayerQuestGate[] keyQuests,
    bool canPlay,
    DateTime serverTime)
  {
    if (canPlay)
    {
      PlayerQuestGate keyQuest = ((IEnumerable<PlayerQuestGate>) keyQuests).First<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => x.in_progress));
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => this.changeScene(keyQuest, ((Component) this).gameObject, serverTime)));
    }
    else
      EventDelegate.Set(this.BtnFormation.onClick, (EventDelegate.Callback) (() => this.StartQuestReleasePopup(keyQuests)));
  }

  private void changeScene(PlayerQuestGate keyQuest, GameObject obj, DateTime serverTime)
  {
    this.StartCoroutine(this.QuestTimeCompare(keyQuest));
  }

  public void ChangeScene(PlayerQuestGate playerKey)
  {
    this.StartCoroutine(this.QuestTimeCompare(playerKey));
  }

  public void StartQuestReleasePopup(PlayerQuestGate[] keyQuests)
  {
    if (this.IsBackToKeyQuest)
      this.StartCoroutine(this.OpenQuestReleasePopup(keyQuests, this));
    else
      this.StartCoroutine(this.OpenCollaboQuestReleasePopup(keyQuests, this));
  }

  private IEnumerator OpenQuestReleasePopup(PlayerQuestGate[] keyQuests, Quest002171Scroll scroll)
  {
    Quest002171Scroll quest002171Scroll = this;
    bool flag = false;
    Future<GameObject> popupF;
    switch (keyQuests.Length)
    {
      case 0:
        popupF = (Future<GameObject>) null;
        break;
      case 1:
        PlayerExtraQuestS playerExtraQuestS = ((IEnumerable<PlayerExtraQuestS>) ((IEnumerable<PlayerExtraQuestS>) SMManager.Get<PlayerExtraQuestS[]>()).CheckMasterData().ToArray<PlayerExtraQuestS>()).First<PlayerExtraQuestS>((Func<PlayerExtraQuestS, bool>) (x => x.quest_extra_s.ID == keyQuests[0].quest_ids[0]));
        if (playerExtraQuestS.remain_battle_count.HasValue)
        {
          int? remainBattleCount = playerExtraQuestS.remain_battle_count;
          int num = 0;
          if (remainBattleCount.GetValueOrDefault() == num & remainBattleCount.HasValue)
            flag = true;
        }
        popupF = new ResourceObject("Prefabs/Banners/KeyQuest/popup_prefab/temp_popup_1").Load<GameObject>();
        break;
      case 2:
        popupF = new ResourceObject("Prefabs/Banners/KeyQuest/popup_prefab/temp_popup_2").Load<GameObject>();
        break;
      case 3:
        popupF = new ResourceObject("Prefabs/Banners/KeyQuest/popup_prefab/temp_popup_3").Load<GameObject>();
        break;
      default:
        popupF = new ResourceObject("Prefabs/Banners/KeyQuest/popup_prefab/temp_popup_over").Load<GameObject>();
        break;
    }
    if (popupF != null)
    {
      if (flag)
      {
        quest002171Scroll.StartCoroutine(PopupCommon.Show(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_TITLE, Consts.GetInstance().QUEST_002171_NOT_REMAINING));
      }
      else
      {
        IEnumerator e = popupF.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        GameObject popup = Singleton<PopupManager>.GetInstance().open(popupF.Result);
        popup.SetActive(false);
        Quest002171QuestOpenPopup script = popup.GetComponent<Quest002171QuestOpenPopup>();
        e = script.Init(keyQuests, scroll);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        yield return (object) null;
        popup.SetActive(true);
        script.resolveScrollPosition();
        popup = (GameObject) null;
        script = (Quest002171QuestOpenPopup) null;
      }
    }
  }

  private IEnumerator OpenCollaboQuestReleasePopup(
    PlayerQuestGate[] keyQuests,
    Quest002171Scroll scroll)
  {
    if (keyQuests.Length != 0 && keyQuests.Length <= 1)
    {
      Future<GameObject> popupF = Res.Prefabs.Banners.KeyQuest.popup_prefab.temp_popup_collabo.Load<GameObject>();
      IEnumerator e = popupF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = popupF.Result.Clone();
      Quest002171CollaboOpenPopup script = popup.GetComponent<Quest002171CollaboOpenPopup>();
      popup.SetActive(false);
      e = script.Init(keyQuests, scroll);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      script.resolveScrollPosition();
    }
  }

  private IEnumerator QuestTimeCompare(PlayerQuestGate keyQuest)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = ServerTime.WaitSync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    DateTime dateTime = ServerTime.NowAppTimeAddDelta();
    DateTime? endAt = keyQuest.end_at;
    if ((endAt.HasValue ? (dateTime < endAt.GetValueOrDefault() ? 1 : 0) : 0) != 0)
    {
      Quest00220Scene.ChangeScene00220(keyQuest.quest_ids[0], this.IsBackToKeyQuest);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Future<GameObject> time_popup = Res.Prefabs.popup.popup_002_23__anim_popup01.Load<GameObject>();
      e = time_popup.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = time_popup.Result;
      Singleton<PopupManager>.GetInstance().openAlert(result);
      time_popup = (Future<GameObject>) null;
    }
  }

  private void SetPossession(int keyNum)
  {
    this.possessionObj.SetActive(true);
    this.possessionDigit.setNumber(keyNum);
  }
}

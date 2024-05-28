// Decompiled with JetBrains decompiler
// Type: Quest002171QuestOpenPopup
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
public class Quest002171QuestOpenPopup : BackButtonMenuBase
{
  [SerializeField]
  private UILabel description;
  [SerializeField]
  private UILabel Title;
  [SerializeField]
  private UIButton ibtnYes;
  [SerializeField]
  private UIButton ibtnNo;
  [SerializeField]
  private List<GameObject> Banners;
  private PlayerQuestGate passGate;
  private bool isQuestResult;
  private Quest002171Scroll scrollcomp;
  private BattleUI05PopupResortie battleResortie;
  [SerializeField]
  private UIGrid grid;
  [SerializeField]
  private UIScrollView scrollview;
  [SerializeField]
  private UIScrollBar scrollbar;
  [SerializeField]
  private NGxScroll ngxScroll;
  [SerializeField]
  private UI2DSprite keySprite;
  [SerializeField]
  private UILabel txtPossession;

  public IEnumerator Init(PlayerQuestGate[] gates, Quest002171Scroll scrollcomp)
  {
    this.scrollcomp = scrollcomp;
    this.isQuestResult = false;
    PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == gates[0].quest_key_id)).FirstOrDefault<PlayerQuestKey>();
    int quantity = playerQuestKey == null ? 0 : playerQuestKey.quantity;
    this.txtPossession.SetTextLocalize(quantity);
    this.Title.SetText(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_TITLE);
    IEnumerator e = this.CreateKeySprite(gates[0].quest_key_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.InitOfEachCount(gates, quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator Init(PlayerQuestGate[] gates, BattleUI05PopupResortie battleResortie)
  {
    this.isQuestResult = true;
    this.battleResortie = battleResortie;
    PlayerQuestKey playerQuestKey = ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == gates[0].quest_key_id)).FirstOrDefault<PlayerQuestKey>();
    int quantity = playerQuestKey == null ? 0 : playerQuestKey.quantity;
    this.txtPossession.SetTextLocalize(quantity);
    this.Title.SetText(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_TITLE);
    IEnumerator e = this.CreateKeySprite(gates[0].quest_key_id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.InitOfEachCount(gates, quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator InitOfEachCount(PlayerQuestGate[] gates, int quantity)
  {
    IEnumerator e;
    switch (gates.Length)
    {
      case 1:
        this.passGate = gates[0];
        this.description.SetTextLocalize(Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION, (IDictionary) new Hashtable()
        {
          {
            (object) "name",
            (object) MasterData.QuestkeyQuestkey[this.passGate.quest_key_id].name
          },
          {
            (object) nameof (quantity),
            (object) this.passGate.consume_quantity.ToLocalizeNumberText()
          },
          {
            (object) "time",
            (object) this.GetReleaseTime(this.passGate.time)
          }
        }));
        if (!Object.op_Inequality((Object) this.ibtnYes, (Object) null))
          break;
        ((UIButtonColor) this.ibtnYes).isEnabled = quantity >= this.passGate.consume_quantity;
        break;
      case 2:
        this.description.SetText(Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION_2, (IDictionary) new Hashtable()
        {
          {
            (object) "name",
            (object) MasterData.QuestkeyQuestkey[gates[0].quest_key_id].name
          }
        }));
        e = this.CreateBannerSprite(gates, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      default:
        this.description.SetText(Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION_2, (IDictionary) new Hashtable()
        {
          {
            (object) "name",
            (object) MasterData.QuestkeyQuestkey[gates[0].quest_key_id].name
          }
        }));
        e = this.CreateBanner(gates.Length);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        e = this.CreateBannerSprite(gates, false, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  private string GetReleaseTime(int totalSec)
  {
    string releaseTime = "";
    TimeSpan timeSpan = new TimeSpan(0, 0, 0, totalSec);
    if (timeSpan.Days > 0)
      releaseTime += Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION_DAY, (IDictionary) new Hashtable()
      {
        {
          (object) "day",
          (object) timeSpan.Days
        }
      });
    if (timeSpan.Hours > 0)
      releaseTime += Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION_HOUR, (IDictionary) new Hashtable()
      {
        {
          (object) "hour",
          (object) timeSpan.Hours
        }
      });
    if (timeSpan.Minutes > 0)
      releaseTime += Consts.Format(Consts.GetInstance().QUEST_002171_RELEASEPOPUP_DESCRIPTION_MIN, (IDictionary) new Hashtable()
      {
        {
          (object) "min",
          (object) timeSpan.Minutes
        }
      });
    return releaseTime;
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    ((UIButtonColor) this.ibtnYes).isEnabled = false;
    ((UIButtonColor) this.ibtnNo).isEnabled = false;
    Singleton<PopupManager>.GetInstance().dismiss();
    this.StartAPI_QuestRelease(this.passGate, this.scrollcomp);
  }

  private void StartAPI_QuestRelease(PlayerQuestGate gate, Quest002171Scroll scroll)
  {
    if (this.isQuestResult)
      this.StartCoroutine(this.QuestRelease(gate));
    else
      this.StartCoroutine(this.QuestRelease(gate, scroll));
  }

  private IEnumerator QuestRelease(PlayerQuestGate gate, Quest002171Scroll scroll)
  {
    IEnumerator e;
    if (scroll.CanPlay)
    {
      PlayerQuestKey key = ((IEnumerable<PlayerQuestKey>) SMManager.Get<PlayerQuestKey[]>()).Where<PlayerQuestKey>((Func<PlayerQuestKey, bool>) (x => x.quest_key_id == gate.quest_key_id)).First<PlayerQuestKey>();
      Future<GameObject> prefabF = Res.Prefabs.popup.popup_002_17_1__anim_popup01.Load<GameObject>();
      e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = prefabF.Result.Clone();
      Quest002171CantOpenQuestPopup component = popup.GetComponent<Quest002171CantOpenQuestPopup>();
      popup.SetActive(false);
      e = component.Init(MasterData.QuestkeyQuestkey[gate.quest_key_id].name, gate.quest_key_id, key.quantity, gate.consume_quantity);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
    }
    else
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      bool isFaildSpend = false;
      e = WebAPI.QuestkeySpend(gate.quest_key_id, gate.consume_quantity, gate.quest_gate_id, (Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        isFaildSpend = true;
      })).Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      if (!isFaildSpend)
      {
        Future<WebAPI.Response.QuestkeyIndex> keyfuture = WebAPI.QuestkeyIndex((Action<WebAPI.Response.UserError>) (error =>
        {
          WebAPI.DefaultUserErrorCallback(error);
          Singleton<CommonRoot>.GetInstance().loadingMode = 0;
          Singleton<CommonRoot>.GetInstance().isLoading = false;
        }));
        e = keyfuture.Wait();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        scroll.ChangeScene(((IEnumerable<PlayerQuestGate>) keyfuture.Result.quest_gates).FirstOrDefault<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => x.quest_gate_id == gate.quest_gate_id)));
      }
    }
  }

  private IEnumerator QuestRelease(PlayerQuestGate gate)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    bool isFaildSpend = false;
    IEnumerator e = WebAPI.QuestkeySpend(gate.quest_key_id, gate.consume_quantity, gate.quest_gate_id, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      isFaildSpend = true;
    })).Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!isFaildSpend)
    {
      Future<WebAPI.Response.QuestkeyIndex> keyfuture = WebAPI.QuestkeyIndex((Action<WebAPI.Response.UserError>) (error =>
      {
        WebAPI.DefaultUserErrorCallback(error);
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        Singleton<CommonRoot>.GetInstance().isLoading = false;
      }));
      e = keyfuture.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      Singleton<CommonRoot>.GetInstance().loadingMode = 0;
      ((IEnumerable<PlayerQuestGate>) keyfuture.Result.quest_gates).FirstOrDefault<PlayerQuestGate>((Func<PlayerQuestGate, bool>) (x => x.quest_gate_id == gate.quest_gate_id));
      Singleton<PopupManager>.GetInstance().dismiss();
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();

  private IEnumerator CreateKeySprite(int keyID)
  {
    Future<Sprite> spriteF = MasterData.QuestkeyQuestkey[keyID].LoadSpriteThumbnail();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.keySprite.sprite2D = spriteF.Result;
  }

  private IEnumerator CreateBanner(int num)
  {
    this.ngxScroll.Clear();
    this.Banners.Clear();
    string path = string.Format("Prefabs/Banners/KeyQuest/popup_banner/banner_base");
    Future<GameObject> prefabF = Singleton<ResourceManager>.GetInstance().Load<GameObject>(path);
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    for (int index = 0; index < num; ++index)
    {
      GameObject gameObject = result.Clone(((Component) this.grid).transform);
      this.ngxScroll.Add(gameObject);
      this.Banners.Add(gameObject);
    }
    if (num > 4 && Object.op_Inequality((Object) this.scrollbar, (Object) null))
      ((Component) this.scrollbar).gameObject.SetActive(true);
    this.ngxScroll.scrollView.panel.baseClipRegion = Vector4.zero;
    this.ngxScroll.ResolvePosition();
    yield return (object) null;
  }

  private IEnumerator CreateBannerSprite(PlayerQuestGate[] gates, bool isScroll, bool isAtlas)
  {
    for (int i = 0; i < gates.Length; ++i)
    {
      IEnumerator e = this.Banners[i].GetComponent<Quest002171PopupBanner>().InitScroll(isScroll, isAtlas, gates[i], this.scrollcomp);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public void resolveScrollPosition()
  {
    if (!Object.op_Inequality((Object) this.ngxScroll, (Object) null))
      return;
    this.ngxScroll.ResolvePosition(0);
  }
}

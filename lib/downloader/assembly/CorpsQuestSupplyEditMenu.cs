// Decompiled with JetBrains decompiler
// Type: CorpsQuestSupplyEditMenu
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
[AddComponentMenu("Scenes/CorpsQuest/SupplyEditMenu")]
public class CorpsQuestSupplyEditMenu : BackButtonMenuBase
{
  [SerializeField]
  private GameObject[] linkItem;
  [SerializeField]
  private GameObject[] selectButton;
  [SerializeField]
  private GameObject[] changeButton;
  private GameObject confirmPopup;
  private GameObject warningPopup;
  private int[] selectedUnitIDs;
  private List<SupplyItem> SupplyItems = new List<SupplyItem>();
  private List<SupplyItem> SaveDeck = new List<SupplyItem>();
  private ItemIcon[] icons = new ItemIcon[Consts.GetInstance().DECK_SUPPLY_MAX];
  private GameObject iconPrefab;
  private CorpsUtil.SequenceType sequenceType;
  private PlayerCorps playerCorps;
  private PlayerCorpsDeck playerCorpsDeck;

  public IEnumerator ResourceLoad()
  {
    Future<GameObject> f = Res.Prefabs.popup.popup_029_tower_supplies_confirm__anim_popup01.Load<GameObject>();
    IEnumerator e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.confirmPopup = f.Result;
    f = (Future<GameObject>) null;
    f = Res.Prefabs.popup.popup_029_tower_supplies_warning_confirm__anim_popup01.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.warningPopup = f.Result;
    f = (Future<GameObject>) null;
    f = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.iconPrefab = f.Result;
    f = (Future<GameObject>) null;
  }

  private IEnumerator SetSelectionUnitAndSupply()
  {
    CorpsQuestSupplyEditMenu questSupplyEditMenu = this;
    questSupplyEditMenu.IsPush = true;
    Singleton<PopupManager>.GetInstance().dismiss(true);
    while (Singleton<PopupManager>.GetInstance().isOpen)
      yield return (object) null;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    foreach (SupplyItem deck in questSupplyEditMenu.SupplyItems.DeckList())
    {
      intList1.Add(deck.SelectCount);
      intList2.Add(deck.Supply.ID);
    }
    IEnumerator e1;
    if (questSupplyEditMenu.sequenceType == CorpsUtil.SequenceType.Reset)
    {
      Future<WebAPI.Response.QuestCorpsReset> f = WebAPI.QuestCorpsReset(intList1.ToArray(), intList2.ToArray(), questSupplyEditMenu.playerCorps.period_id, questSupplyEditMenu.selectedUnitIDs, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) f.Result.corps_player_unit_ids);
        f = (Future<WebAPI.Response.QuestCorpsReset>) null;
      }
    }
    else
    {
      Future<WebAPI.Response.QuestCorpsEntry> f = WebAPI.QuestCorpsEntry(intList1.ToArray(), intList2.ToArray(), questSupplyEditMenu.playerCorps.period_id, questSupplyEditMenu.selectedUnitIDs, (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
      e1 = f.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (f.Result == null)
      {
        MypageScene.ChangeSceneOnError();
        yield break;
      }
      else
      {
        Singleton<NGGameDataManager>.GetInstance().corps_player_unit_ids = new HashSet<int>((IEnumerable<int>) f.Result.corps_player_unit_ids);
        f = (Future<WebAPI.Response.QuestCorpsEntry>) null;
      }
    }
    // ISSUE: reference to a compiler-generated method
    e1 = questSupplyEditMenu.UpdateCorpsDeck(new Action(questSupplyEditMenu.\u003CSetSelectionUnitAndSupply\u003Eb__14_2));
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
  }

  private IEnumerator UpdateCorpsDeck(Action successAction)
  {
    CorpsQuestSupplyEditMenu questSupplyEditMenu = this;
    if (questSupplyEditMenu.playerCorpsDeck == null || questSupplyEditMenu.playerCorpsDeck.deck_player_unit_ids.Length == 0)
    {
      successAction();
    }
    else
    {
      List<int> intList = new List<int>(questSupplyEditMenu.playerCorpsDeck.deck_player_unit_ids.Length);
      bool flag = false;
      for (int index = 0; index < questSupplyEditMenu.playerCorpsDeck.deck_player_unit_ids.Length; ++index)
      {
        if (((IEnumerable<int>) questSupplyEditMenu.selectedUnitIDs).Contains<int>(questSupplyEditMenu.playerCorpsDeck.deck_player_unit_ids[index]))
          intList.Add(questSupplyEditMenu.playerCorpsDeck.deck_player_unit_ids[index]);
        else
          flag = true;
      }
      if (flag)
      {
        Future<WebAPI.Response.QuestCorpsDeckEdit> f = WebAPI.QuestCorpsDeckEdit(questSupplyEditMenu.playerCorps.period_id, intList.ToArray(), (Action<WebAPI.Response.UserError>) (e => WebAPI.DefaultUserErrorCallback(e)));
        IEnumerator e1 = f.Wait();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        if (f.Result == null)
        {
          MypageScene.ChangeSceneOnError();
          yield break;
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          questSupplyEditMenu.playerCorpsDeck = Array.Find<PlayerCorpsDeck>(f.Result.player_corps_decks, new Predicate<PlayerCorpsDeck>(questSupplyEditMenu.\u003CUpdateCorpsDeck\u003Eb__15_1));
          f = (Future<WebAPI.Response.QuestCorpsDeckEdit>) null;
        }
      }
      successAction();
    }
  }

  private void IbtnChange(int idx)
  {
    if (this.IsPushAndSet())
      return;
    this.InitSaveSpace();
    bool flag = this.SupplyItems.RemoveDeck(idx);
    Quest00210aScene.ChangeScene(false, new Quest00210Menu.Param()
    {
      SupplyItems = this.SupplyItems,
      SaveDeck = this.SaveDeck,
      select = idx,
      removeButton = flag,
      limitedOnly = true,
      mode = Quest00210Scene.Mode.Corps,
      args = CorpsQuestSupplyEditScene.toArgs(this.playerCorps, this.selectedUnitIDs, this.sequenceType)
    }, true);
  }

  private void UpdateSelectButton()
  {
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
      this.changeButton[index].SetActive(false);
    int num = 0;
    for (int index = 0; index < this.SupplyItems.Count; ++index)
    {
      if (this.SupplyItems[index].DeckIndex > 0)
      {
        this.changeButton[this.SupplyItems[index].DeckIndex - 1].SetActive(true);
        ++num;
      }
    }
  }

  private void InitSaveSpace() => this.SaveDeck = this.SupplyItems.Copy();

  public IEnumerator InitializeAsync(
    PlayerCorps corps,
    int[] unitIDs,
    SupplyItem[] supplyItems,
    CorpsUtil.SequenceType type)
  {
    CorpsQuestSupplyEditMenu questSupplyEditMenu = this;
    questSupplyEditMenu.playerCorps = corps;
    questSupplyEditMenu.selectedUnitIDs = unitIDs;
    CorpsPeriod corpsPeriod;
    if (MasterData.CorpsPeriod.TryGetValue(questSupplyEditMenu.playerCorps.period_id, out corpsPeriod))
    {
      int settingCorpsSetting = corpsPeriod.setting_CorpsSetting;
    }
    // ISSUE: reference to a compiler-generated method
    questSupplyEditMenu.playerCorpsDeck = Array.Find<PlayerCorpsDeck>(SMManager.Get<PlayerCorpsDeck[]>(), new Predicate<PlayerCorpsDeck>(questSupplyEditMenu.\u003CInitializeAsync\u003Eb__19_0));
    questSupplyEditMenu.sequenceType = type;
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
    {
      GameObject self = Object.Instantiate<GameObject>(questSupplyEditMenu.iconPrefab);
      if (Object.op_Inequality((Object) questSupplyEditMenu.icons[index], (Object) null))
        Object.Destroy((Object) ((Component) questSupplyEditMenu.icons[index]).gameObject);
      self.SetParent(questSupplyEditMenu.linkItem[index]);
      ItemIcon component = self.GetComponent<ItemIcon>();
      component.SetModeSupply();
      questSupplyEditMenu.icons[index] = component;
    }
    questSupplyEditMenu.SupplyItems = ((IEnumerable<SupplyItem>) supplyItems).ToList<SupplyItem>();
    questSupplyEditMenu.InitSaveSpace();
    int counter = 0;
    foreach (SupplyItem s in questSupplyEditMenu.SupplyItems)
    {
      if (s.DeckIndex != 0)
      {
        questSupplyEditMenu.icons[s.DeckIndex - 1].SetEmpty(false);
        IEnumerator e = questSupplyEditMenu.icons[s.DeckIndex - 1].InitBySupplyItem(s);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        questSupplyEditMenu.icons[s.DeckIndex - 1].QuantitySupply = true;
        questSupplyEditMenu.icons[s.DeckIndex - 1].EnableQuantity(s.SelectCount);
        ++counter;
      }
    }
    for (int i = 0; i < Consts.GetInstance().DECK_SUPPLY_MAX; ++i)
    {
      if (!questSupplyEditMenu.SupplyItems.Where<SupplyItem>((Func<SupplyItem, bool>) (x => x.DeckIndex == i + 1)).Any<SupplyItem>())
        questSupplyEditMenu.icons[i].SetEmpty(true);
    }
    questSupplyEditMenu.UpdateSelectButton();
  }

  public void onDecideButton()
  {
    if (this.IsPushAndSet())
      return;
    GameObject prefab = this.confirmPopup.Clone();
    prefab.SetActive(false);
    prefab.GetComponent<Tower029SupplyConfirmPopup>().Initialize((Action) (() => this.StartCoroutine(this.SetSelectionUnitAndSupply())));
    prefab.SetActive(true);
    Singleton<PopupManager>.GetInstance().open(prefab, isCloned: true);
  }

  public void onSupplyButton1() => this.IbtnChange(1);

  public void onSupplyButton2() => this.IbtnChange(2);

  public void onSupplyButton3() => this.IbtnChange(3);

  public void onSupplyButton4() => this.IbtnChange(4);

  public void onSupplyButton5() => this.IbtnChange(5);

  public void onResetButton()
  {
    if (this.IsPushAndSet())
      return;
    this.SupplyItems.RemoveAll();
    for (int index = 0; index < Consts.GetInstance().DECK_SUPPLY_MAX; ++index)
    {
      this.icons[index].SetEmpty(true);
      this.icons[index].QuantitySupply = false;
    }
    this.UpdateSelectButton();
    this.InitSaveSpace();
    this.StartCoroutine(this.IsPushOff());
  }

  public override void onBackButton()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }
}

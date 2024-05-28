// Decompiled with JetBrains decompiler
// Type: Bugu0053Menu
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
public class Bugu0053Menu : BackButtonMenuBase
{
  [SerializeField]
  protected GameObject[] iconParent;
  [SerializeField]
  protected GameObject[] dir_Add;
  [SerializeField]
  protected GameObject dirPageSilver;
  [SerializeField]
  protected GameObject dirPageGold;
  [SerializeField]
  protected GameObject dirPageRainbow;
  [SerializeField]
  protected GameObject dirRecipe;
  [SerializeField]
  protected UILabel TxtComposite;
  [SerializeField]
  protected UILabel TxtNeededzenie;
  [SerializeField]
  protected UILabel TxtZenie;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected GameObject DirRecipeName;
  [SerializeField]
  protected UILabel TxtRecipeName;
  [SerializeField]
  protected GameObject IbtnRecipeObject;
  private const int DEPTH = 12;
  private const int RARITY_WARNING = 3;
  private const int SELECT_MIN = 2;
  private const int SELECT_MAX = 5;
  private bool ViewButtonFlag;
  private bool isInit;
  private ItemIcon gearScript;
  private List<GameCore.ItemInfo> SendGears = new List<GameCore.ItemInfo>();
  private List<InventoryItem> playerGears = new List<InventoryItem>();
  public GameObject itemIconPrefab;
  private GameObject rarityWarningPrefab;

  public IEnumerator InitGearsSynthesis(List<InventoryItem> playerGears, Player player)
  {
    if (this.isInit)
    {
      playerGears.Clear();
      this.isInit = false;
    }
    if (!SMManager.Get<Player>().IsGearRecipe())
      this.IbtnRecipeObject.SetActive(false);
    this.DestroySelectIcon();
    this.InitAddButton();
    this.ViewButtonFlag = false;
    this.dirPageRainbow.SetActive(false);
    this.dirPageGold.SetActive(false);
    this.dirPageSilver.SetActive(false);
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.itemIconPrefab = prefabF.Result;
    e = this.CreateMaterialList(playerGears);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.SetZenie();
    this.DirRecipeName.SetActive(false);
    Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = -1;
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public void SetRecipeInfo(List<InventoryItem> gears, string recipeName)
  {
    this.DestroySelectIcon();
    this.InitAddButton();
    this.StartCoroutine(this.CreateMaterialList(gears));
    this.SetZenie();
    this.DirRecipeName.SetActive(true);
    this.TxtRecipeName.SetTextLocalize(recipeName);
  }

  public IEnumerator CreateMaterialList(List<InventoryItem> gears)
  {
    this.playerGears = gears;
    int totalRank = 0;
    this.SendGears.Clear();
    foreach (InventoryItem playerGear in this.playerGears)
      this.SendGears.Add(playerGear.Item);
    for (int i = 0; i < this.playerGears.Count; ++i)
    {
      this.gearScript = this.itemIconPrefab.Clone(this.iconParent[i].transform).GetComponent<ItemIcon>();
      IEnumerator e = this.gearScript.InitByItemInfo(this.playerGears[i].Item);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.gearScript.EnableLongPressEvent();
      this.gearScript.resetExpireDate();
      totalRank += this.playerGears[i].Item.gearLevel;
      if (totalRank >= 25)
        this.dirPageRainbow.SetActive(true);
      else if (totalRank >= 20)
        this.dirPageGold.SetActive(true);
      else if (totalRank >= 15)
        this.dirPageSilver.SetActive(true);
    }
    for (int count = this.playerGears.Count; count < 5; ++count)
    {
      this.dir_Add[count].SetActive(false);
      this.gearScript = this.itemIconPrefab.Clone(this.iconParent[count].transform).GetComponent<ItemIcon>();
      this.gearScript.SetEmpty(true);
      if (!this.ViewButtonFlag)
      {
        this.dir_Add[count].SetActive(true);
        this.ViewButtonFlag = true;
      }
    }
  }

  public void SetZenie()
  {
    this.TxtZenie.SetTextLocalize(CalcItemCost.GetCompositeCost(this.playerGears));
  }

  public virtual void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public virtual void IbtnStartComposite()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.StartComposite());
  }

  public void IbtnRecipe()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00510Scene.changeSceneRecipe(true);
  }

  public void recipeDisable() => this.dirRecipe.SetActive(false);

  private void InitAddButton()
  {
    for (int index = 0; index < 5; ++index)
      this.dir_Add[index].SetActive(false);
  }

  private void InitIcon(int count)
  {
    for (int index = count; index < 5; ++index)
    {
      this.dir_Add[index].SetActive(false);
      if (!this.ViewButtonFlag)
      {
        this.dir_Add[index].SetActive(true);
        this.ViewButtonFlag = true;
      }
    }
  }

  private void DestroySelectIcon()
  {
    foreach (GameObject gameObject in this.iconParent)
      ((IEnumerable<ItemIcon>) gameObject.GetComponentsInChildren<ItemIcon>()).ForEach<ItemIcon>((Action<ItemIcon>) (x => Object.Destroy((Object) ((Component) x).gameObject)));
  }

  private IEnumerator StartComposite()
  {
    Bugu0053Menu bugu0053Menu = this;
    List<GameCore.ItemInfo> list1 = bugu0053Menu.SendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.gear.rarity.index >= 3)).ToList<GameCore.ItemInfo>();
    List<GameCore.ItemInfo> list2 = bugu0053Menu.SendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.favorite)).ToList<GameCore.ItemInfo>();
    int countCompositeManaseed = 0;
    int num = bugu0053Menu.SendGears.Sum<GameCore.ItemInfo>((Func<GameCore.ItemInfo, int>) (x =>
    {
      if (!x.gear.isComposeManaSeed())
        return 0;
      ++countCompositeManaseed;
      return x.gearAccessoryRemainingAmount;
    }));
    if (list2.Count == 0)
    {
      GearGear gear = countCompositeManaseed <= 0 || countCompositeManaseed != bugu0053Menu.SendGears.Count ? (GearGear) null : bugu0053Menu.SendGears.First<GameCore.ItemInfo>().gear;
      Action execComposite = gear != null ? (bugu0053Menu.findManaseedCombineRecipe(gear.group_id, countCompositeManaseed) != null ? (Action) (() => PopupCommonNoYes.Show(Consts.GetInstance().POPUP_005_COMPOSITE_MANASEEDS_WARNING_TITLE2, Consts.GetInstance().POPUP_005_COMPOSITE_MANASEEDS_WARNING_MESSAGE2, (Action) (() => this.CompositeAPIEvent()), (Action) (() => { }), callbackAfterClose: true)) : (num > gear.manaSeedRecoveryLimit ? (Action) (() => PopupCommonNoYes.Show(Consts.GetInstance().POPUP_005_COMPOSITE_MANASEEDS_WARNING_TITLE, Consts.GetInstance().POPUP_005_COMPOSITE_MANASEEDS_WARNING_MESSAGE, (Action) (() => this.CompositeAPIEvent()), (Action) (() => { }), callbackAfterClose: true)) : (Action) (() => this.CompositeAPIEvent()))) : (Action) (() => this.CompositeAPIEvent());
      if (list1.Count >= 1)
      {
        IEnumerator e;
        if (Object.op_Equality((Object) bugu0053Menu.rarityWarningPrefab, (Object) null))
        {
          Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_005_8_3_2.Load<GameObject>();
          e = popupPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          bugu0053Menu.rarityWarningPrefab = popupPrefabF.Result;
          popupPrefabF = (Future<GameObject>) null;
        }
        GameObject obj = Singleton<PopupManager>.GetInstance().openAlert(bugu0053Menu.rarityWarningPrefab);
        obj.transform.localPosition = new Vector3(0.0f, 110f, 0.0f);
        e = obj.GetComponent<Bugu005382Menu>().ChangeSprite(bugu0053Menu.SendGears);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        int countTap = 0;
        EventDelegate.Add(obj.GetComponent<Bugu005382Menu>().yesButton.onClick, (EventDelegate.Callback) (() =>
        {
          if (countTap == 0)
            execComposite();
          ++countTap;
        }));
        obj = (GameObject) null;
      }
      else
        execComposite();
    }
    else
    {
      string title = Consts.Format(Consts.GetInstance().POPUP_005_GEAR_WARNING_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "type",
          (object) Consts.GetInstance().GEAR_0052_COMPOSITE
        }
      });
      string message = Consts.Format(Consts.GetInstance().POPUP_005_FAVORITE_WARNING_MESSAGE, (IDictionary) new Hashtable()
      {
        {
          (object) "type",
          (object) Consts.GetInstance().GEAR_0052_COMPOSITE
        }
      });
      bugu0053Menu.StartCoroutine(PopupCommon.Show(title, message));
    }
  }

  private GearCombineRecipe findManaseedCombineRecipe(int id, int quantity)
  {
    return ((IEnumerable<GearCombineRecipe>) MasterData.GearCombineRecipeList).FirstOrDefault<GearCombineRecipe>((Func<GearCombineRecipe, bool>) (x =>
    {
      if (x.material1_gear_id != id)
        return false;
      int num1 = 1;
      int? nullable;
      if (x.material2_gear_id.HasValue)
      {
        int num2 = id;
        nullable = x.material2_gear_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(num2 == valueOrDefault & nullable.HasValue))
          return false;
        ++num1;
      }
      if (x.material3_gear_id.HasValue)
      {
        int num3 = id;
        nullable = x.material3_gear_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(num3 == valueOrDefault & nullable.HasValue))
          return false;
        ++num1;
      }
      if (x.material4_gear_id.HasValue)
      {
        int num4 = id;
        nullable = x.material4_gear_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(num4 == valueOrDefault & nullable.HasValue))
          return false;
        ++num1;
      }
      if (x.material5_gear_id.HasValue)
      {
        int num5 = id;
        nullable = x.material5_gear_id;
        int valueOrDefault = nullable.GetValueOrDefault();
        if (!(num5 == valueOrDefault & nullable.HasValue))
          return false;
        ++num1;
      }
      return num1 == quantity;
    }));
  }

  public void CompositeAPIEvent()
  {
    if (this.playerGears.Count >= 2)
      this.StartCoroutine(this.CompositeAPI());
    else
      this.StartCoroutine(PopupCommon.Show(Consts.Format(Consts.GetInstance().POPUP_005_GEAR_WARNING_TITLE, (IDictionary) new Hashtable()
      {
        {
          (object) "type",
          (object) Consts.GetInstance().GEAR_0052_COMPOSITE
        }
      }), Consts.Format(Consts.GetInstance().POPUP_005_COMPOSITE_WARNING_MESSAGE)));
  }

  public IEnumerator CompositeAPI()
  {
    int reisouJewelBefore = SMManager.Get<Player>().reisou_jewel;
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    Future<WebAPI.Response.ItemGearCombine> futureF = WebAPI.ItemGearCombine(this.playerGears.ToGearId().ToArray(), this.playerGears.ToMaterialId().ToArray(), (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = futureF.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    WebAPI.Response.ItemGearCombine result = futureF.Result;
    if (result != null)
    {
      bool isNew = false;
      List<GameCore.ItemInfo> numList = new List<GameCore.ItemInfo>(this.playerGears.Select<InventoryItem, GameCore.ItemInfo>((Func<InventoryItem, GameCore.ItemInfo>) (x => x.Item)));
      if (result.player_item != (PlayerItem) null)
      {
        numList.Add(new GameCore.ItemInfo(result.player_item));
        isNew = result.player_item.is_new;
      }
      if (result.player_material_gear != (PlayerMaterialGear) null)
        numList.Add(new GameCore.ItemInfo(result.player_material_gear));
      int addReisouJewel = result.player.reisou_jewel - reisouJewelBefore;
      Singleton<CommonRoot>.GetInstance().isLoading = false;
      this.isInit = true;
      Bugu00539Scene.ChangeScene(true, new Bugu00539ChangeSceneParam(numList, isNew, result.animation_pattern, (GameCore.ItemInfo) null, (PlayerItem) null, (Action) null, addReisouJewel));
    }
  }

  public void SelectGear()
  {
    if (this.IsPush)
      return;
    Singleton<PopupManager>.GetInstance().closeAll();
    Debug.LogWarning((object) ("SendGears:" + (object) this.SendGears.Count));
    if (this.SendGears.Count > 0)
      Singleton<NGGameDataManager>.GetInstance().lastReferenceItemID = this.SendGears.Last<GameCore.ItemInfo>().itemID;
    Bugu00521Scene.ChangeScene(false, this.SendGears);
  }

  public void onEndScene()
  {
    this.ViewButtonFlag = false;
    this.playerGears.Clear();
  }
}

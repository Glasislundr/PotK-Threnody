// Decompiled with JetBrains decompiler
// Type: Sea030GiftDetailsScrollList
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
public class Sea030GiftDetailsScrollList : MonoBehaviour
{
  [SerializeField]
  private UILabel txtTitle;
  [SerializeField]
  private GameObject txt_attention;
  [SerializeField]
  private GameObject txt_lock_recipe;
  [SerializeField]
  private GameObject txtPercentAmount;
  [SerializeField]
  private GameObject txtPercentAmountNum;
  [SerializeField]
  private GameObject txtZenyAmount;
  [SerializeField]
  private GameObject txtZenyAmountNum;
  [SerializeField]
  private GameObject dirYes;
  [SerializeField]
  private UIButton recipeYes;
  [SerializeField]
  private GameObject dirMaterialGift;
  [SerializeField]
  private GameObject dynMainGift;
  [SerializeField]
  private UILabel txtTips;
  [SerializeField]
  private GameObject arrowBlue;
  [SerializeField]
  private GameObject[] txtMaterialNum;
  [SerializeField]
  private GameObject[] slcMaterialNum_txt;
  [SerializeField]
  private GameObject[] dynMaterialBugu;
  [SerializeField]
  private Transform[] iconTransform;
  private CallGiftRecipe call_recipe;
  private GearGear call_item;
  private GameObject ItemIconPrefab;
  private Sea030GiftDetailsScrollList.ListType list_type;
  private Sea030GiftDetailsScrollContainer ScrollContainerMenu;
  private int myLine;
  private int nowColumn;
  private int parentColumn;
  private Sea030GiftDetailsScrollList.GiftMixerInfo giftMixerInfo = new Sea030GiftDetailsScrollList.GiftMixerInfo();

  public IEnumerator Init(
    List<CallGiftRecipe> recipeList,
    GearGear item,
    GearGear parent_item,
    GameObject IconPrefab,
    Sea030GiftDetailsScrollList.ListType list_type,
    Sea030GiftDetailsScrollContainer menu,
    int my_line,
    int parent_column)
  {
    Sea030GiftDetailsScrollList detailsScrollList = this;
    detailsScrollList.call_item = item;
    detailsScrollList.giftMixerInfo.successGear = item;
    detailsScrollList.giftMixerInfo.parentGear = parent_item;
    detailsScrollList.ItemIconPrefab = IconPrefab;
    detailsScrollList.list_type = list_type;
    detailsScrollList.ScrollContainerMenu = menu;
    detailsScrollList.myLine = my_line;
    detailsScrollList.parentColumn = parent_column;
    IEnumerator e;
    if (detailsScrollList.list_type == Sea030GiftDetailsScrollList.ListType.Tips)
    {
      detailsScrollList.call_recipe = (CallGiftRecipe) null;
      e = detailsScrollList.CreateTipsLsit();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      detailsScrollList.txtTitle.SetTextLocalize(detailsScrollList.call_item.name);
      // ISSUE: reference to a compiler-generated method
      PlayerMaterialGear player_material = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>(new Func<PlayerMaterialGear, bool>(detailsScrollList.\u003CInit\u003Eb__29_0));
      int quantity = 0;
      if (player_material != (PlayerMaterialGear) null)
        quantity = player_material.quantity;
      e = detailsScrollList.SetCallItemIcon(detailsScrollList.call_item, detailsScrollList.dynMainGift.transform, false, player_material, 0);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      detailsScrollList.giftMixerInfo.playerSuccessGearCount = quantity;
      if (detailsScrollList.list_type == Sea030GiftDetailsScrollList.ListType.Child)
      {
        Transform transform = detailsScrollList.iconTransform[detailsScrollList.parentColumn - 1];
        Vector3 position = detailsScrollList.arrowBlue.transform.position;
        position.x = transform.position.x;
        detailsScrollList.arrowBlue.transform.position = position;
      }
      bool flag = recipeList.Count >= 1;
      detailsScrollList.txt_attention.SetActive(!flag);
      if (detailsScrollList.ScrollContainerMenu.isLockRecipe)
      {
        flag = false;
        detailsScrollList.txt_lock_recipe.SetActive(true);
        detailsScrollList.txt_attention.SetActive(false);
      }
      detailsScrollList.txtPercentAmount.SetActive(flag);
      detailsScrollList.txtPercentAmountNum.SetActive(flag);
      detailsScrollList.txtZenyAmount.SetActive(flag);
      detailsScrollList.txtZenyAmountNum.SetActive(flag);
      detailsScrollList.dirYes.SetActive(flag);
      detailsScrollList.dirMaterialGift.SetActive(flag);
      if (flag)
      {
        detailsScrollList.call_recipe = recipeList[0];
        e = detailsScrollList.CreateRecipeLsit();
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
      else
      {
        detailsScrollList.call_recipe = (CallGiftRecipe) null;
        yield break;
      }
    }
    yield return (object) null;
  }

  private IEnumerator CreateTipsLsit()
  {
    CallItem callItem = (CallItem) null;
    MasterData.CallItem.TryGetValue(this.call_item.ID, out callItem);
    if (callItem != null)
      this.txtTips.SetTextLocalize(callItem.tips);
    else
      this.txtTips.SetTextLocalize("");
    yield return (object) null;
  }

  private IEnumerator CreateRecipeLsit()
  {
    ((UIButtonColor) this.recipeYes).isEnabled = true;
    this.txtPercentAmountNum.GetComponent<UILabel>().SetTextLocalize(this.call_recipe.success_ratio);
    this.giftMixerInfo.successRatio = this.call_recipe.success_ratio;
    UILabel component = this.txtZenyAmountNum.GetComponent<UILabel>();
    if (SMManager.Get<Player>().money < (long) this.call_recipe.cost_money)
    {
      ((UIButtonColor) this.recipeYes).isEnabled = false;
      component.SetTextLocalize("[fa0000]{0}".F((object) this.call_recipe.cost_money));
    }
    else
      component.SetTextLocalize(this.call_recipe.cost_money);
    foreach (int column_num in Enumerable.Range(1, 5))
    {
      System.Type type = this.call_recipe.GetType();
      object obj = type.GetField("material{0}_gear_id_GearGear".F((object) column_num)).GetValue((object) this.call_recipe);
      if (obj != null)
      {
        int material_gear_id = (int) obj;
        int material_count = (int) type.GetField("material{0}_gear_count".F((object) column_num)).GetValue((object) this.call_recipe);
        this.txtMaterialNum[column_num - 1].GetComponent<UILabel>().SetTextLocalize(material_count);
        PlayerMaterialGear player_material = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == material_gear_id));
        int player_material_quantity = 0;
        bool isGray = false;
        if (player_material == (PlayerMaterialGear) null)
        {
          isGray = true;
          ((UIButtonColor) this.recipeYes).isEnabled = false;
        }
        else
        {
          player_material_quantity = player_material.quantity;
          if (player_material_quantity < material_count)
          {
            isGray = true;
            ((UIButtonColor) this.recipeYes).isEnabled = false;
          }
        }
        GearGear materialGear = (GearGear) null;
        MasterData.GearGear.TryGetValue(material_gear_id, out materialGear);
        IEnumerator e = this.SetCallItemIcon(materialGear, this.dynMaterialBugu[column_num - 1].transform, isGray, player_material, column_num);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        this.giftMixerInfo.materialGear.Add(new Sea030GiftDetailsScrollList.RecipeMaterialInfo()
        {
          gear = materialGear,
          playerQuantity = player_material_quantity,
          neadQuantity = material_count
        });
        this.giftMixerInfo.zennyAmount = (long) this.call_recipe.cost_money;
        this.giftMixerInfo.recipeID = this.call_recipe.ID;
        materialGear = (GearGear) null;
      }
      else
      {
        this.txtMaterialNum[column_num - 1].SetActive(false);
        this.slcMaterialNum_txt[column_num - 1].SetActive(false);
      }
    }
    yield return (object) null;
  }

  private IEnumerator SetCallItemIcon(
    GearGear gear,
    Transform iconTransform,
    bool isGray,
    PlayerMaterialGear player_material,
    int column_num)
  {
    Sea030GiftDetailsScrollList detailsScrollList = this;
    ItemIcon gearIcon = detailsScrollList.ItemIconPrefab.Clone(iconTransform).GetComponent<ItemIcon>();
    Player player = SMManager.Get<Player>();
    if (player_material == (PlayerMaterialGear) null)
    {
      player_material = new PlayerMaterialGear();
      player_material.gear_id = gear.ID;
      player_material.player_id = player.id;
      player_material.quantity = 0;
    }
    IEnumerator e = gearIcon.InitByPlayerMaterialGear(player_material);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    gearIcon.onClick = (Action<ItemIcon>) (_ => this.OnDownItemList(column_num));
    gearIcon.gear.rank.SetActive(false);
    gearIcon.gear.unlimit.SetActive(false);
    gearIcon.Gray = isGray;
    gearIcon.EnableQuantity(player_material.quantity);
    if (column_num == 0)
      gearIcon.EnableLongPressEvent(true, new Action<ItemIcon>(detailsScrollList.GearDetail));
    else
      gearIcon.EnableLongPressEvent(true, new Action<ItemIcon>(detailsScrollList.OnMaterialPopup));
  }

  private void GearDetail(ItemIcon itemIcon)
  {
    Sea030GiftListMenu.isCallGiftRecipeWindowOpen = false;
    Bugu00561Scene.changeScene(true, itemIcon.ItemInfo);
  }

  private void OnMaterialPopup(ItemIcon itemIcon)
  {
    GearGear gear = (GearGear) null;
    MasterData.GearGear.TryGetValue(itemIcon.ItemInfo.gear.ID, out gear);
    PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.gear_id == itemIcon.ItemInfo.gear.ID));
    int quantity = 0;
    if (playerMaterialGear != (PlayerMaterialGear) null)
      quantity = playerMaterialGear.quantity;
    this.StartCoroutine(this.SetMaterialPopup(gear, quantity));
  }

  private IEnumerator SetMaterialPopup(GearGear gear, int quantity)
  {
    Future<GameObject> prefabF = Res.Prefabs.bugu005_3.popup_005_3_14__anim_popup01.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    e = Singleton<PopupManager>.GetInstance().open(result).GetComponent<Bugu0053MaterialPopup>().CallItemInit(gear, this.ItemIconPrefab, quantity);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private void OnDownItemList(int column_num)
  {
    int num = this.nowColumn == column_num ? 1 : 0;
    this.nowColumn = column_num;
    if (num != 0)
      return;
    GearGear callItem;
    if (column_num != 0)
      MasterData.GearGear.TryGetValue((int) this.call_recipe.GetType().GetField("material{0}_gear_id_GearGear".F((object) column_num)).GetValue((object) this.call_recipe), out callItem);
    else
      callItem = this.call_item;
    this.StartCoroutine(this.ScrollContainerMenu.AddChildScrollList(callItem, this.giftMixerInfo.parentGear, this.nowColumn, this.myLine));
  }

  public void Ondecide() => this.StartCoroutine(this.popupCallItemMixer());

  private IEnumerator popupCallItemMixer()
  {
    Sea030GiftDetailsScrollList detailsScrollList = this;
    if (!((IEnumerable<int>) Sea030GiftListMenu.playerRecipeIDList).Contains<int>(detailsScrollList.call_recipe.ID))
    {
      Singleton<NGMessageUI>.GetInstance().SetMessageByPosType("特定の誓約ミッションクリアで解放");
    }
    else
    {
      GearGear gearGear = (GearGear) null;
      MasterData.GearGear.TryGetValue(detailsScrollList.call_recipe.failure_gear_id_GearGear, out gearGear);
      detailsScrollList.giftMixerInfo.failureGear = gearGear;
      detailsScrollList.giftMixerInfo.failureGearGiveCount = detailsScrollList.call_recipe.failure_gear_count;
      // ISSUE: reference to a compiler-generated method
      PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).FirstOrDefault<PlayerMaterialGear>(new Func<PlayerMaterialGear, bool>(detailsScrollList.\u003CpopupCallItemMixer\u003Eb__38_0));
      detailsScrollList.giftMixerInfo.playerFailureGearCount = !(playerMaterialGear == (PlayerMaterialGear) null) ? playerMaterialGear.quantity : 0;
      long zenny = SMManager.Get<Player>().money;
      Future<GameObject> popupGiftMixer = detailsScrollList.giftMixerInfo.failureGear != null ? new ResourceObject("Prefabs/popup/popup_sea030_giftMixer").Load<GameObject>() : new ResourceObject("Prefabs/popup/popup_sea030_giftMixer_short").Load<GameObject>();
      IEnumerator e = popupGiftMixer.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sea030PopUpGiftMixer component = Singleton<PopupManager>.GetInstance().open(popupGiftMixer.Result).GetComponent<Sea030PopUpGiftMixer>();
      detailsScrollList.StartCoroutine(component.Init(detailsScrollList.giftMixerInfo, zenny, detailsScrollList.ItemIconPrefab, detailsScrollList.ScrollContainerMenu));
    }
  }

  public enum ListType
  {
    Parent = 1,
    Child = 2,
    Tips = 3,
  }

  public class RecipeMaterialInfo
  {
    public GearGear gear;
    public int playerQuantity;
    public int neadQuantity;
  }

  public class GiftMixerInfo
  {
    public GearGear successGear;
    public int playerSuccessGearCount;
    public int successRatio;
    public GearGear failureGear;
    public int playerFailureGearCount;
    public int failureGearGiveCount;
    public long zennyAmount;
    public int recipeID;
    public List<Sea030GiftDetailsScrollList.RecipeMaterialInfo> materialGear = new List<Sea030GiftDetailsScrollList.RecipeMaterialInfo>();
    public GearGear parentGear;
  }
}

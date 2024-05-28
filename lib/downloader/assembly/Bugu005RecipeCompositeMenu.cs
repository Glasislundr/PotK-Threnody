// Decompiled with JetBrains decompiler
// Type: Bugu005RecipeCompositeMenu
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
public class Bugu005RecipeCompositeMenu : BackButtonMenuBase
{
  private GearGear gear;
  private List<GameCore.ItemInfo> gears;
  private List<GearCombineRecipe> allGearRecipes;
  private Bugu00510Menu menu;
  private int thisRecipe;
  [Header("Transform")]
  [SerializeField]
  private Transform buguContainer;
  [SerializeField]
  private UIGrid grid;
  [Header("Label")]
  [SerializeField]
  private UILabel txtDescription;
  [SerializeField]
  private UILabel txtRecipeName;
  [SerializeField]
  private UILabel txtZenyAmount;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtCount;
  [SerializeField]
  private UILabel txtRightCount;
  [SerializeField]
  private UILabel txtCost;
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton[] sliderButtons;
  private int maxCount;
  private int selectedCount = 1;
  private int sliderCount = 1;
  private int amount;
  private List<InventoryItem> inventoryItems;
  private bool rankMoreTwo;
  private bool rankMoreTwoUnique;
  [Header("Button")]
  [SerializeField]
  private SpreadColorButton changeMatButton;
  public SpreadColorButton yesButton;
  private GameObject nowPopup;

  public IEnumerator Init(
    Bugu00510Menu m,
    GearGear g,
    List<GameCore.ItemInfo> gs,
    List<GearCombineRecipe> gearRecipes,
    int zenyAmount,
    int count,
    List<InventoryItem> itemsList)
  {
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) this.slider).enabled = false;
    this.menu = m;
    this.gear = g;
    this.gears = gs;
    this.allGearRecipes = gearRecipes;
    this.amount = zenyAmount;
    this.thisRecipe = gearRecipes[0].ID;
    this.inventoryItems = itemsList;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefab = prefabF.Result;
    prefabF = Res.Prefabs.bugu005_10.dir_MaterialNum.Load<GameObject>();
    e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject prefabMaterialNum = prefabF.Result;
    this.maxCount = count;
    this.maxCount = Mathf.Min(this.maxCount, 99);
    long money = SMManager.Get<Player>().money;
    while (money <= (long) (this.maxCount * this.amount))
      --this.maxCount;
    if (this.maxCount < 1)
      this.maxCount = 1;
    if (this.maxCount <= 1)
    {
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
      ((Behaviour) this.slider).enabled = false;
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
      this.txtSelectMin.text = "0";
      foreach (UIButtonColor sliderButton in this.sliderButtons)
        sliderButton.isEnabled = false;
      this.selectedCount = 1;
      this.sliderCount = 1;
    }
    else
    {
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = true;
      ((Behaviour) this.slider).enabled = true;
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount;
      this.txtSelectMin.text = "1";
      this.selectedCount = 1;
      this.sliderCount = this.selectedCount - 1;
    }
    this.UpdateInfo();
    e = this.SetMainIcon(this.gear, prefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetIcon(this.gears, prefab, prefabMaterialNum);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtRecipeName.SetTextLocalize(this.gear.name);
    this.txtZenyAmount.SetTextLocalize(this.amount);
    this.txtCost.SetTextLocalize((this.amount * this.selectedCount).ToString());
    this.txtSelectMax.SetTextLocalize(this.maxCount.ToString());
  }

  private IEnumerator SetMainIcon(GearGear gear, GameObject iconPrefab)
  {
    IEnumerator e = iconPrefab.Clone(this.buguContainer).GetComponent<ItemIcon>().InitByGear(gear, gear.GetElement());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator SetIcon(
    List<GameCore.ItemInfo> gears,
    GameObject iconPrefab,
    GameObject materialNumPrefab)
  {
    foreach (GameCore.ItemInfo gear1 in gears)
    {
      GameCore.ItemInfo gear = gear1;
      GameObject gameObject1 = iconPrefab.Clone(((Component) this.grid).transform);
      GameObject gameObject2 = materialNumPrefab.Clone(gameObject1.transform);
      int count = 0;
      this.inventoryItems.ForEach((Action<InventoryItem>) (x =>
      {
        if (x.Item.masterID != gear.masterID)
          return;
        if (x.Item.isWeapon)
          ++count;
        else
          count += x.Item.quantity;
      }));
      ((Component) gameObject2.transform.Find("txt_Num")).GetComponentInChildren<UILabel>().SetTextLocalize(count.ToString());
      if (!this.rankMoreTwo)
        this.rankMoreTwo = this.inventoryItems.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.masterID == gear.masterID && x.Item.gearLevel >= 2));
      if (!this.rankMoreTwoUnique && gear.gearLevel >= 2)
        this.rankMoreTwoUnique = true;
      ItemIcon IconScript = gameObject1.GetComponent<ItemIcon>();
      IEnumerator e = IconScript.InitByItemInfo(gear);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      IconScript.ShowInRecipe();
      IconScript = (ItemIcon) null;
    }
    this.grid.Reposition();
  }

  public void IbtnChangeMaterial()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0, true);
    Singleton<NGSceneManager>.GetInstance().changeScene("bugu005_recipe_composite_material_select", true, (object) this.gear, (object) this.gears, (object) this.allGearRecipes);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * ((float) this.maxCount - 1f));
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.selectedCount = this.maxCount != 1 ? this.sliderCount + 1 : 1;
    ((UIButtonColor) this.yesButton).isEnabled = this.selectedCount > 0;
    ((UIButtonColor) this.changeMatButton).isEnabled = this.selectedCount == 1;
    this.txtCount.SetTextLocalize(this.selectedCount);
    this.txtRightCount.SetTextLocalize(this.selectedCount);
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / ((float) this.maxCount - 1f);
    this.txtCost.SetTextLocalize((this.amount * this.selectedCount).ToString());
  }

  public void IbtnDecrease()
  {
    --this.sliderCount;
    if (this.sliderCount <= 0)
      this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnIncrease()
  {
    ++this.sliderCount;
    if (this.sliderCount >= this.maxCount - 1)
      this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnSetMin()
  {
    this.sliderCount = 0;
    this.UpdateInfo();
  }

  public void IbtnSetMax()
  {
    this.sliderCount = this.maxCount - 1;
    this.UpdateInfo();
  }

  public void IbtnYes()
  {
    if (this.IsPushAndSet())
      return;
    if (this.selectedCount == 1)
    {
      if (this.rankMoreTwoUnique)
        this.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().Bugu005RecipeCompositeMenu_Title, Consts.GetInstance().Bugu005RecipeCompositeMenu_Message, (Action) (() => this.CreateOneGears()), (Action) (() =>
        {
          this.IsPush = false;
          this.ConfirmationNoBtn();
        }))).gameObject;
      else
        this.CreateOneGears();
    }
    else if (this.rankMoreTwo)
      this.nowPopup = ((Component) ModalWindow.ShowYesNo(Consts.GetInstance().Bugu005RecipeCompositeMenu_Title, Consts.GetInstance().Bugu005RecipeCompositeMenu_Message, (Action) (() => this.CreateGearsWithMultipleMaterials()), (Action) (() =>
      {
        this.IsPush = false;
        this.ConfirmationNoBtn();
      }))).gameObject;
    else
      this.CreateGearsWithMultipleMaterials();
  }

  private void CloseThisPopup()
  {
    ((Component) this.menu.dirRecipe).gameObject.SetActive(true);
    foreach (Component component in Object.FindObjectsOfType<Bugu0053DirRecipePopup>())
      component.gameObject.GetComponent<NGTweenParts>().isActive = false;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  private void CreateOneGears()
  {
    this.CloseThisPopup();
    this.StartCoroutine(this.menu.CompositeAPI(this.gears));
  }

  private void CreateGearsWithMultipleMaterials()
  {
    this.CloseThisPopup();
    this.StartCoroutine(this.menu.CompositeMultipleAPI(this.thisRecipe, this.selectedCount));
  }

  private void ConfirmationNoBtn() => Object.Destroy((Object) this.nowPopup);

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    ((Component) this.menu.dirRecipe).gameObject.SetActive(true);
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}

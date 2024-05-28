// Decompiled with JetBrains decompiler
// Type: PopupReisouCreation
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
public class PopupReisouCreation : BackButtonMenuBase
{
  private GearReisouChaosCreation recipe;
  [Header("Icon")]
  [SerializeField]
  private GameObject dirIcon;
  [Header("Label")]
  [SerializeField]
  private UILabel txtSand;
  [SerializeField]
  private UILabel txtMedal;
  [Header("Slider")]
  [SerializeField]
  private UILabel txtCount;
  [SerializeField]
  private UILabel txtSandCost;
  [SerializeField]
  private UILabel txtMedalCost;
  [SerializeField]
  private UILabel txtSelectMin;
  [SerializeField]
  private UILabel txtSelectMax;
  [SerializeField]
  private UISlider slider;
  [SerializeField]
  private UIButton[] sliderButtons;
  private const int maxCountLimit = 100;
  private int maxCount;
  private int selectedCount = 1;
  private int sliderCount = 1;
  private int sandAmount;
  private int medalAmount;
  [Header("Button")]
  public SpreadColorButton yesButton;
  protected Action cbCreation;

  public IEnumerator Init(GearReisouChaosCreation recipe, Action cbCreation)
  {
    ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
    ((Behaviour) this.slider).enabled = false;
    this.recipe = recipe;
    this.sandAmount = recipe.cost_sand;
    this.medalAmount = recipe.cost_medal;
    this.cbCreation = cbCreation;
    Future<GameObject> prefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = prefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject result = prefabF.Result;
    this.maxCount = 100;
    int maxReisouItems = SMManager.Get<Player>().max_reisou_items;
    PlayerItem[] array = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.entity_type == MasterDataTable.CommonRewardType.gear && x.isReisou())).ToArray<PlayerItem>();
    this.maxCount = Mathf.Min(this.maxCount, SMManager.Get<Player>().max_reisou_items - array.Length);
    long pSand = (long) SMManager.Get<Player>().reisou_jewel;
    long pMedal = (long) SMManager.Get<Player>().battle_medal;
    while (pSand < (long) (this.maxCount * this.sandAmount) || pMedal < (long) (this.maxCount * this.medalAmount))
      --this.maxCount;
    if (this.maxCount <= 1)
    {
      ((UIProgressBar) this.slider).numberOfSteps = this.maxCount + 1;
      ((Behaviour) this.slider).enabled = false;
      ((Collider) ((Component) this.slider).GetComponent<BoxCollider>()).enabled = false;
      this.txtSelectMin.text = "0";
      this.selectedCount = 1;
      this.sliderCount = 1;
      foreach (UIButtonColor sliderButton in this.sliderButtons)
        sliderButton.isEnabled = false;
      if (this.maxCount < 1)
      {
        ((UIButtonColor) this.yesButton).isEnabled = false;
        this.maxCount = 1;
      }
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
    e = this.SetReisouIcon(recipe.chaos_ID, result);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.txtSand.SetTextLocalize(pSand);
    this.txtMedal.SetTextLocalize(pMedal);
    string format = "[ff0000]{0}[-]";
    if (pSand < (long) (this.sandAmount * this.selectedCount))
      this.txtSandCost.SetTextLocalize(format.F((object) (this.sandAmount * this.selectedCount)));
    else
      this.txtSandCost.SetTextLocalize(this.sandAmount * this.selectedCount);
    if (pMedal < (long) (this.medalAmount * this.selectedCount))
      this.txtMedalCost.SetTextLocalize(format.F((object) (this.medalAmount * this.selectedCount)));
    else
      this.txtMedalCost.SetTextLocalize(this.medalAmount * this.selectedCount);
    this.txtSelectMax.SetTextLocalize(this.maxCount.ToString());
  }

  private IEnumerator SetReisouIcon(GearGear gear, GameObject prefab)
  {
    PlayerItem playerItem = new PlayerItem(gear, MasterDataTable.CommonRewardType.gear);
    ItemIcon itemIcon = prefab.Clone(this.dirIcon.transform).GetComponent<ItemIcon>();
    IEnumerator e = itemIcon.InitByPlayerItem(playerItem);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    itemIcon.EnableLongPressEventReisou(playerItem);
    itemIcon.onClick = (Action<ItemIcon>) (item => itemIcon.OpenReisouDetailPopup(item.ItemInfo, playerItem));
  }

  public void OnValueChange()
  {
    this.sliderCount = Mathf.RoundToInt(((UIProgressBar) this.slider).value * ((float) this.maxCount - 1f));
    this.UpdateInfo();
  }

  private void UpdateInfo()
  {
    this.selectedCount = this.maxCount != 1 ? this.sliderCount + 1 : 1;
    this.txtCount.SetTextLocalize(this.selectedCount);
    ((UIProgressBar) this.slider).value = (float) this.sliderCount / ((float) this.maxCount - 1f);
    this.txtSandCost.SetTextLocalize(this.sandAmount * this.selectedCount);
    this.txtMedalCost.SetTextLocalize(this.medalAmount * this.selectedCount);
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
    this.StartCoroutine(this.CallReisouCreationAPI());
  }

  private IEnumerator CallReisouCreationAPI()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(1, true);
    yield return (object) null;
    Future<WebAPI.Response.ItemGearReisouCreation> paramF = WebAPI.ItemGearReisouCreation(this.recipe.ID, this.selectedCount, (Action<WebAPI.Response.UserError>) (error =>
    {
      WebAPI.DefaultUserErrorCallback(error);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e = paramF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (paramF.Result != null)
    {
      PlayerItem playerItem = paramF.Result.player_items[0];
      int create_count = paramF.Result.player_items.Length;
      yield return (object) null;
      Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
      Future<GameObject> popupPrefabF = new ResourceObject("Prefabs/popup/popup_Reisou_Creation_result").Load<GameObject>();
      e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject popup = popupPrefabF.Result.Clone();
      PopupReisouCreationResult component = popup.GetComponent<PopupReisouCreationResult>();
      popup.SetActive(false);
      e = component.Init(playerItem, create_count);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      popup.SetActive(true);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      Action cbCreation = this.cbCreation;
      if (cbCreation != null)
        cbCreation();
    }
  }

  public void IbtnNo()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}

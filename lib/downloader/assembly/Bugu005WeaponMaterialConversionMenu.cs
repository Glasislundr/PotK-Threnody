// Decompiled with JetBrains decompiler
// Type: Bugu005WeaponMaterialConversionMenu
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
public class Bugu005WeaponMaterialConversionMenu : Bugu005SelectItemListMenuBase
{
  [SerializeField]
  protected UIButton DecisionButton;
  [SerializeField]
  protected UILabel NumProsession3;
  [SerializeField]
  protected UILabel NumSelectedCount3;
  private GameObject selectPopupPrefab;
  private Bugu005WeaponMaterialConversionScene.Mode mode = Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial;
  [SerializeField]
  protected UILabel TitleLabel;
  [SerializeField]
  protected UILabel AnnotationLabel;
  [SerializeField]
  protected UISprite spriteButtonTxt;
  [SerializeField]
  protected const int DEFAULT_SELECT_MAX = 99;
  private bool needClearCache = true;

  public IEnumerator Init(Bugu005WeaponMaterialConversionScene.Mode mode)
  {
    Bugu005WeaponMaterialConversionMenu materialConversionMenu = this;
    materialConversionMenu.mode = mode;
    materialConversionMenu.SelectMax = 99;
    switch (materialConversionMenu.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        materialConversionMenu.spriteButtonTxt.ChangeSprite("slc_button_text_material_28pt.png__GUI__button_text__button_text_prefab");
        materialConversionMenu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleWeaponConversion);
        materialConversionMenu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationWeaponConversion);
        break;
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        materialConversionMenu.spriteButtonTxt.ChangeSprite("slc_button_text_bugu_28pt.png__GUI__button_text__button_text_prefab");
        materialConversionMenu.TitleLabel.SetTextLocalize(Consts.GetInstance().TitleWeaponMaterialConversion);
        materialConversionMenu.AnnotationLabel.SetTextLocalize(Consts.GetInstance().AnnotationWeaponMaterialConversion);
        int maxItems = SMManager.Get<Player>().max_items;
        int num = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou()));
        materialConversionMenu.SelectMax = Mathf.Clamp(maxItems - num, 0, materialConversionMenu.SelectMax);
        break;
    }
    // ISSUE: reference to a compiler-generated method
    yield return (object) materialConversionMenu.\u003C\u003En__0();
  }

  public override Persist<Persist.ItemSortAndFilterInfo> GetPersist()
  {
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        return Persist.bugu0052SortAndFilter;
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        return Persist.bugu005WeaponMaterialListSortAndFilter;
      default:
        Debug.LogError((object) "Invalid Mode Selected");
        return Persist.bugu0052SortAndFilter;
    }
  }

  protected override List<PlayerItem> GetItemList()
  {
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        return ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou())).ToList<PlayerItem>();
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        return (List<PlayerItem>) null;
      default:
        return (List<PlayerItem>) null;
    }
  }

  protected override long GetRevisionItemList()
  {
    return this.mode == Bugu005WeaponMaterialConversionScene.Mode.Weapon ? SMManager.Revision<PlayerItem[]>() : 0L;
  }

  protected override List<PlayerMaterialGear> GetMaterialList()
  {
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        return (List<PlayerMaterialGear>) null;
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        return ((IEnumerable<PlayerMaterialGear>) SMManager.Get<PlayerMaterialGear[]>()).Where<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.isWeaponMaterial())).ToList<PlayerMaterialGear>();
      default:
        return (List<PlayerMaterialGear>) null;
    }
  }

  protected override long GetRevisionMaterialList()
  {
    return this.mode == Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial ? SMManager.Revision<PlayerMaterialGear[]>() : 0L;
  }

  protected override bool IsGrayIcon(InventoryItem item)
  {
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        if (item.Item != null && item.Item.isUsedOrHasExpireDateGear)
          return true;
        break;
    }
    return base.IsGrayIcon(item);
  }

  protected override bool DisableTouchIcon(InventoryItem item)
  {
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        if (item.Item != null && item.Item.isUsedOrHasExpireDateGear)
          return true;
        break;
    }
    return base.DisableTouchIcon(item);
  }

  protected override IEnumerator InitExtension()
  {
    Bugu005WeaponMaterialConversionMenu materialConversionMenu = this;
    if (Object.op_Equality((Object) materialConversionMenu.selectPopupPrefab, (Object) null))
    {
      Future<GameObject> prefab = Res.Prefabs.popup.popup_005_weapon_material_select__anim_popup01.Load<GameObject>();
      IEnumerator e = prefab.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      materialConversionMenu.selectPopupPrefab = prefab.Result;
      prefab = (Future<GameObject>) null;
    }
    materialConversionMenu.SelectItemList.Clear();
  }

  protected override void BottomInfoUpdate()
  {
    Player player = SMManager.Get<Player>();
    int num = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Count<PlayerItem>((Func<PlayerItem, bool>) (x => !x.isSupply() && !x.isReisou()));
    int selectItemNum = 0;
    selectItemNum = this.SelectItemList.Count<InventoryItem>((Func<InventoryItem, bool>) (x => x.Item.isWeapon));
    this.SelectItemList.Where<InventoryItem>((Func<InventoryItem, bool>) (x => !x.Item.isWeapon)).ForEach<InventoryItem>((Action<InventoryItem>) (item => selectItemNum += item.selectCount));
    this.NumSelectedCount3.SetTextLocalize(selectItemNum);
    this.NumSelectedCount3.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_0052_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) selectItemNum
      },
      {
        (object) "max",
        (object) this.SelectMax
      }
    }));
    ((UIWidget) this.NumSelectedCount3).color = selectItemNum < this.SelectMax ? Color.white : Color.red;
    this.NumProsession3.SetTextLocalize(Consts.Format(Consts.GetInstance().GEAR_0052_POSSESSION, (IDictionary) new Hashtable()
    {
      {
        (object) "now",
        (object) num
      },
      {
        (object) "max",
        (object) player.max_items
      }
    }));
    ((UIButtonColor) this.DecisionButton).isEnabled = selectItemNum > 0 && this.SelectMax >= selectItemNum;
  }

  protected override void SelectItemProc(GameCore.ItemInfo item)
  {
    InventoryItem byItem = this.InventoryItems.FindByItem(item);
    if (byItem == null)
      return;
    if (byItem.select)
    {
      if (!item.isWeapon)
      {
        this.StartCoroutine(this.CountSelectPopUp(byItem));
      }
      else
      {
        this.RemoveSelectItem(byItem);
        this.UpdateSelectItemIndexWithInfo();
      }
    }
    else
    {
      if (this.SelectItemList.Count >= this.SelectMax)
        return;
      if (!item.isWeapon)
      {
        this.StartCoroutine(this.CountSelectPopUp(byItem));
      }
      else
      {
        this.AddSelectItem(byItem);
        this.UpdateSelectItemIndexWithInfo();
      }
    }
  }

  public virtual void IbtnDecision()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.ExecConvertAgreementPopupProc());
  }

  private IEnumerator CountSelectPopUp(InventoryItem item)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Bugu005WeaponMaterialConversionMenu menu = this;
    GameObject popup;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      popup.SetActive(false);
      Singleton<PopupManager>.GetInstance().open(popup, isCloned: true);
      popup.SetActive(true);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    popup = menu.selectPopupPrefab.Clone();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) popup.GetComponent<Bugu005WeaponMaterialSelectPopup>().InitSceneAsync(item, menu);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  protected IEnumerator ExecConvertAgreementPopupProc()
  {
    bool waitSelect = true;
    bool selectYes = false;
    string title = string.Empty;
    string message = string.Empty;
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        title = Consts.GetInstance().AgreementPopupTitleWeaponConversion;
        message = Consts.GetInstance().AgreementPopupMessageWeaponConversion;
        break;
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        title = Consts.GetInstance().AgreementPopupTitleWeaponMaterialConversion;
        message = Consts.GetInstance().AgreementPopupMessageWeaponMaterialConversion;
        break;
    }
    PopupCommonNoYes.Show(title, message, (Action) (() =>
    {
      selectYes = true;
      waitSelect = false;
    }), (Action) (() =>
    {
      selectYes = false;
      waitSelect = false;
    }), (NGUIText.Alignment) 1);
    yield return (object) new WaitWhile((Func<bool>) (() => waitSelect));
    if (selectYes)
    {
      switch (this.mode)
      {
        case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
          yield return (object) this.ExecConvertToMaterialAPI();
          break;
        case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
          yield return (object) this.ExecConvertToGearAPI();
          break;
      }
    }
  }

  public void SetConvertCount(InventoryItem item, int count)
  {
    item.selectCount = count;
    if (count != 0)
    {
      if (!this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == item)))
        this.AddSelectItem(item);
    }
    else if (this.SelectItemList.Any<InventoryItem>((Func<InventoryItem, bool>) (x => x == item)))
      this.RemoveSelectItem(item);
    this.UpdateSelectItemIndexWithInfo();
  }

  private IEnumerator ExecConvertToMaterialAPI()
  {
    Bugu005WeaponMaterialConversionMenu materialConversionMenu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.ItemGearExchange> future = WebAPI.ItemGearExchange(materialConversionMenu.SelectItemList.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.itemID)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        e1 = materialConversionMenu.Init();
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        yield return (object) new WaitForSeconds(0.5f);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        future = (Future<WebAPI.Response.ItemGearExchange>) null;
      }
    }
  }

  private IEnumerator ExecConvertToGearAPI()
  {
    Bugu005WeaponMaterialConversionMenu materialConversionMenu = this;
    if (!Singleton<CommonRoot>.GetInstance().isLoading)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 1;
      Singleton<CommonRoot>.GetInstance().isLoading = true;
      Future<WebAPI.Response.ItemBodyExchange> future = WebAPI.ItemBodyExchange(materialConversionMenu.SelectItemList.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.Item.masterID)).ToArray<int>(), materialConversionMenu.SelectItemList.Select<InventoryItem, int>((Func<InventoryItem, int>) (x => x.selectCount)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
      {
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        WebAPI.DefaultUserErrorCallback(e);
        MypageScene.ChangeSceneOnError();
      }));
      IEnumerator e1 = future.Wait();
      while (e1.MoveNext())
        yield return e1.Current;
      e1 = (IEnumerator) null;
      if (future.Result != null)
      {
        e1 = materialConversionMenu.Init(materialConversionMenu.mode);
        while (e1.MoveNext())
          yield return e1.Current;
        e1 = (IEnumerator) null;
        yield return (object) new WaitForSeconds(0.5f);
        Singleton<CommonRoot>.GetInstance().isLoading = false;
        Singleton<CommonRoot>.GetInstance().loadingMode = 0;
        future = (Future<WebAPI.Response.ItemBodyExchange>) null;
      }
    }
  }

  protected override void UpdateInventoryItemList()
  {
    List<InventoryItem> list1 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isWeapon || x.Item.isSupply;
    })).ToList<InventoryItem>();
    if (list1 != null && list1.Count<InventoryItem>() > 0)
    {
      List<PlayerItem> itemList = this.GetItemList();
      foreach (InventoryItem inventoryItem in list1)
      {
        InventoryItem invItem = inventoryItem;
        PlayerItem playerItem = itemList.FirstOrDefault<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == invItem.Item.itemID));
        if (playerItem != (PlayerItem) null)
          this.UpdateInvetoryItem(invItem, playerItem);
      }
    }
    List<InventoryItem> list2 = this.InventoryItems.Where<InventoryItem>((Func<InventoryItem, bool>) (x =>
    {
      if (x.Item == null || x.removeButton)
        return false;
      return x.Item.isCompse || x.Item.isExchangable;
    })).ToList<InventoryItem>();
    if (list2 != null && list2.Count<InventoryItem>() > 0)
    {
      PlayerMaterialGear[] source = SMManager.Get<PlayerMaterialGear[]>();
      foreach (InventoryItem inventoryItem in list2)
      {
        InventoryItem invItem = inventoryItem;
        PlayerMaterialGear playerMaterialGear = ((IEnumerable<PlayerMaterialGear>) source).FirstOrDefault<PlayerMaterialGear>((Func<PlayerMaterialGear, bool>) (x => x.id == invItem.Item.itemID));
        if (playerMaterialGear != (PlayerMaterialGear) null)
          this.UpdateInvetoryItem(invItem, playerMaterialGear);
      }
    }
    this.SelectItemList.ForEachIndex<InventoryItem>((Action<InventoryItem, int>) ((x, idx) =>
    {
      x.select = true;
      x.Gray = true;
      if (this.SelectMode != Bugu005SelectItemListMenuBase.SelectModeEnum.Num)
        return;
      x.index = idx + 1;
    }));
    this.DisplayIconAndBottomInfoUpdate();
    this.isUpdateIcon = true;
  }

  protected virtual void OnEnable()
  {
    if (!this.scroll.scrollView.isDragging)
      return;
    this.scroll.scrollView.Press(false);
  }

  public override void onEndScene()
  {
    base.onEndScene();
    Persist.sortOrder.Flush();
    if (!this.needClearCache)
      return;
    ItemIcon.ClearCache();
  }

  public override void IbtnBack()
  {
    this.needClearCache = false;
    base.IbtnBack();
  }

  public void IbtnStorage()
  {
    if (this.IsPushAndSet())
      return;
    Singleton<NGSceneManager>.GetInstance().destroyCurrentScene();
    switch (this.mode)
    {
      case Bugu005WeaponMaterialConversionScene.Mode.Weapon:
        Bugu005WeaponMaterialConversionScene.ChangeScene(false, Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial);
        break;
      case Bugu005WeaponMaterialConversionScene.Mode.WeaponMaterial:
        Bugu005WeaponMaterialConversionScene.ChangeScene(false, Bugu005WeaponMaterialConversionScene.Mode.Weapon);
        break;
    }
  }
}

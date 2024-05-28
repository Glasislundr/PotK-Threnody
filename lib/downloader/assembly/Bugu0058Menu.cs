// Decompiled with JetBrains decompiler
// Type: Bugu0058Menu
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
public class Bugu0058Menu : BackButtonMenuBase
{
  private const int MAX_SELECT = 5;
  [SerializeField]
  private GearNameTypeRarity gearHeader;
  [SerializeField]
  private UI2DSprite gearRank;
  [SerializeField]
  private Sprite[] gearRankList;
  [SerializeField]
  private UI2DSprite gearImg;
  [SerializeField]
  private GearStatusUpParam gearHP;
  [SerializeField]
  private GearStatusUpParam gearSTR;
  [SerializeField]
  private GearStatusUpParam gearMGC;
  [SerializeField]
  private GearStatusUpParam gearDEF;
  [SerializeField]
  private GearStatusUpParam gearMND;
  [SerializeField]
  private GearStatusUpParam gearSPD;
  [SerializeField]
  private GearStatusUpParam gearTEC;
  [SerializeField]
  private GearStatusUpParam gearLUC;
  [SerializeField]
  private List<GearAddMaterial> gearMaterialList = new List<GearAddMaterial>();
  [SerializeField]
  private UILabel lblSpendZeny;
  [SerializeField]
  private UIButton upgradeButton;
  [SerializeField]
  private UIButton autoSelectButton;
  [SerializeField]
  private bool resetFlag;
  private GameCore.ItemInfo gearEntryBase;
  private List<GameCore.ItemInfo> gearEntryMaterial = new List<GameCore.ItemInfo>();
  private List<GameCore.ItemInfo> AutoSelectList = new List<GameCore.ItemInfo>();
  private List<PlayerItem> UpdatePlayerItem;

  public void IbtnBack()
  {
    if (this.IsPushAndSet())
      return;
    this.backScene();
  }

  public override void onBackButton() => this.IbtnBack();

  public void IbtnChageBugu()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00522Scene.ChangeScene(true);
  }

  public void IbtnMaterialSelect()
  {
    if (this.IsPushAndSet())
      return;
    Bugu00523Scene.ChangeScene(false, this.gearEntryMaterial, this.gearEntryBase);
  }

  public void IbtnPakuPaku()
  {
    if (this.IsPushAndSet())
      return;
    this.StartCoroutine(this.PakuPakuAPIEvent());
  }

  public IEnumerator PakuPakuAPIEvent()
  {
    Bugu0058Menu bugu0058Menu = this;
    List<GameCore.ItemInfo> SendGears = bugu0058Menu.gearEntryMaterial.ToList<GameCore.ItemInfo>();
    if (SendGears.Where<GameCore.ItemInfo>((Func<GameCore.ItemInfo, bool>) (x => x.gear.rarity.index >= 3)).ToList<GameCore.ItemInfo>().Count >= 1)
    {
      Future<GameObject> popupPrefabF = Res.Prefabs.popup.popup_005_8_3_2.Load<GameObject>();
      IEnumerator e = popupPrefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GameObject result = popupPrefabF.Result;
      GameObject obj = Singleton<PopupManager>.GetInstance().openAlert(result);
      obj.transform.localPosition = new Vector3(0.0f, 110f, 0.0f);
      e = obj.GetComponent<Bugu005382Menu>().ChangeSprite(SendGears);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      obj.GetComponent<Bugu005382Menu>().ChangeText0058();
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Add(obj.GetComponent<Bugu005382Menu>().yesButton.onClick, new EventDelegate.Callback(bugu0058Menu.\u003CPakuPakuAPIEvent\u003Eb__28_1));
      popupPrefabF = (Future<GameObject>) null;
      obj = (GameObject) null;
    }
    else
      bugu0058Menu.StartCoroutine(bugu0058Menu.PakuPakuAPI());
  }

  public IEnumerator PakuPakuAPI()
  {
    int reisouJewelBefore = SMManager.Get<Player>().reisou_jewel;
    Future<WebAPI.Response.ItemGearBuildup> f = WebAPI.ItemGearBuildup(this.gearEntryBase.itemID, this.gearEntryMaterial.Select<GameCore.ItemInfo, int>((Func<GameCore.ItemInfo, int>) (x => x.itemID)).ToArray<int>(), (Action<WebAPI.Response.UserError>) (e =>
    {
      WebAPI.DefaultUserErrorCallback(e);
      MypageScene.ChangeSceneOnError();
    }));
    IEnumerator e1 = f.Wait();
    while (e1.MoveNext())
      yield return e1.Current;
    e1 = (IEnumerator) null;
    if (f.Result != null)
    {
      List<GameCore.ItemInfo> numList = new List<GameCore.ItemInfo>((IEnumerable<GameCore.ItemInfo>) this.gearEntryMaterial);
      numList.Add(new GameCore.ItemInfo(f.Result.player_item));
      int addReisouJewel = f.Result.player.reisou_jewel - reisouJewelBefore;
      Bugu00539Scene.ChangeScene(true, new Bugu00539ChangeSceneParam(numList, false, f.Result.animation_pattern, this.gearEntryBase, (PlayerItem) null, (Action) null, addReisouJewel));
      this.UpdatePlayerItem = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).ToList<PlayerItem>();
      this.resetFlag = true;
    }
  }

  private void CreateAutoSelectList()
  {
    this.AutoSelectList = (List<GameCore.ItemInfo>) null;
    this.AutoSelectList = new List<GameCore.ItemInfo>();
    List<Bugu0058Menu.PlayerItemSort> playerItemSortList = new List<Bugu0058Menu.PlayerItemSort>();
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.sword);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.axe);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.spear);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.bow);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.gun);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.staff);
    Bugu0058Menu.GetAutoSelectList(playerItemSortList, 0, GearKindEnum.shield);
    List<Bugu0058Menu.PlayerItemSort> list = playerItemSortList.Where<Bugu0058Menu.PlayerItemSort>((Func<Bugu0058Menu.PlayerItemSort, bool>) (x => x.item.id != this.gearEntryBase.itemID)).Where<Bugu0058Menu.PlayerItemSort>((Func<Bugu0058Menu.PlayerItemSort, bool>) (x => !x.item.broken)).Where<Bugu0058Menu.PlayerItemSort>((Func<Bugu0058Menu.PlayerItemSort, bool>) (x => !x.item.favorite)).Where<Bugu0058Menu.PlayerItemSort>((Func<Bugu0058Menu.PlayerItemSort, bool>) (x => !x.item.ForBattle)).ToList<Bugu0058Menu.PlayerItemSort>();
    int num = 0;
    foreach (Bugu0058Menu.PlayerItemSort playerItemSort in list.OrderBy<Bugu0058Menu.PlayerItemSort, int>((Func<Bugu0058Menu.PlayerItemSort, int>) (x => x.index)).ToList<Bugu0058Menu.PlayerItemSort>())
    {
      if (num < 5)
      {
        GameCore.ItemInfo itemInfo = new GameCore.ItemInfo(playerItemSort.item);
        this.AutoSelectList.Add(itemInfo);
        if (!this.CheckZeny(CalcItemCost.GetBuildupCost(this.AutoSelectList)))
        {
          this.AutoSelectList.Remove(itemInfo);
          break;
        }
        ++num;
      }
      else
        break;
    }
    if (this.AutoSelectList.Count == 0)
      ((UIButtonColor) this.autoSelectButton).isEnabled = false;
    else
      ((UIButtonColor) this.autoSelectButton).isEnabled = true;
  }

  private void IbtnAutoSelect()
  {
    this.InitSpendZeny();
    this.gearEntryMaterial = (List<GameCore.ItemInfo>) null;
    this.gearEntryMaterial = this.AutoSelectList;
    this.StartCoroutine(this.SetMenuAsync(this.gearEntryBase, this.gearEntryMaterial));
  }

  public static void GetAutoSelectList(
    List<Bugu0058Menu.PlayerItemSort> addList,
    int baseParam,
    GearKindEnum gearEnum,
    bool rarityLimit = true)
  {
    int? nullable = ((IEnumerable<GearBuildupLogic>) MasterData.GearBuildupLogicList).FirstIndexOrNull<GearBuildupLogic>((Func<GearBuildupLogic, bool>) (x => x.base_param == baseParam));
    if (!nullable.HasValue)
      return;
    GearBuildupLogic gearBuildupLogic = MasterData.GearBuildupLogicList[nullable.Value];
    int rbl = 0;
    for (int gear_level = 1; gear_level <= gearBuildupLogic.MaterialRankCount(); ++gear_level)
    {
      if (0 < gearBuildupLogic.MaterialRank(gear_level))
      {
        rbl = gear_level;
        break;
      }
    }
    List<PlayerItem> list1 = ((IEnumerable<PlayerItem>) SMManager.Get<PlayerItem[]>()).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear != null)).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear.kind.isEquip)).ToList<PlayerItem>();
    if (rarityLimit)
      list1 = list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear.rarity.index < 3)).OrderBy<PlayerItem, int>((Func<PlayerItem, int>) (x => x.gear.rarity.index)).ToList<PlayerItem>();
    List<PlayerItem> list2 = list1.Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear.kind.Enum == gearEnum)).Where<PlayerItem>((Func<PlayerItem, bool>) (x => x.gear_level >= rbl)).ToList<PlayerItem>();
    for (int index = 0; index < list2.Count; ++index)
      addList.Add(new Bugu0058Menu.PlayerItemSort()
      {
        index = index,
        item = list2[index]
      });
  }

  public IEnumerator SetMenuAsync(GameCore.ItemInfo gearData, List<GameCore.ItemInfo> gearMaterialData)
  {
    Singleton<PopupManager>.GetInstance().closeAll();
    this.Init();
    if (this.UpdatePlayerItem == null)
    {
      this.gearEntryBase = gearData;
      this.gearEntryMaterial = gearMaterialData;
    }
    else
    {
      List<GameCore.ItemInfo> itemInfoList = new List<GameCore.ItemInfo>();
      foreach (GameCore.ItemInfo itemInfo in gearMaterialData)
      {
        GameCore.ItemInfo gear = itemInfo;
        int? nullable = this.UpdatePlayerItem.FirstIndexOrNull<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == gear.itemID));
        if (nullable.HasValue)
          itemInfoList.Add(new GameCore.ItemInfo(this.UpdatePlayerItem[nullable.Value]));
      }
      int? nullable1 = this.UpdatePlayerItem.FirstIndexOrNull<PlayerItem>((Func<PlayerItem, bool>) (x => x.id == gearData.itemID));
      this.gearEntryBase = (GameCore.ItemInfo) null;
      if (nullable1.HasValue)
        this.gearEntryBase = new GameCore.ItemInfo(this.UpdatePlayerItem[nullable1.Value]);
      this.gearEntryMaterial = (List<GameCore.ItemInfo>) null;
      this.gearEntryMaterial = itemInfoList;
    }
    if (this.gearEntryBase != null)
    {
      this.CreateAutoSelectList();
      this.SetGearRank(this.gearEntryBase);
      IEnumerator e = this.SetGearImg(this.gearEntryBase);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetGearHeader(this.gearEntryBase);
      if (this.resetFlag)
      {
        this.resetFlag = false;
        this.gearEntryMaterial.Clear();
      }
      this.SetGearStatusUpParam(this.gearEntryBase, this.gearEntryMaterial);
      e = this.SetGearMaterialList(this.gearEntryBase, this.gearEntryMaterial);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.SetCalcSpendZeny(this.gearEntryBase, this.gearEntryMaterial);
    }
  }

  private void Init()
  {
    this.InitGearHeader();
    this.InitGearStatusUpParam();
    this.InitGearRank();
    this.InitSpendZeny();
    this.InitGearMaterialList();
  }

  private IEnumerator SetGearImg(GameCore.ItemInfo gearData)
  {
    Future<Sprite> spriteF = gearData.gear.LoadSpriteBasic();
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.gearImg.sprite2D = spriteF.Result;
    UI2DSprite gearImg1 = this.gearImg;
    Rect textureRect1 = spriteF.Result.textureRect;
    int num1 = Mathf.FloorToInt(((Rect) ref textureRect1).width);
    ((UIWidget) gearImg1).width = num1;
    UI2DSprite gearImg2 = this.gearImg;
    Rect textureRect2 = spriteF.Result.textureRect;
    int num2 = Mathf.FloorToInt(((Rect) ref textureRect2).height);
    ((UIWidget) gearImg2).height = num2;
  }

  private void InitGearHeader() => this.gearHeader.Init();

  private void SetGearHeader(GameCore.ItemInfo gearData) => this.gearHeader.Set(gearData);

  private void InitGearStatusUpParam()
  {
    this.gearHP.Init();
    this.gearSTR.Init();
    this.gearMGC.Init();
    this.gearDEF.Init();
    this.gearMND.Init();
    this.gearSPD.Init();
    this.gearTEC.Init();
    this.gearLUC.Init();
  }

  private void SetGearStatusUpParam(GameCore.ItemInfo gearBaseData, List<GameCore.ItemInfo> gearMaterialData)
  {
    this.gearHP.SetCalcStatus(gearBaseData.gear.hp_buildup_limit, gearBaseData.gear.hp_incremental, 0, gearMaterialData, GearKindEnum.shield);
    this.gearSTR.SetCalcStatus(gearBaseData.gear.strength_buildup_limit, gearBaseData.gear.strength_incremental, 0, gearMaterialData, GearKindEnum.axe);
    this.gearMGC.SetCalcStatus(gearBaseData.gear.intelligence_buildup_limit, gearBaseData.gear.intelligence_incremental, 0, gearMaterialData, GearKindEnum.gun);
    this.gearDEF.SetCalcStatus(gearBaseData.gear.vitality_buildup_limit, gearBaseData.gear.vitality_incremental, 0, gearMaterialData, GearKindEnum.spear);
    this.gearMND.SetCalcStatus(gearBaseData.gear.mind_buildup_limit, gearBaseData.gear.mind_incremental, 0, gearMaterialData, GearKindEnum.staff);
    this.gearSPD.SetCalcStatus(gearBaseData.gear.agility_buildup_limit, gearBaseData.gear.agility_incremental, 0, gearMaterialData, GearKindEnum.sword);
    this.gearTEC.SetCalcStatus(gearBaseData.gear.dexterity_buildup_limit, gearBaseData.gear.dexterity_incremental, 0, gearMaterialData, GearKindEnum.bow);
    this.gearLUC.SetCalcStatus(gearBaseData.gear.lucky_buildup_limit, gearBaseData.gear.lucky_incremental, 0, gearMaterialData, GearKindEnum.accessories);
  }

  private void InitGearMaterialList()
  {
    this.gearMaterialList.ForEach((Action<GearAddMaterial>) (x => x.Init()));
  }

  private IEnumerator SetGearMaterialList(GameCore.ItemInfo gearData, List<GameCore.ItemInfo> gearMaterialData)
  {
    for (int i = 0; i < this.gearEntryMaterial.Count; ++i)
    {
      IEnumerator e = this.gearMaterialList[i].Set(gearData, gearMaterialData[i]);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  private void InitGearRank() => ((Component) this.gearRank).gameObject.SetActive(false);

  private void InitSpendZeny()
  {
    this.SetSpendZeny(0);
    ((UIButtonColor) this.upgradeButton).isEnabled = false;
  }

  private void SetGearRank(GameCore.ItemInfo gear)
  {
    ((Component) this.gearRank).gameObject.SetActive(true);
    this.gearRank.sprite2D = this.gearRankList[gear.gearLevel - 1];
  }

  private void SetCalcSpendZeny(GameCore.ItemInfo gearData, List<GameCore.ItemInfo> gearMaterialData)
  {
    int buildupCost = CalcItemCost.GetBuildupCost(gearMaterialData);
    if (!this.CheckZeny(buildupCost))
      return;
    this.SetSpendZeny(buildupCost);
    ((UIButtonColor) this.upgradeButton).isEnabled = true;
  }

  private void SetSpendZeny(int spendZeny) => this.lblSpendZeny.SetTextLocalize(spendZeny);

  private bool CheckZeny(int useZeny) => useZeny != 0 && SMManager.Get<Player>().CheckZeny(useZeny);

  [Serializable]
  public class PlayerItemSort
  {
    public int index;
    public PlayerItem item;
  }
}

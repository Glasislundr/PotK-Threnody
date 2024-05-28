// Decompiled with JetBrains decompiler
// Type: UniqueIcons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class UniqueIcons : IconPrefabBase
{
  [SerializeField]
  private GameObject item;
  [SerializeField]
  private GameObject backGround;
  [SerializeField]
  private GameObject labelBase;
  [SerializeField]
  private bool isLoadedNumbers;
  [SerializeField]
  private Sprite[] numbers;
  [SerializeField]
  private Sprite equals;
  [SerializeField]
  private GameObject[] numbersObj;
  [SerializeField]
  private GameObject[] numbers10Obj;
  public Sprite[] backSprite;
  public string spriteName = "Item_Icon_Kiseki";
  private string kiseki = "Item_Icon_Kiseki";
  private string zeny = "Item_Icon_Zeny";
  private string medal = "Item_Icon_Medal";
  private string point = "Item_Icon_Point";
  private string key = "Item_Icon_Key";
  private string ticket = "Item_Icon_GachaTicket";
  private string season_ticket = "thum";
  [SerializeField]
  private UILabel label;
  public string itemName = "";
  private int width = 24;
  private int height = 28;
  private float scale = 0.8f;
  private LongPressButton button_;
  private Action<UniqueIcons> onClick_;
  private Action<UniqueIcons> onLongPress_;

  public static void ClearCache()
  {
  }

  public bool BackGroundActivated
  {
    get => this.backGround.activeSelf;
    set => this.backGround.SetActive(value);
  }

  public bool LabelActivated
  {
    get => this.labelBase.activeSelf;
    set => this.labelBase.SetActive(value);
  }

  private IEnumerator InitNumber()
  {
    if (!this.isLoadedNumbers)
    {
      for (int index = 0; index < this.numbers.Length; ++index)
      {
        if (Object.op_Equality((Object) this.numbers[index], (Object) null))
          this.numbers[index] = Resources.Load<Sprite>("Icons/slc_Number" + index.ToString());
      }
      this.isLoadedNumbers = true;
    }
    if (Object.op_Equality((Object) this.equals, (Object) null))
    {
      Future<Sprite> equalsF = Singleton<ResourceManager>.GetInstance().Load<Sprite>("Icons/slc_equals");
      IEnumerator e = equalsF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.equals = equalsF.Result;
      equalsF = (Future<Sprite>) null;
    }
  }

  public IEnumerator Set(int num = 0)
  {
    IEnumerator e;
    switch (num)
    {
      case 0:
        e = this.SetKiseki(5);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case 1:
        e = this.SetZeny(1500);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case 2:
        e = this.SetMedal(30);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case 3:
        e = this.SetPoint(500);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  public IEnumerator SetKiseki(int count = 0, bool isPaid = false)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Kiseki.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    string _itemName = isPaid ? Consts.GetInstance().UNIQUE_ICON_PAID_KISEKI : Consts.GetInstance().UNIQUE_ICON_KISEKI;
    this.SetTexture(result, _itemName, this.kiseki);
  }

  public IEnumerator SetMedal(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Medal.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_MEDAL, this.medal);
  }

  public IEnumerator SetZeny(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Zeny.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_ZENY, this.zeny);
  }

  public IEnumerator SetKey(int id, int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = MasterData.QuestkeyQuestkey[id].LoadSpriteThumbnail();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().QUEST_KEY_ITEM];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_KEY, this.key);
  }

  public IEnumerator SetPoint(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Point.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_POINT, this.point);
  }

  public IEnumerator SetBattleMedal(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_BattleMedal.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_BATTLE_MEDAL, this.point);
  }

  public IEnumerator SetAwakeSkill(int id = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = MasterData.BattleskillSkill[id].LoadBattleSkillIcon();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    ((Behaviour) this.backGround.GetComponent<UI2DSprite>()).enabled = false;
    this.SetTexture(result, MasterData.BattleskillSkill[id].name, this.point, true);
    this.item.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
    this.item.transform.localPosition = new Vector3(-2f, 8f, 0.0f);
  }

  public IEnumerator SetTowerMedal(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_TowerMedal.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_TOWER_MEDAL, this.point);
  }

  public IEnumerator SetRaidMedal(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = new ResourceObject("Icons/Item_Icon_RaidJuel").Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_RAID_MEDAL, this.point);
  }

  public IEnumerator SetRecoveryItemIconImage(int rewardID, int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = new ResourceObject("AssetBundle/Resources/recovery_item/{0}/2D/item_thum".F((object) rewardID)).Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.item.GetComponent<UI2DSprite>().sprite2D = result;
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_AP_RECOVERY_ITEM, this.point);
  }

  public IEnumerator SetGachaTicket(int count = 0, int id = 0)
  {
    IEnumerator e;
    if (id <= 0)
    {
      e = this.SetCommonGachaTicket(count);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    else
    {
      e = this.InitNumber();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      GachaTicket data = MasterData.GachaTicket[id];
      Future<Sprite> spriteF = data.LoadSpriteF();
      e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = spriteF.Result;
      this.SetNumber(count);
      this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
      this.SetTexture(result, data.short_name, this.ticket);
    }
  }

  public IEnumerator SetSeasonTicket(int count = 0, int id = 1)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SeasonTicketSeasonTicket data = MasterData.SeasonTicketSeasonTicket[id];
    Future<Sprite> spriteF = data.LoadThumneilF();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, data.name, this.season_ticket);
  }

  public IEnumerator SetPlayerExp(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_PLAYEREXP, this.point);
  }

  public IEnumerator SetUnitExp(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_UNITEXP, this.point);
  }

  public IEnumerator SetApRecover(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_APRECOVER, this.point);
  }

  public IEnumerator SetDpRecover(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_DPRECOVER, this.point);
  }

  public IEnumerator SetMaxUnit(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_MAXUNIT, this.point);
  }

  public IEnumerator SetMaxItem(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_MAXITEM, this.point);
  }

  public IEnumerator SetCpRecover(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_CPRECOVER, this.point);
  }

  public IEnumerator SetMpRecover(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count, false);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_MPRECOVER, this.point);
  }

  public IEnumerator SetStamp(int count)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_STAMP, this.point);
  }

  public IEnumerator SetEmblem()
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(0);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_EMBLEM, this.point);
  }

  public IEnumerator SetKillersTicket(int id, int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SelectTicket data = MasterData.SelectTicket[id];
    Future<Sprite> spriteF = data.createLoaderThumb();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, data.short_name, this.ticket);
  }

  public IEnumerator SetMaterialTicket(int id, int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SelectTicket data = MasterData.SelectTicket[id];
    Future<Sprite> spriteF = data.createLoaderThumb();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, data.short_name, this.ticket);
  }

  public IEnumerator SetMaterialPack(int ticketID, int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SelectTicket data = MasterData.SelectTicket[ticketID];
    string format = "MaterialPack/{0}/pack";
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format(format, (object) data.packID));
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, data.short_name, this.ticket);
  }

  public IEnumerator SetGuildMap(int id)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("AssetBundle/Resources/GuildMap/{0}/2D/c_thum", (object) id));
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(0);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_GUILD_TOWN, this.point);
    this.labelBase.SetActive(false);
  }

  public IEnumerator SetGuildFacility(int id)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitUnit unitUnit;
    if (MasterData.UnitUnit.TryGetValue(id, out unitUnit))
    {
      Future<Sprite> spriteF = unitUnit.LoadSpriteThumbnail();
      e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      Sprite result = spriteF.Result;
      this.SetNumber(0);
      this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
      this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_GUILD_TOWN, this.point);
      this.labelBase.SetActive(false);
    }
  }

  public IEnumerator SetGuildMedal(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = new ResourceObject("Icons/Item_Icon_GuildMedal").Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_GUILD_MEDAL, this.point);
  }

  public IEnumerator SetReincarnationTypeTicket(int id, int count = 0)
  {
    yield return (object) this.InitNumber();
    UnitTypeTicket data = MasterData.UnitTypeTicket[id];
    Future<Sprite> spriteF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("ReincarnationTypeTicket/{0}/ticket", (object) data.icon_id));
    IEnumerator e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, data.short_name, this.ticket);
  }

  public IEnumerator SetCommonTicket(int id, int count = 0)
  {
    CommonTicket ticketMaster = MasterData.CommonTicket[id];
    if (ticketMaster.icon_id == 0)
    {
      int id1 = ticketMaster.ID;
    }
    else
    {
      int iconId = ticketMaster.icon_id;
    }
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = ticketMaster.LoadIconMSpriteF();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    string uniqueIconEventCoin = Consts.GetInstance().UNIQUE_ICON_EVENT_COIN;
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, uniqueIconEventCoin, this.point);
  }

  public IEnumerator SetItemIconCommonImage()
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_Common.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.item.GetComponent<UI2DSprite>().sprite2D = result;
  }

  private IEnumerator SetCommonGachaTicket(int count = 0)
  {
    IEnumerator e = this.InitNumber();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<Sprite> spriteF = Res.Icons.Item_Icon_GachaTicket.Load<Sprite>();
    e = spriteF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Sprite result = spriteF.Result;
    this.SetNumber(count);
    this.backGround.GetComponent<UI2DSprite>().sprite2D = this.backSprite[Consts.GetInstance().NORMAL_BUGU];
    this.SetTexture(result, Consts.GetInstance().UNIQUE_ICON_TICKET, this.ticket);
  }

  private void SetTexture(Sprite sprite, string _itemName, string _spriteName, bool isSnap = false)
  {
    this.spriteName = _spriteName;
    this.itemName = _itemName;
    this.label.SetText(this.itemName);
    UI2DSprite component = this.item.GetComponent<UI2DSprite>();
    component.sprite2D = sprite;
    if (!isSnap)
      return;
    ((UIWidget) component).SetDimensions(((Texture) sprite.texture).width, ((Texture) sprite.texture).height);
  }

  private void SetNumber(int count, bool equal = true)
  {
    int length = count.ToString().Length;
    if (equal)
      ++length;
    int num1 = count;
    GameObject[] useNumbers;
    if (this.numbersObj.Length < num1.ToString().Length | this.numbersObj.Length < num1.ToString().Length + 1 & equal)
    {
      if (this.numbers10Obj.Length < num1.ToString().Length | this.numbers10Obj.Length < num1.ToString().Length + 1 & equal)
      {
        Debug.LogWarning((object) "桁数オーバーの為、個数を表示できませんでした。");
        return;
      }
      useNumbers = this.numbers10Obj;
    }
    else
      useNumbers = this.numbersObj;
    int index1 = 0;
    if (num1 != 0)
    {
      int index2 = 0;
      while (num1 > 0)
      {
        int num2 = num1 % 10;
        useNumbers[index2].SetActive(true);
        UI2DSprite component = useNumbers[index2].GetComponent<UI2DSprite>();
        component.sprite2D = this.numbers[num2];
        ((UIWidget) component).SetDirty();
        this.ChoiceNumberSize(component, num2);
        num1 /= 10;
        ++index2;
      }
      if (equal)
      {
        useNumbers[index2].SetActive(true);
        UI2DSprite component = useNumbers[index2].GetComponent<UI2DSprite>();
        component.sprite2D = this.equals;
        ((UIWidget) component).height = ((Texture) this.equals.texture).height;
        ((UIWidget) component).width = ((Texture) this.equals.texture).width;
        ++index2;
      }
      index1 = index2;
    }
    for (; index1 < useNumbers.Length; ++index1)
      useNumbers[index1].SetActive(false);
    bool flag = false;
    switch (length)
    {
      case 4:
      case 5:
        flag = true;
        break;
      case 6:
        this.scale *= 0.85f;
        flag = true;
        break;
      case 7:
        this.scale *= 0.7f;
        flag = true;
        break;
    }
    if (!flag)
      return;
    this.SetNumberScale(useNumbers);
  }

  public void SetNumberPositionY(float y)
  {
    foreach (GameObject gameObject in this.numbersObj)
    {
      Transform transform = gameObject.transform;
      transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
    }
  }

  public void SetNumberScale(GameObject[] useNumbers)
  {
    ((IEnumerable<GameObject>) useNumbers).Select<GameObject, UI2DSprite>((Func<GameObject, UI2DSprite>) (x => x.GetComponent<UI2DSprite>())).ForEach<UI2DSprite>((Action<UI2DSprite>) (sprite =>
    {
      ((UIWidget) sprite).width = (int) ((double) ((UIWidget) sprite).width * (double) this.scale);
      ((UIWidget) sprite).height = (int) ((double) ((UIWidget) sprite).height * (double) this.scale);
    }));
    float num = (float) this.width * this.scale;
    for (int index = 1; index < useNumbers.Length; ++index)
    {
      Vector3 localPosition = useNumbers[index].transform.localPosition;
      localPosition.x = useNumbers[index - 1].transform.localPosition.x - num;
      useNumbers[index].transform.localPosition = localPosition;
    }
  }

  public void ChoiceNumberSize(UI2DSprite target, int num)
  {
    int num1 = this.width;
    int height = this.height;
    switch (num)
    {
      case 0:
        num1 = 24;
        break;
      case 1:
        num1 = 16;
        break;
      case 2:
        num1 = 22;
        break;
      case 3:
        num1 = 22;
        break;
      case 4:
        num1 = 24;
        break;
      case 5:
        num1 = 22;
        break;
      case 6:
        num1 = 22;
        break;
      case 7:
        num1 = 22;
        break;
      case 8:
        num1 = 24;
        break;
      case 9:
        num1 = 22;
        break;
    }
    ((UIWidget) target).width = num1;
    ((UIWidget) target).height = height;
  }

  public override bool Gray
  {
    get => this.gray;
    set
    {
      if (this.gray == value)
        return;
      this.gray = value;
      Color color = this.gray ? Color.gray : Color.white;
      ((Component) this).GetComponent<UIWidget>().color = color;
      if (!Object.op_Inequality((Object) this.button_, (Object) null))
        return;
      ((UIButtonColor) this.button_).hover = ((UIButtonColor) this.button_).pressed = ((UIButtonColor) this.button_).defaultColor = color;
    }
  }

  public void CreateButton()
  {
    UIWidget component = ((Component) this).GetComponent<UIWidget>();
    GameObject gameObject = new GameObject();
    ((Object) gameObject).name = "ibtn_button";
    GameObject self = gameObject;
    self.SetParent(((Component) this).gameObject);
    self.AddComponent<UIWidget>().depth = component.depth;
    self.AddComponent<BoxCollider>().size = new Vector3((float) component.width, (float) component.height, 0.0f);
    this.button_ = self.AddComponent<LongPressButton>();
    ((UIButtonColor) this.button_).hover = ((UIButtonColor) this.button_).pressed = ((UIButtonColor) this.button_).defaultColor;
    self.AddComponent<UIDragScrollView>();
  }

  public void DisableButton()
  {
    if (Object.op_Equality((Object) this.button_, (Object) null))
    {
      Transform transform = ((Component) this).transform.Find("ibtn_button");
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      this.button_ = ((Component) transform).GetComponent<LongPressButton>();
    }
    this.button_.onClick.Clear();
    this.button_.onLongPress.Clear();
    ((Collider) ((Component) this.button_).gameObject.GetComponent<BoxCollider>()).enabled = false;
  }

  public Action<UniqueIcons> onClick
  {
    get => this.onClick_;
    set
    {
      this.onClick_ = value;
      if (this.onClick_ == null)
        return;
      EventDelegate.Set(this.button_.onClick, (EventDelegate.Callback) (() => this.onClick_(this)));
    }
  }

  public Action<UniqueIcons> onLongPress
  {
    get => this.onLongPress_;
    set
    {
      this.onLongPress_ = value;
      if (this.onLongPress_ == null)
        return;
      EventDelegate.Set(this.button_.onLongPress, (EventDelegate.Callback) (() => this.onLongPress_(this)));
    }
  }

  public void DisableLongPressEvent()
  {
    this.button_.onClick.Clear();
    this.button_.onLongPress.Clear();
  }
}

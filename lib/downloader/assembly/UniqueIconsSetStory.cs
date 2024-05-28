// Decompiled with JetBrains decompiler
// Type: UniqueIconsSetStory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class UniqueIconsSetStory : MonoBehaviour
{
  [HideInInspector]
  public GameObject LinkPrefab;
  [HideInInspector]
  public Transform LinkParent;
  [SerializeField]
  private UISprite BackGround;
  private int spritewidth = 65;
  private int spriteheight = 65;
  private bool notSizeChange;
  private const int UNIQUE_ICON_SIZE = 100;

  [HideInInspector]
  public string name { get; set; }

  public IEnumerator SetIconUnique(
    string name,
    MasterDataTable.CommonRewardType entity_type,
    int entity_id,
    int quantity,
    Transform parent,
    bool nonSizeChange = false)
  {
    this.notSizeChange = nonSizeChange;
    this.name = name;
    this.LinkParent = parent;
    IEnumerator e;
    switch (entity_type)
    {
      case MasterDataTable.CommonRewardType.unit:
      case MasterDataTable.CommonRewardType.material_unit:
        e = this.unitCreate(entity_id);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.supply:
        e = this.supplyCreate(MasterData.SupplySupply[entity_id], quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.gear:
      case MasterDataTable.CommonRewardType.material_gear:
        e = this.gearCreate(MasterData.GearGear[entity_id], quantity, false);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.money:
        e = this.moneyCreate(quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.coin:
        e = this.kisekiCreate(quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.medal:
        e = this.medalCreate(quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.friend_point:
        e = this.friendpointCreate(quantity);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.awake_skill:
        e = this.awakeSkillCreate(MasterData.BattleskillSkill[entity_id]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
      case MasterDataTable.CommonRewardType.gear_body:
        e = this.gearCreate(MasterData.GearGear[entity_id], quantity, true);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
        break;
    }
  }

  public IEnumerator SetIconUnique(
    string name,
    Transform parent,
    QuestStoryMission story = null,
    QuestExtraMission extra = null)
  {
    IEnumerator e;
    if (story != null)
    {
      e = this.SetIconUnique(name, story.entity_type, story.entity_id, story.quantity, parent);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (extra != null)
    {
      e = this.SetIconUnique(name, extra.entity_type, extra.entity_id, extra.quantity, parent);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
  }

  public IEnumerator unitCreate(int id)
  {
    Future<GameObject> PrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UnitIcon link = PrefabF.Result.CloneAndGetComponent<UnitIcon>(this.LinkParent);
    e = link.SetUnit(MasterData.UnitUnit[id], MasterData.UnitUnit[id].GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AdjustSizeOfIcon(((Component) link).GetComponent<UIWidget>());
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = MasterData.UnitUnit[id].name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator gearCreate(GearGear gear, int quan, bool isWeaponMaterial)
  {
    Future<GameObject> PrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon link = PrefabF.Result.CloneAndGetComponent<ItemIcon>(this.LinkParent);
    e = link.InitByGear(gear, gear.GetElement(), isWeaponMaterial);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AdjustSizeOfIcon(((Component) link).GetComponent<UIWidget>());
    this.SetDepth(((Component) link).gameObject);
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    link.EnableQuantity(quan);
    this.name = gear.name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator supplyCreate(SupplySupply supply, int quan)
  {
    Future<GameObject> PrefabF = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ItemIcon link = PrefabF.Result.CloneAndGetComponent<ItemIcon>(this.LinkParent);
    e = link.InitBySupply(supply);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.AdjustSizeOfIcon(((Component) link).GetComponent<UIWidget>());
    this.SetDepth(((Component) link).gameObject);
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    link.EnableQuantity(quan);
    this.name = supply.name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator moneyCreate(int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetZeny(count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = Consts.GetInstance().UNIQUE_ICON_ZENY;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator kisekiCreate(int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetKiseki(count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = Consts.GetInstance().UNIQUE_ICON_KISEKI;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator friendpointCreate(int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetPoint(count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = Consts.GetInstance().UNIQUE_ICON_POINT;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator medalCreate(int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetMedal(count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = Consts.GetInstance().UNIQUE_ICON_MEDAL;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator keyCreate(int id, int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetKey(id, count);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = MasterData.QuestkeyQuestkey[id].name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator ticketCreate(int id, int count)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetGachaTicket(count, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = MasterData.GachaTicket[id].short_name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  public IEnumerator awakeSkillCreate(BattleskillSkill battleSkill)
  {
    Future<GameObject> PrefabF = Res.Icons.UniqueIconPrefab.Load<GameObject>();
    IEnumerator e = PrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    UniqueIcons link = PrefabF.Result.CloneAndGetComponent<UniqueIcons>(this.LinkParent);
    e = link.SetAwakeSkill(battleSkill.ID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!this.notSizeChange)
      link.SetSize(this.spritewidth, this.spriteheight);
    this.name = battleSkill.name;
    this.LinkPrefab = ((Component) link).gameObject;
  }

  private void AdjustSizeOfIcon(UIWidget widget)
  {
    widget.width = 100;
    widget.height = 100;
  }

  private void SetDepth(GameObject iconObj)
  {
    if (Object.op_Equality((Object) this.BackGround, (Object) null))
      return;
    int depth = ((UIWidget) this.BackGround).depth;
    foreach (UI2DSprite componentsInChild in iconObj.GetComponentsInChildren<UI2DSprite>(true))
      ((UIWidget) componentsInChild).depth = ((UIWidget) componentsInChild).depth + depth;
  }
}

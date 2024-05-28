// Decompiled with JetBrains decompiler
// Type: Explore033StatusEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using Explore;
using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Explore033StatusEnemy : Battle0181CharacterStatus
{
  [SerializeField]
  private GameObject mWeakPointRoot;
  [SerializeField]
  private UIGrid mWeakPointIconListAnchor;
  [SerializeField]
  private UIGrid mWeakPointIconGridAnchor;
  private GameObject mWeakPointLblPrefab;

  public override IEnumerator Init(
    BL.UnitPosition up,
    AttackStatus attackStatus,
    int firstAttack,
    bool isColosseum,
    bool isDemoMode)
  {
    Explore033StatusEnemy explore033StatusEnemy = this;
    Future<GameObject> loader = new ResourceObject("Prefabs/explore033_Encount/dir_WeakPoint_Label").Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    explore033StatusEnemy.mWeakPointLblPrefab = loader.Result;
    explore033StatusEnemy.current = up;
    explore033StatusEnemy.maxHp = explore033StatusEnemy.current.unit.initialHp;
    explore033StatusEnemy.currentHp = explore033StatusEnemy.current.unit.exploreHp;
    explore033StatusEnemy.hpGauge.setValue(explore033StatusEnemy.currentHp, explore033StatusEnemy.maxHp, false);
    explore033StatusEnemy.setHPNumbers(explore033StatusEnemy.currentHp.ToString());
    explore033StatusEnemy.txt_consumeHp.SetTextLocalize("");
    explore033StatusEnemy.isHpDamaged = false;
    explore033StatusEnemy.SetUnitGearIcon(explore033StatusEnemy.current.unit);
  }

  public override void Healed(int heal)
  {
  }

  public void SetWeakPoint(WeakPoint weakPoint)
  {
    int num1 = 0;
    foreach (CommonElement element in weakPoint.Element)
    {
      this.createWeakPointIcon(this.go_commonElementIcon, this.mWeakPointIconGridAnchor).GetComponent<CommonElementIcon>().Init(element);
      ++num1;
    }
    foreach (GearKindEnum ID in weakPoint.Gearkind)
    {
      this.createWeakPointIcon(this.go_weaponTypeIcon, this.mWeakPointIconGridAnchor).GetComponent<GearKindIcon>().Init((int) ID);
      ++num1;
    }
    int num2 = 0;
    foreach (UnitTypeEnum unitTypeEnum in weakPoint.PrincessType)
    {
      this.mWeakPointLblPrefab.Clone(((Component) this.mWeakPointIconListAnchor).transform).GetComponentInChildren<UILabel>().SetTextLocalize(this.getPrincessTypeString(unitTypeEnum));
      ++num2;
      ++num1;
    }
    UIWidget component = ((Component) this.mWeakPointIconListAnchor).GetComponent<UIWidget>();
    component.SetDimensions(component.width, num2 * (int) this.mWeakPointIconListAnchor.cellHeight);
    if (num1 < 1)
    {
      this.mWeakPointRoot.SetActive(false);
    }
    else
    {
      this.mWeakPointRoot.SetActive(true);
      this.mWeakPointIconGridAnchor.Reposition();
      this.mWeakPointIconListAnchor.Reposition();
    }
  }

  private string getPrincessTypeString(UnitTypeEnum unitTypeEnum)
  {
    switch (unitTypeEnum)
    {
      case UnitTypeEnum.ouki:
        return "王姫";
      case UnitTypeEnum.meiki:
        return "命姫";
      case UnitTypeEnum.kouki:
        return "攻姫";
      case UnitTypeEnum.maki:
        return "魔姫";
      case UnitTypeEnum.syuki:
        return "守姫";
      case UnitTypeEnum.syouki:
        return "匠姫";
      default:
        Debug.LogError((object) "タイプ不一致");
        return "";
    }
  }

  protected GameObject createWeakPointIcon(GameObject prefab, UIGrid anchor)
  {
    GameObject weakPointIcon = prefab.Clone(((Component) anchor).transform);
    UI2DSprite componentInChildren = weakPointIcon.GetComponentInChildren<UI2DSprite>();
    ((UIWidget) componentInChildren).width = 46;
    ((UIWidget) componentInChildren).height = 42;
    ((UIWidget) componentInChildren).depth = 100;
    return weakPointIcon;
  }
}

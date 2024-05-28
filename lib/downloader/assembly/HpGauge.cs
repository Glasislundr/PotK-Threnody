// Decompiled with JetBrains decompiler
// Type: HpGauge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HpGauge : BattleMonoBehaviour
{
  [SerializeField]
  protected GameObject gauge;
  [SerializeField]
  protected GameObject dirGearIcon;
  private float gaugeValue;
  private BL.BattleModified<BL.Unit> modified;
  private BL.BattleModified<BL.StructValue<bool>> isViewUnitTypeModified;
  [SerializeField]
  protected HpGauge.ElementIcons[] gears;
  [SerializeField]
  protected GameObject bossGauge;
  [SerializeField]
  protected GameObject[] bossParts;
  private bool mIsEffectMode;
  private const int animeFrame = 30;

  public void enableBossMode()
  {
    if (this.bossParts == null || this.bossParts.Length < 1)
      return;
    ((Component) this.gauge.transform.parent).gameObject.SetActive(false);
    this.gauge = this.bossGauge;
    foreach (GameObject bossPart in this.bossParts)
      bossPart.SetActive(true);
  }

  public bool isEffectMode
  {
    get => this.mIsEffectMode;
    set => this.mIsEffectMode = value;
  }

  protected override IEnumerator Start_Battle()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    HpGauge hpGauge = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    hpGauge.isViewUnitTypeModified = BL.Observe<BL.StructValue<bool>>(hpGauge.env.core.isViewUnitType);
    return false;
  }

  public void setUnit(BL.Unit unit)
  {
    this.modified = BL.Observe<BL.Unit>(unit);
    this.SetGearKind(unit);
  }

  private void SetGearKind(BL.Unit unit)
  {
    ((IEnumerable<HpGauge.ElementIcons>) this.gears).ForEach<HpGauge.ElementIcons>((Action<HpGauge.ElementIcons>) (x =>
    {
      if (!Object.op_Inequality((Object) x.parent, (Object) null))
        return;
      x.parent.SetActive(false);
    }));
    int kindGearKind = unit.unit.kind_GearKind;
    int index = !unit.isFacility || kindGearKind != 9999 ? unit.unit.kind_GearKind - 1 : 7;
    if (this.gears.Length <= index || !Object.op_Inequality((Object) this.gears[index].parent, (Object) null))
      return;
    HpGauge.ElementIcons gear = this.gears[index];
    gear.parent.SetActive(true);
    ((IEnumerable<GameObject>) gear.icons).ForEach<GameObject>((Action<GameObject>) (x =>
    {
      if (!Object.op_Inequality((Object) x, (Object) null))
        return;
      x.SetActive(false);
    }));
    int element = (int) unit.GetElement();
    if (!Object.op_Inequality((Object) gear.icons[element - 1], (Object) null))
      return;
    gear.icons[element - 1].SetActive(true);
  }

  protected override void LateUpdate_Battle()
  {
    if (!this.mIsEffectMode && this.modified != null && this.modified.isChangedOnce())
      this.setValue(this.modified.value.hp, this.modified.value.parameter.Hp);
    if (!this.isViewUnitTypeModified.isChangedOnce())
      return;
    this.DispUnitTypeIcon(this.isViewUnitTypeModified.value.value);
  }

  private void DispUnitTypeIcon(bool canDisp) => this.dirGearIcon.SetActive(canDisp);

  public void setGauge(int start, int end)
  {
    this.setValue(start, this.modified.value.parameter.Hp);
    this.StartCoroutine(this.gaugeAnim(start, end));
  }

  public void setGauge(int value) => this.setValue(value, this.modified.value.parameter.Hp);

  private IEnumerator gaugeAnim(int start, int end)
  {
    int count = 0;
    while (++count < 30)
    {
      yield return (object) null;
      this.setValue(start + (end - start) * count / 30, this.modified.value.parameter.Hp);
    }
    this.setValue(end, this.modified.value.parameter.Hp);
  }

  public bool setValue(int n, int max)
  {
    if (Mathf.Approximately((float) max, 0.0f))
      return false;
    float num = (float) n / (float) max;
    if (Mathf.Approximately(this.gaugeValue, num))
      return false;
    this.gauge.transform.localScale = new Vector3(num * 2.54f, this.gauge.transform.localScale.y, this.gauge.transform.localScale.z);
    this.gaugeValue = num;
    return true;
  }

  [Serializable]
  public struct ElementIcons
  {
    public GameObject parent;
    public GameObject[] icons;
  }
}

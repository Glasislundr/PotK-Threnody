// Decompiled with JetBrains decompiler
// Type: UnitIconBottomBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UnitIconBottomBase : MonoBehaviour
{
  public GameObject[] GearRarity;
  public UI2DSprite rarityStar;
  public GameObject textParent;
  public UILabel[] textLevels;
  public GameObject gearIcon;
  public UI2DSprite spriteFrameNormal;
  public UI2DSprite spriteFrameAwake;
  private const int USUAL = 0;
  private const int FRIENDLY = 1;

  public void Set(PlayerUnit playerUnit)
  {
    this.SetText(playerUnit.total_level.ToLocalizeNumberText());
    UI2DSprite[] ui2DspriteArray = new UI2DSprite[2]
    {
      this.spriteFrameNormal,
      this.spriteFrameAwake
    };
    bool[] flagArray = new bool[2]
    {
      !playerUnit.unit.awake_unit_flag,
      playerUnit.unit.awake_unit_flag
    };
    for (int index = 0; index < ui2DspriteArray.Length; ++index)
    {
      UI2DSprite ui2Dsprite = ui2DspriteArray[index];
      if (Object.op_Inequality((Object) ui2Dsprite, (Object) null))
        ((Behaviour) ui2Dsprite).enabled = flagArray[index];
    }
    RarityIcon.SetRarity(playerUnit, this.rarityStar, false);
    this.StartCoroutine(this.SetGearSprite(playerUnit.unit.kind_GearKind, playerUnit.GetElement()));
  }

  public void SetText(string level)
  {
    this.textParent.SetActive(true);
    ((Component) this.textLevels[0]).gameObject.SetActive(true);
    this.textLevels[0].SetTextLocalize(level);
  }

  public void Set(int rarity, int kind, CommonElement element)
  {
    ((IEnumerable<GameObject>) this.GearRarity).ToggleOnce(rarity - 1);
    this.StartCoroutine(this.SetGearSprite(kind, element));
  }

  public void Set(UnitUnit unit)
  {
    this.StartCoroutine(this.SetGearSprite(unit.kind_GearKind, unit.GetElement()));
  }

  private IEnumerator SetGearSprite(int kind, CommonElement element)
  {
    Future<GameObject> SetGearPrefab = Res.Icons.GearKindIcon.Load<GameObject>();
    IEnumerator e = SetGearPrefab.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    SetGearPrefab.Result.Clone(this.gearIcon.transform).GetComponent<GearKindIcon>().Init(kind, element);
  }
}

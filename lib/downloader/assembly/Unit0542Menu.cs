// Decompiled with JetBrains decompiler
// Type: Unit0542Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnitDetails;
using UnityEngine;

#nullable disable
public class Unit0542Menu : Unit0042Menu
{
  protected override IEnumerator LoadPrefabs()
  {
    Unit0542Menu unit0542Menu = this;
    Future<GameObject> loader = (Future<GameObject>) null;
    loader = Res.Prefabs.unit054_2.detail054_2.Load<GameObject>();
    IEnumerator e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.detailPrefab = loader.Result;
    loader = Res.Prefabs.ItemIcon.prefab.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.gearIconPrefab = loader.Result;
    loader = Res.Icons.GearKindIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.gearKindIconPrefab = loader.Result;
    loader = Res.Prefabs.battle017_11_1_1.SkillDetailDialog.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.skillDetailDialogPrefab = loader.Result;
    loader = Res.Icons.GearProfiencyIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.profIconPrefab = loader.Result;
    loader = Res.Prefabs.BattleSkillIcon._battleSkillIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.skillTypeIconPrefab = loader.Result;
    loader = Res.Icons.CommonElementIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.commonElementIconPrefab = loader.Result;
    loader = Res.Icons.SPAtkTypeIcon.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.spAtkTypeIconPrefab = loader.Result;
    loader = Res.Prefabs.gacha006_8.slc_3DModel.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.modelPrefab = loader.Result;
    loader = Res.Prefabs.unit.dir_unit_status_detail.Load<GameObject>();
    e = loader.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unit0542Menu.statusDetailPrefab = loader.Result;
  }

  public override Control controlFlags => Control.Zero;
}

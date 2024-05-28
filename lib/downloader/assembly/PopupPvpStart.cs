// Decompiled with JetBrains decompiler
// Type: PopupPvpStart
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PopupPvpStart : MonoBehaviour
{
  [SerializeField]
  private PopupPvpStart.PopupPvpStartObject playerObjData;
  [SerializeField]
  private PopupPvpStart.PopupPvpStartObject enemyObjData;

  public IEnumerator Initialize(
    string pName,
    string eName,
    int pEmblem,
    int eEmblem,
    List<BL.Unit> pUnits,
    List<BL.Unit> eUnits,
    string pGuild = null,
    string eGuild = null)
  {
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitPrefab = unitPrefabF.Result;
    e = this.playerObjData.SetData(pName, pEmblem, pGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemyObjData.SetData(eName, eEmblem, eGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.playerObjData.CreateUnitsThumPlayer(pUnits, unitPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemyObjData.CreateUnitsThumEnemy(eUnits, unitPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0536");
  }

  public IEnumerator Initialize(
    string pName,
    string eName,
    int pEmblem,
    int eEmblem,
    int[] pUnits,
    int[] eUnits,
    string pGuild = null,
    string eGuild = null)
  {
    Future<GameObject> unitPrefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
    IEnumerator e = unitPrefabF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    GameObject unitPrefab = unitPrefabF.Result;
    e = this.playerObjData.SetData(pName, pEmblem, pGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemyObjData.SetData(eName, eEmblem, eGuild);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.playerObjData.CreateUnitsThumPlayerDebug(pUnits, unitPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.enemyObjData.CreateUnitsThumEnemyDebug(eUnits, unitPrefab);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Singleton<NGSoundManager>.GetInstance().playSE("SE_0536");
  }

  [Serializable]
  private class PopupPvpStartObject
  {
    [SerializeField]
    private UILabel name;
    [SerializeField]
    private UI2DSprite emblem;
    [SerializeField]
    private UILabel guild;
    [SerializeField]
    private Transform[] units;

    public IEnumerator SetData(string name, int emblem, string guild)
    {
      this.name.text = name;
      if (Object.op_Inequality((Object) this.guild, (Object) null) && guild != null)
        this.guild.text = guild;
      Future<Sprite> spriteF = EmblemUtility.LoadEmblemSprite(emblem);
      IEnumerator e = spriteF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.emblem.sprite2D = spriteF.Result;
    }

    public IEnumerator CreateUnitsThumPlayer(List<BL.Unit> units, GameObject prefab)
    {
      for (int i = 0; i < units.Count && i < this.units.Length; ++i)
      {
        IEnumerator e = this.CreateUnitThum(units[i].unit.ID, units[i].lv, prefab, this.units[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    public IEnumerator CreateUnitsThumEnemy(List<BL.Unit> units, GameObject prefab)
    {
      List<BL.Unit> unitList = new List<BL.Unit>();
      for (int index = units.Count - 1; index >= 0; --index)
        unitList.Add(units[index]);
      units = unitList;
      for (int i = 0; i < units.Count && i < this.units.Length; ++i)
      {
        IEnumerator e = this.CreateUnitThum(units[i].unit.ID, units[i].lv, prefab, this.units[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    public IEnumerator CreateUnitsThumPlayerDebug(int[] units, GameObject prefab)
    {
      for (int i = 0; i < units.Length; ++i)
      {
        IEnumerator e = this.CreateUnitThum(units[i], 22, prefab, this.units[i]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    public IEnumerator CreateUnitsThumEnemyDebug(int[] units, GameObject prefab)
    {
      for (int i = 0; i < units.Length; ++i)
      {
        IEnumerator e = this.CreateUnitThum(units[i], 22, prefab, this.units[i + this.units.Length - units.Length]);
        while (e.MoveNext())
          yield return e.Current;
        e = (IEnumerator) null;
      }
    }

    public IEnumerator CreateUnitThum(int id, int lv, GameObject prefab, Transform parent)
    {
      UnitIcon up = prefab.Clone(parent).GetComponent<UnitIcon>();
      IEnumerator e = up.SetUnit(MasterData.UnitUnit[id], MasterData.UnitUnit[id].GetElement(), false);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      up.setLevelText(lv.ToString());
      up.ShowBottomInfosLevelOnly();
    }
  }
}

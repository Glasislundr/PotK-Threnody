// Decompiled with JetBrains decompiler
// Type: Battle020221Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Battle020221Menu : NGMenuBase
{
  [SerializeField]
  protected UILabel TxtCharaname22;
  [SerializeField]
  protected UILabel TxtCharaname222;
  [SerializeField]
  protected UILabel TxtDescription24;
  [SerializeField]
  protected UILabel TxtLvAfter26;
  [SerializeField]
  protected UILabel TxtLvbefore26;
  [SerializeField]
  protected UILabel TxtPopuptitle26;
  private GameObject UnitPrefab;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  public GameObject chara1;
  public GameObject chara2;
  private Action onCallback;

  public void Init(int id1, int id2, string name1, string name2, int beforeLv, int afterLv)
  {
    this.TxtLvbefore26.SetTextLocalize(beforeLv);
    this.TxtLvAfter26.SetTextLocalize(afterLv);
    this.TxtCharaname22.SetTextLocalize(name1);
    this.TxtCharaname222.SetTextLocalize(name2);
    this.StartCoroutine(this.SetCharaIcon(id1, id2));
  }

  private IEnumerator LoadUnitPrefab()
  {
    if (!Object.op_Implicit((Object) this.UnitPrefab))
    {
      Future<GameObject> prefabF = Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.UnitPrefab = prefabF.Result;
    }
  }

  public IEnumerator SetUnitPrefab(GameObject setObject, UnitUnit unit)
  {
    GameObject gameObject = this.UnitPrefab.Clone(setObject.transform);
    gameObject.transform.localScale = new Vector3(1f, 1f);
    UnitIcon unitScript = gameObject.GetComponent<UnitIcon>();
    IEnumerator e = unitScript.SetUnit(unit, unit.GetElement(), false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    unitScript.BottomModeValue = UnitIconBase.BottomMode.Nothing;
  }

  private IEnumerator SetCharaIcon(int id1, int id2)
  {
    IEnumerator e = this.LoadUnitPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitPrefab(this.chara1, MasterData.UnitCharacter[id1].GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitPrefab(this.chara2, MasterData.UnitCharacter[id2].GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public virtual void IbtnScreen()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.onCallback == null)
      return;
    this.onCallback();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;
}

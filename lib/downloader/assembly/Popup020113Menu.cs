// Decompiled with JetBrains decompiler
// Type: Popup020113Menu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup020113Menu : NGMenuBase
{
  private Action onCallback;
  private GameObject UnitPrefab;
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  [SerializeField]
  protected UILabel[] UnitName;
  [SerializeField]
  protected GameObject[] UnitIconObject;
  private const float scale = 1f;

  public virtual IEnumerator Init(QuestHarmonyS quest)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Popup020113Menu popup020113Menu = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    popup020113Menu.UnitName[0].SetTextLocalize(quest.unit.name);
    popup020113Menu.UnitName[1].SetTextLocalize(quest.target_unit.name);
    popup020113Menu.StartCoroutine(popup020113Menu.SetCharaIcon(quest.unit.character, quest.target_unit.character));
    return false;
  }

  protected IEnumerator LoadUnitPrefab()
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

  public void PopUpTap()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.onCallback == null)
      return;
    this.onCallback();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;

  protected IEnumerator SetUnitPrefab(GameObject setObject, UnitUnit unit)
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

  public virtual IEnumerator SetCharaIcon(UnitCharacter unit_1, UnitCharacter unit_2)
  {
    IEnumerator e = this.LoadUnitPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitPrefab(this.UnitIconObject[0], unit_1.GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitPrefab(this.UnitIconObject[1], unit_2.GetDefaultUnitUnit());
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}

// Decompiled with JetBrains decompiler
// Type: DetailMenuScrollViewBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System;
using System.Collections;
using UnitDetails;
using UnityEngine;

#nullable disable
public class DetailMenuScrollViewBase : MonoBehaviour
{
  public bool isEarthMode;
  public bool isMemory;

  public Control controlFlags { get; private set; }

  public void setControlFlags(Unit0042Scene menu)
  {
    if (!Object.op_Inequality((Object) menu, (Object) null))
      return;
    this.setControlFlags(menu.bootParam.controlFlags);
  }

  public void setControlFlags(Control flags) => this.controlFlags = flags;

  public virtual bool Init(PlayerUnit playerUnit, PlayerUnit baseUnit)
  {
    ((Component) this).gameObject.SetActive(true);
    return true;
  }

  public virtual IEnumerator setModel(
    PlayerUnit playerUnit,
    GameObject modelPrefab,
    Vector3 modelPos,
    bool light,
    Action action = null)
  {
    yield break;
  }

  public virtual IEnumerator initAsync(
    PlayerUnit playerUnit,
    bool limitMode,
    bool isMaterial,
    GameObject[] prefabs = null)
  {
    yield break;
  }

  protected virtual void setText(UILabel label, int v) => label.SetTextLocalize(v);

  public virtual void EndScene()
  {
  }

  public virtual void MarkAsChanged()
  {
  }

  public virtual IEnumerator initAsyncDiffMode(
    PlayerUnit playerUnit,
    PlayerUnit prevUnit,
    IDetailMenuContainer menuContainer)
  {
    yield break;
  }

  protected void inactivateGameObject<T>(T co) where T : Component
  {
    if (!Object.op_Inequality((Object) (object) co, (Object) null))
      return;
    co.gameObject.SetActive(false);
  }

  protected void inactivateGameObject(GameObject go)
  {
    if (!Object.op_Inequality((Object) go, (Object) null))
      return;
    go.SetActive(false);
  }
}

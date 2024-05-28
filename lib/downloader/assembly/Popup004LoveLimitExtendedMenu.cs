// Decompiled with JetBrains decompiler
// Type: Popup004LoveLimitExtendedMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Popup004LoveLimitExtendedMenu : NGMenuBase
{
  private const float LINK_WIDTH = 136f;
  private const float LINK_DEFWIDTH = 136f;
  private const float scale = 1f;
  [SerializeField]
  private GameObject linkChar;
  [SerializeField]
  protected UILabel TxtCharaname22;
  [SerializeField]
  protected UILabel TxtLoveLimitAfter26;
  [SerializeField]
  protected UILabel TxtLoveLimitbefore26;
  [SerializeField]
  protected UILabel TxtTitle;
  [SerializeField]
  protected UILabel TxtMessage;
  private GameObject UnitPrefab;
  private Action onCallback;

  public void Init(UnitUnit unit, string name, int beforeLoveLimit, int afterLoveLimit)
  {
    this.TxtLoveLimitbefore26.SetTextLocalize(string.Format("{0}%", (object) beforeLoveLimit));
    this.TxtLoveLimitAfter26.SetTextLocalize(string.Format("{0}%", (object) afterLoveLimit));
    this.TxtCharaname22.SetTextLocalize(name);
    if (unit.IsSea)
    {
      this.TxtTitle.SetTextLocalize(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_DEAR);
      this.TxtMessage.SetTextLocalize(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_MESSAGE_DEAR);
    }
    else if (unit.IsResonanceUnit)
    {
      this.TxtTitle.SetTextLocalize(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_TITLE_RELEVANCE);
      this.TxtMessage.SetTextLocalize(Consts.GetInstance().POPUP_004_LIMIT_EXTENDED_MESSAGE_RELEVANCE);
    }
    this.StartCoroutine(this.SetCharaIcon(unit));
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

  private IEnumerator SetCharaIcon(UnitUnit unit)
  {
    IEnumerator e = this.LoadUnitPrefab();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = this.SetUnitPrefab(this.linkChar, unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  private IEnumerator LoadUnitPrefab()
  {
    if (!Object.op_Implicit((Object) this.UnitPrefab))
    {
      Future<GameObject> prefabF = Singleton<NGGameDataManager>.GetInstance().IsSea ? Res.Prefabs.Sea.UnitIcon.normal_sea.Load<GameObject>() : Res.Prefabs.UnitIcon.normal.Load<GameObject>();
      IEnumerator e = prefabF.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.UnitPrefab = prefabF.Result;
    }
  }

  public virtual void PopUpTap()
  {
    Singleton<PopupManager>.GetInstance().dismiss();
    if (this.onCallback == null)
      return;
    this.onCallback();
  }

  public void SetCallback(Action callback) => this.onCallback = callback;
}

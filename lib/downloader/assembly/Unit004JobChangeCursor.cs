// Decompiled with JetBrains decompiler
// Type: Unit004JobChangeCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class Unit004JobChangeCursor : MonoBehaviour
{
  [SerializeField]
  private GameObject objFocus_;
  [SerializeField]
  private GameObject btnCurrent_;
  [SerializeField]
  private GameObject btnNormal_;
  [SerializeField]
  private UILabel txtJobName_;
  [SerializeField]
  private UILabel txtRankName_;
  [SerializeField]
  private GameObject objActive_;
  [SerializeField]
  private GameObject objChain_;
  [SerializeField]
  private GameObject objUnopened_;
  [SerializeField]
  public bool isShort;

  public void Initialize(
    MasterDataTable.UnitJob uj,
    bool isCurrent,
    Sprite sprFrame,
    EventDelegate.Callback btnCallback)
  {
    this.txtJobName_.SetTextLocalize(uj.name);
    UnitJobRankName unitJobRankName;
    this.txtRankName_.SetTextLocalize(MasterData.UnitJobRankName.TryGetValue(uj.job_rank_UnitJobRank, out unitJobRankName) ? unitJobRankName.name : string.Empty);
    this.SetFocus(isCurrent);
    this.btnCurrent_.gameObject.SetActive(isCurrent);
    this.btnNormal_.gameObject.SetActive(!isCurrent);
    if (isCurrent)
    {
      if (Object.op_Inequality((Object) sprFrame, (Object) null))
        this.btnCurrent_.GetComponent<UI2DSprite>().sprite2D = sprFrame;
      EventDelegate.Set(this.btnCurrent_.GetComponent<UIButton>().onClick, btnCallback);
      this.btnNormal_.GetComponent<UIButton>().onClick.Clear();
    }
    else
    {
      if (Object.op_Inequality((Object) sprFrame, (Object) null))
        this.btnNormal_.GetComponent<UI2DSprite>().sprite2D = sprFrame;
      this.btnCurrent_.GetComponent<UIButton>().onClick.Clear();
      EventDelegate.Set(this.btnNormal_.GetComponent<UIButton>().onClick, btnCallback);
    }
  }

  public void SetDecoration(bool unlock, bool activeChange, bool conditionsLocked)
  {
    if (Object.op_Inequality((Object) this.objChain_, (Object) null))
      this.objChain_.SetActive(!unlock | conditionsLocked);
    if (Object.op_Inequality((Object) this.objActive_, (Object) null))
      this.objActive_.SetActive(activeChange && !conditionsLocked);
    if (!Object.op_Inequality((Object) this.objUnopened_, (Object) null))
      return;
    this.objUnopened_.SetActive(((activeChange ? 0 : (!unlock ? 1 : 0)) | (conditionsLocked ? 1 : 0)) != 0);
  }

  public void SetFocus(bool isFocus)
  {
    if (!Object.op_Inequality((Object) this.objFocus_, (Object) null))
      return;
    this.objFocus_.SetActive(isFocus);
  }
}

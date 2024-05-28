// Decompiled with JetBrains decompiler
// Type: Bugu0053GearPopup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Bugu0053GearPopup : ItemDetailPopupBase
{
  private Bugu0053DirRecipePopup root;
  [SerializeField]
  private UIButton[] IbtnClose;

  public virtual IEnumerator Init(
    Bugu0053DirRecipePopup recipePopup,
    MasterDataTable.CommonRewardType type,
    int id)
  {
    Bugu0053GearPopup bugu0053GearPopup = this;
    bugu0053GearPopup.root = recipePopup;
    bugu0053GearPopup.root.isBackKey = false;
    ((UIRect) ((Component) bugu0053GearPopup).GetComponent<UIWidget>()).alpha = 0.0f;
    IEnumerator e = bugu0053GearPopup.SetInfo(type, id);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((UIRect) ((Component) bugu0053GearPopup).GetComponent<UIWidget>()).alpha = 1f;
    ((Behaviour) ((Component) bugu0053GearPopup).GetComponent<Shop00742Menu>()).enabled = false;
    ((Behaviour) ((Component) bugu0053GearPopup).GetComponent<ItemDetailPopupBase>()).enabled = false;
    for (int index = 0; index < bugu0053GearPopup.IbtnClose.Length; ++index)
    {
      // ISSUE: reference to a compiler-generated method
      EventDelegate.Set(bugu0053GearPopup.IbtnClose[index].onClick, new EventDelegate.Callback(bugu0053GearPopup.\u003CInit\u003Eb__2_0));
    }
  }

  public override void IbtnNo()
  {
    if (Object.op_Inequality((Object) this.root, (Object) null))
      Singleton<CommonRoot>.GetInstance().StartCoroutine(this.root.BackKeyEnable());
    Singleton<PopupManager>.GetInstance().onDismiss();
  }

  public override void onBackButton() => this.IbtnNo();
}

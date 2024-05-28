// Decompiled with JetBrains decompiler
// Type: PopupHotDealSelectItemIcon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class PopupHotDealSelectItemIcon : MonoBehaviour
{
  [SerializeField]
  private CreateIconObject Icon;
  [SerializeField]
  private UILabel QuantityLbl;

  public IEnumerator Initialize(MasterDataTable.CommonRewardType type, int typeId, int quantity)
  {
    IEnumerator e = this.Icon.CreateThumbnail(type, typeId, isButton: false);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.QuantityLbl.SetTextLocalize(string.Format("x{0}", (object) quantity));
  }
}

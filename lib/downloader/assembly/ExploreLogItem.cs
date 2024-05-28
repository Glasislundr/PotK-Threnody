// Decompiled with JetBrains decompiler
// Type: ExploreLogItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ExploreLogItem : MonoBehaviour
{
  [SerializeField]
  private UILabel mMessageLbl;

  public void SetMessage(string message, Color color)
  {
    this.mMessageLbl.SetTextLocalize(message);
    ((UIWidget) this.mMessageLbl).color = color;
  }
}

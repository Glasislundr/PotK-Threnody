// Decompiled with JetBrains decompiler
// Type: EmblemSortAndFilterTabButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EmblemSortAndFilterTabButton : EmblemSortAndFilterButton
{
  [SerializeField]
  private UILabel TextSelect;

  public void SetText(string title) => this.TextSelect.SetTextLocalize(title);

  public void SetTextColor(Color col) => ((UIWidget) this.TextSelect).color = col;
}

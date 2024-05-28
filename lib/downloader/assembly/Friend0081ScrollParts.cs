// Decompiled with JetBrains decompiler
// Type: Friend0081ScrollParts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Friend0081ScrollParts : FriendScrollBar
{
  [SerializeField]
  private UIButton button;

  public void Set(int index, Friend0081Menu menu)
  {
    EventDelegate.Set(this.button.onClick, new EventDelegate.Callback(((FriendScrollBar) this).onDetails));
    this.index = index;
    this.menu = menu;
  }
}

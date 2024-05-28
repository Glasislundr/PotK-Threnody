// Decompiled with JetBrains decompiler
// Type: GuildBaseItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class GuildBaseItem
{
  [SerializeField]
  private GameObject enemyObject;
  [SerializeField]
  private GameObject friendObject;
  [SerializeField]
  private GameObject parentObject;
  [SerializeField]
  private UILabel nameLabel;

  public void Initialize(bool isEnemy, string name)
  {
    this.enemyObject.SetActive(isEnemy);
    this.friendObject.SetActive(!isEnemy);
    this.nameLabel.SetTextLocalize(name);
  }

  public void SetActive(bool flag) => this.parentObject.SetActive(flag);
}

// Decompiled with JetBrains decompiler
// Type: BackButtonPopupBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public abstract class BackButtonPopupBase : BackButtonMenuBase
{
  [SerializeField]
  [Tooltip("先頭位置を示す(false=\"Update()\"を実行しない)")]
  protected bool isTopObject = true;

  public void setTopObject(GameObject obj) => this.topObject = obj;

  public GameObject topObject { get; private set; }

  protected override void Update()
  {
  }
}

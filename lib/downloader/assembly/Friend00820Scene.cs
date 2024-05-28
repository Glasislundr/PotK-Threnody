// Decompiled with JetBrains decompiler
// Type: Friend00820Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Friend00820Scene : NGSceneBase
{
  [SerializeField]
  private Friend00820Menu menu;
  [SerializeField]
  private GameObject smsButton;
  [SerializeField]
  private GameObject lineButton;

  public IEnumerator onStartSceneAsync()
  {
    this.smsButton.SetActive(false);
    this.lineButton.SetActive(false);
    this.menu.ScrollContainerResolvePosition();
    yield break;
  }
}

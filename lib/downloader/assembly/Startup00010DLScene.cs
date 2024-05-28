// Decompiled with JetBrains decompiler
// Type: Startup00010DLScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Startup00010DLScene : MonoBehaviour
{
  [SerializeField]
  private Startup00010DLMenu menu;

  private void Awake() => ModalWindow.setupRootPanel(((Component) this).GetComponent<UIRoot>());

  public void Start() => this.menu.onStartSceneAsync();
}

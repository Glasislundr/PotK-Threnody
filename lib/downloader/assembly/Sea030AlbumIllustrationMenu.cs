// Decompiled with JetBrains decompiler
// Type: Sea030AlbumIllustrationMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030AlbumIllustrationMenu : BackButtonMenuBase
{
  [SerializeField]
  private IllustController controller;
  [SerializeField]
  private UIButton ibtnBack;
  [SerializeField]
  private UI2DSprite imageSprite;

  public IEnumerator Init(float panel_width, int album_id)
  {
    Sea030AlbumIllustrationMenu illustrationMenu = this;
    ((Component) illustrationMenu.ibtnBack).gameObject.SetActive(false);
    illustrationMenu.controller.Init(new Action(illustrationMenu.ShowButton), panel_width);
    Future<Sprite> imageF = Singleton<ResourceManager>.GetInstance().Load<Sprite>(string.Format("Album/{0}/slc_album_l", (object) album_id));
    IEnumerator e = imageF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    illustrationMenu.imageSprite.sprite2D = imageF.Result;
    yield return (object) null;
  }

  public override void onBackButton() => this.backScene();

  protected void ShowButton() => ((Component) this.ibtnBack).gameObject.SetActive(true);
}

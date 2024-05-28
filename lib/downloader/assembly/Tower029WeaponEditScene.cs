// Decompiled with JetBrains decompiler
// Type: Tower029WeaponEditScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class Tower029WeaponEditScene : NGSceneBase
{
  [SerializeField]
  private Tower029WeaponEditMenu menu;

  public static void ChangeScene()
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("tower029_weapon_edit", true);
  }

  public IEnumerator onStartSceneAsync()
  {
    Tower029WeaponEditScene tower029WeaponEditScene = this;
    IEnumerator e = tower029WeaponEditScene.menu.InitializeAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    tower029WeaponEditScene.bgmFile = TowerUtil.BgmFile;
    tower029WeaponEditScene.bgmName = TowerUtil.BgmName;
  }

  public void onStartScene() => Singleton<CommonRoot>.GetInstance().isLoading = false;
}

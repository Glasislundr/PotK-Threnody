// Decompiled with JetBrains decompiler
// Type: EditCustomDeckScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CustomDeck/Scene")]
public class EditCustomDeckScene : NGSceneBase
{
  private static string defSceneName = "quest002_CustomFormation";
  private EditCustomDeckMenu menu_;

  public static void changeScene(bool bStack, int deckNumber = -1, int[] usedIds = null, bool bSetFromScene = false)
  {
    string str = !bStack | bSetFromScene ? Singleton<NGSceneManager>.GetInstance().sceneName : string.Empty;
    Singleton<NGSceneManager>.GetInstance().changeScene(EditCustomDeckScene.defSceneName, (bStack ? 1 : 0) != 0, (object) str, (object) deckNumber, (object) usedIds);
  }

  public static void changeScene(ColosseumUtility.Info colosseumInfo)
  {
    string sceneName = Singleton<NGSceneManager>.GetInstance().sceneName;
    Singleton<NGSceneManager>.GetInstance().changeScene(EditCustomDeckScene.defSceneName, false, (object) sceneName, (object) colosseumInfo);
  }

  public override IEnumerator onInitSceneAsync()
  {
    EditCustomDeckScene editCustomDeckScene = this;
    editCustomDeckScene.menu_ = editCustomDeckScene.menuBase as EditCustomDeckMenu;
    IEnumerator e = editCustomDeckScene.menu_.doLoadResources();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    Future<GameObject> ld = new ResourceObject("Prefabs/BackGround/CustomFormation_Background").Load<GameObject>();
    e = ld.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    editCustomDeckScene.backgroundPrefab = ld.Result;
  }

  public IEnumerator onStartSceneAsync(string fromScene, int deckNumber, int[] usedIds)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Singleton<NGGameDataManager>.GetInstance().isEditCustomDeck = true;
    this.menu_.fromScene = fromScene;
    string[] strArray = Singleton<NGSceneManager>.GetInstance().getSceneStackPath().Split('.');
    this.menu_.sortie = EditCustomDeckScene.Sortie.None;
    foreach (string str in strArray)
    {
      switch (str)
      {
        case "quest002_8":
          this.menu_.sortie = EditCustomDeckScene.Sortie.Normal;
          break;
        case "versus026_2":
        case "versus026_10":
          this.menu_.sortie = EditCustomDeckScene.Sortie.Versus;
          break;
        case "raid032_battle":
          this.menu_.sortie = EditCustomDeckScene.Sortie.GuildRaid;
          break;
      }
      if (this.menu_.sortie != EditCustomDeckScene.Sortie.None)
        break;
    }
    this.menu_.usedIds = usedIds;
    IEnumerator e = this.menu_.onStartSceneAsync(false, deckNumber);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(string fromScene, int deckNumber, int[] usedIds)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onBackSceneAsync(string fromScene, int deckNumber, int[] usedIds)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = this.menu_.onStartSceneAsync(true, deckNumber);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onBackScene(string fromScene, int deckNumber, int[] usedIds)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onStartSceneAsync(string fromScene, ColosseumUtility.Info colosseumInfo)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Singleton<CommonRoot>.GetInstance().SetFooterEnable(false);
    Singleton<NGGameDataManager>.GetInstance().isEditCustomDeck = true;
    this.menu_.fromScene = fromScene;
    this.menu_.sortie = EditCustomDeckScene.Sortie.Colosseum;
    this.menu_.colosseumInfo = colosseumInfo;
    IEnumerator e = this.menu_.onStartSceneAsync(false, -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onStartScene(string fromScene, ColosseumUtility.Info colosseumInfo)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public IEnumerator onBackSceneAsync(string fromScene, ColosseumUtility.Info colosseumInfo)
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    IEnumerator e = this.menu_.onStartSceneAsync(true, -1);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public void onBackScene(string fromScene, ColosseumUtility.Info colosseumInfo)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public enum Sortie
  {
    None,
    Normal,
    Versus,
    Colosseum,
    GuildRaid,
  }
}

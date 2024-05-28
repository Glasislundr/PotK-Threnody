// Decompiled with JetBrains decompiler
// Type: CorpsQuestManualScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
[AddComponentMenu("Scenes/CorpsQuest/ManualScene")]
public class CorpsQuestManualScene : NGSceneBase
{
  private CorpsQuestManualMenu menu;

  public static void ChangeScene(CorpsSetting setting)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("CorpsQuest_manual", true, (object) setting);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CorpsQuestManualScene questManualScene = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    questManualScene.menu = questManualScene.menuBase as CorpsQuestManualMenu;
    return false;
  }

  public IEnumerator onStartSceneAsync(CorpsSetting setting)
  {
    CorpsQuestManualScene questManualScene = this;
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    yield return (object) null;
    IEnumerator e = MasterData.LoadCorpsHowto(setting);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = questManualScene.menu.InitializeAsync(setting.ID);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    if (!string.IsNullOrEmpty(setting.bgm_file))
    {
      questManualScene.bgmFile = setting.bgm_file;
      questManualScene.bgmName = setting.bgm_name;
    }
    MasterDataCache.Unload("CorpsHowto");
  }

  public void onStartScene(CorpsSetting setting)
  {
    Singleton<CommonRoot>.GetInstance().HideLoadingLayer();
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().ShowLoadingLayer(0);
    Singleton<CommonRoot>.GetInstance().headerType = CommonRoot.HeaderType.Normal;
  }
}

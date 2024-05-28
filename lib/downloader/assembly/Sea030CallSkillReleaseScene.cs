// Decompiled with JetBrains decompiler
// Type: Sea030CallSkillReleaseScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Sea030CallSkillReleaseScene : NGSceneBase
{
  private static string DefName = "sea030_CallSkillRelease";
  [SerializeField]
  private Sea030CallSkillReleaseMenu menu;
  [SerializeField]
  private GameObject blackBg;
  private int orgDeapth;
  private bool advView = true;

  public static void changeScene(bool bStack, int sameCharacterId)
  {
    NGSceneManager instance = Singleton<NGSceneManager>.GetInstance();
    Unit0043Scene sceneBase = instance.sceneBase as Unit0043Scene;
    if (Object.op_Implicit((Object) sceneBase))
      instance.changeScene(Sea030CallSkillReleaseScene.DefName, (bStack ? 1 : 0) != 0, (object) sameCharacterId, (object) sceneBase.BgmFile, (object) sceneBase.BgmName);
    else
      instance.changeScene(Sea030CallSkillReleaseScene.DefName, (bStack ? 1 : 0) != 0, (object) sameCharacterId);
  }

  public override IEnumerator onInitSceneAsync()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Sea030CallSkillReleaseScene skillReleaseScene = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    skillReleaseScene.bgmFile = Singleton<NGSoundManager>.GetInstance().GetCueName();
    skillReleaseScene.bgmName = Singleton<NGSoundManager>.GetInstance().GetBgmName();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator onStartSceneAsync(int sameCharacterId, string bgm_file, string bgm_name)
  {
    Sea030CallSkillReleaseScene skillReleaseScene = this;
    skillReleaseScene.bgmFile = bgm_file;
    skillReleaseScene.bgmName = bgm_name;
    IEnumerator e = skillReleaseScene.onStartSceneAsync(sameCharacterId);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(int sameCharacterId)
  {
    CallCharacter master = (CallCharacter) null;
    foreach (CallCharacter callCharacter in MasterData.CallCharacterList)
    {
      if (callCharacter.same_character_id == sameCharacterId)
        master = callCharacter;
    }
    if (this.advView)
    {
      Singleton<CommonRoot>.GetInstance().loadingMode = 2;
      this.advView = false;
      Story0093Scene.changeScene009_3(true, master.call_script_id, true);
    }
    else
    {
      yield return (object) this.menu.LoadPrefab();
      this.blackBg.SetActive(false);
      Singleton<CommonRoot>.GetInstance().isActiveBackground = false;
      this.menu.CreatePopup(master);
    }
  }

  public void onStartScene(int sameCharacterId, string bgm_file, string bgm_name)
  {
  }

  public void onStartScene(int sameCharacterId)
  {
  }

  public override void onEndScene()
  {
    Singleton<CommonRoot>.GetInstance().isActiveBackground = true;
  }
}

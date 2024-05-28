// Decompiled with JetBrains decompiler
// Type: Quest0025Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using SM;
using System.Collections;
using UnityEngine;

#nullable disable
public class Quest0025Scene : NGSceneBase
{
  public Quest0025Menu menu;

  public static void changeScene0025(bool stack, Quest0025Scene.Quest0025Param param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("quest002_5", (stack ? 1 : 0) != 0, (object) param);
  }

  public IEnumerator onStartSceneAsync(Quest0025Scene.Quest0025Param param)
  {
    Quest0025Scene quest0025Scene = this;
    quest0025Scene.tweens = (UITweener[]) null;
    quest0025Scene.IsPush = false;
    QuestBG backgroundComponent = Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>();
    if (Object.op_Inequality((Object) backgroundComponent, (Object) null))
      backgroundComponent.currentPos = QuestBG.QuestPosition.Chapter;
    BGChange bgchange = ((Component) quest0025Scene).GetComponent<BGChange>();
    bool resetBG = false;
    IEnumerator e;
    if (Object.op_Equality((Object) backgroundComponent, (Object) null))
    {
      resetBG = true;
      e = bgchange.BGprefabCreate(true, param.L);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    if (param.createBG | resetBG)
    {
      e = ((Component) quest0025Scene).GetComponent<BGChange>().SetChapterBg((Quest00240723Menu.StoryMode) param.XL);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
    }
    PlayerStoryQuestS[] StoryData = SMManager.Get<PlayerStoryQuestS[]>();
    bgchange.CrossToXL();
    Singleton<CommonRoot>.GetInstance().getBackgroundComponent<QuestBG>().Toggle(QuestBG.AnimApply.LMainPostion);
    e = quest0025Scene.menu.Init(StoryData, param.XL, param.L, param.Hard, param.reStart);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public class Quest0025Param
  {
    public int XL;
    public int L;
    public bool Hard;
    public bool reStart;
    public bool createBG;

    public Quest0025Param(int xl, int l, bool hard, bool restart = false, bool createBG = true)
    {
      this.XL = xl;
      this.L = l;
      this.Hard = hard;
      this.reStart = restart;
      this.createBG = createBG;
    }
  }
}

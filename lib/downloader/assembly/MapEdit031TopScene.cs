// Decompiled with JetBrains decompiler
// Type: MapEdit031TopScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MapEdit;
using System.Collections;
using UnityEngine;

#nullable disable
public class MapEdit031TopScene : NGSceneBase
{
  private static string defaultName = "map_edit031_top";
  [SerializeField]
  private MapEdit031TopMenu editMenu_;

  public static void changeScene(bool isStack, int slotId, bool isEdit)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(MapEdit031TopScene.defaultName, (isStack ? 1 : 0) != 0, (object) slotId, (object) isEdit);
  }

  public IEnumerator onStartSceneAsync(int slotId, bool isEdit)
  {
    MapEdit031TopScene mapEdit031TopScene = this;
    mapEdit031TopScene.menuBases = ((Component) mapEdit031TopScene).GetComponents<NGMenuBase>();
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    mapEdit031TopScene.menuBase = (NGMenuBase) mapEdit031TopScene.editMenu_;
    MapEditData editData = new MapEditData(slotId);
    if (editData.isError_)
    {
      Singleton<NGSceneManager>.GetInstance().backScene();
    }
    else
    {
      IEnumerator e = mapEdit031TopScene.editMenu_.initialize(editData, isEdit);
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      while (!mapEdit031TopScene.editMenu_.isInitailzed_)
        yield return (object) null;
    }
  }

  public void onStartScene(int slotId, bool isEdit)
  {
    Singleton<CommonRoot>.GetInstance().isLoading = false;
  }

  public override IEnumerator onEndSceneAsync()
  {
    Singleton<CommonRoot>.GetInstance().isLoading = true;
    IEnumerator e = this.editMenu_.coEndSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    e = base.onEndSceneAsync();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }
}

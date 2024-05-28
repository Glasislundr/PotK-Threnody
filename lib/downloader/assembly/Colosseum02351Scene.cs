// Decompiled with JetBrains decompiler
// Type: Colosseum02351Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum02351Scene : NGSceneBase
{
  [SerializeField]
  private Colosseum02351Menu menu;

  public static void ChangeScene(
    Colosseum0235Scene.Param param,
    ColosseumRank rank,
    int fromPoint,
    int nextPoint)
  {
    Colosseum02351Scene.Data data = new Colosseum02351Scene.Data(rank, fromPoint, nextPoint);
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_5_1", false, (object) param, (object) data);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Colosseum02351Scene colosseum02351Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum02351Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    yield break;
  }

  public IEnumerator onStartSceneAsync(
    Colosseum0235Scene.Param param,
    Colosseum02351Scene.Data data)
  {
    IEnumerator e = this.menu.Initialize(param, data.rank, data.fromPoint, data.nextPoint);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public class Data
  {
    public int fromPoint;
    public int nextPoint;

    public ColosseumRank rank { get; set; }

    public Data(ColosseumRank r, int fp, int np)
    {
      this.rank = r;
      this.fromPoint = fp;
      this.nextPoint = np;
    }
  }
}

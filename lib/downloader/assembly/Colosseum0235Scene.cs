// Decompiled with JetBrains decompiler
// Type: Colosseum0235Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class Colosseum0235Scene : NGSceneBase
{
  [SerializeField]
  private Colosseum0235Menu menu;

  public static void ChangeScene(Colosseum0235Scene.Param param)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene("colosseum023_5", false, (object) param);
  }

  public override IEnumerator onInitSceneAsync()
  {
    Colosseum0235Scene colosseum0235Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.ColosseumBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    colosseum0235Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    IEnumerator e = this.onStartSceneAsync(new Colosseum0235Scene.Param(4, 3, 20, (int[]) null, (ColosseumUtility.Info) null));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Colosseum0235Scene.Param param)
  {
    IEnumerator e = this.menu.Initialize(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public class Param
  {
    public bool isInit = true;
    public int maxRank = 4;
    public int nowId = 3;
    public Vector2 scrollPos = Vector2.zero;
    public int viewUnlockId = 20;
    public int[] opponents;
    public ColosseumUtility.Info collosseumInfo = new ColosseumUtility.Info();

    public Param(int m, int n, int v, int[] o, ColosseumUtility.Info info)
    {
      this.maxRank = m;
      this.nowId = n;
      this.viewUnlockId = v;
      this.opponents = o;
      this.collosseumInfo = info;
    }
  }
}

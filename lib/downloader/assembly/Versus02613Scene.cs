// Decompiled with JetBrains decompiler
// Type: Versus02613Scene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using SM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Versus02613Scene : NGSceneBase
{
  [SerializeField]
  private Versus02613Menu menu;
  private static readonly string SCENE_NAME = "versus026_13";

  public static void ChangeScene(
    bool stack,
    Versus02613Scene.BootParam param,
    PvPClassRecord info,
    int in_bestClass,
    string target_player_id,
    bool in_continueBackground)
  {
    param.push(Versus02613Scene.SCENE_NAME, new Versus02613Scene.BootArgument(Versus02613Scene.SCENE_NAME, info, in_bestClass, target_player_id, in_continueBackground));
    Singleton<NGSceneManager>.GetInstance().changeScene(Versus02613Scene.SCENE_NAME, (stack ? 1 : 0) != 0, (object) param);
  }

  public static void ChangeTopScene(
    bool stack,
    PvPClassRecord info,
    int in_bestClass,
    string target_player_id,
    bool in_continueBackground)
  {
    Singleton<NGSceneManager>.GetInstance().changeScene(Versus02613Scene.SCENE_NAME, (stack ? 1 : 0) != 0, (object) new Versus02613Scene.BootParam(Versus02613Scene.SCENE_NAME, info, in_bestClass, target_player_id, in_continueBackground));
  }

  public override IEnumerator onInitSceneAsync()
  {
    Versus02613Scene versus02613Scene = this;
    Future<GameObject> bgF = Res.Prefabs.BackGround.MultiBackground.Load<GameObject>();
    IEnumerator e = bgF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    versus02613Scene.backgroundPrefab = bgF.Result;
  }

  public IEnumerator onStartSceneAsync()
  {
    Versus02613Scene versus02613Scene = this;
    Future<WebAPI.Response.PvpBoot> futureF = WebAPI.PvpBoot();
    IEnumerator e = futureF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    WebAPI.Response.PvpBoot result = futureF.Result;
    e = versus02613Scene.onStartSceneAsync(new Versus02613Scene.BootParam(versus02613Scene.sceneName, result.pvp_class_record, result.best_class, SMManager.Get<Player>().id, false));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator onStartSceneAsync(Versus02613Scene.BootParam param)
  {
    Versus02613Scene versus02613Scene = this;
    versus02613Scene.continueBackground = param.current.continueBackground;
    ((Component) versus02613Scene.menu).gameObject.SetActive(false);
    IEnumerator e = versus02613Scene.menu.Initialize(param);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ((Component) versus02613Scene.menu).gameObject.SetActive(true);
  }

  public class BootParam
  {
    private Stack<Versus02613Scene.BootArgument> stack_ = new Stack<Versus02613Scene.BootArgument>();

    public BootParam(
      string scene_name,
      PvPClassRecord info,
      int bestClass,
      string playerId,
      bool continueBackground)
    {
      this.stack_.Push(new Versus02613Scene.BootArgument(scene_name, info, bestClass, playerId, continueBackground));
      this.current = this.stack_.Peek();
    }

    public void push(string scene_name, Versus02613Scene.BootArgument arg = null)
    {
      if (arg == null)
        arg = this.current;
      this.push(new Versus02613Scene.BootArgument(scene_name, arg));
    }

    public void push(Versus02613Scene.BootArgument arg)
    {
      this.stack_.Push(arg);
      this.current = this.stack_.Peek();
    }

    public Versus02613Scene.BootArgument pop()
    {
      Versus02613Scene.BootArgument bootArgument = this.stack_.Count > 1 ? this.stack_.Pop() : this.stack_.Peek();
      this.current = this.stack_.Peek();
      return bootArgument;
    }

    public Versus02613Scene.BootArgument current { get; private set; }
  }

  public class BootArgument
  {
    public string scene { get; private set; }

    public PvPClassRecord info { get; private set; }

    public int bestClass { get; private set; }

    public string playerId { get; private set; }

    public bool continueBackground { get; private set; }

    public string battleId { get; private set; }

    public BootArgument(
      string scene_name,
      PvPClassRecord pvp_record,
      int best_class,
      string player_id,
      bool continue_background)
    {
      this.scene = scene_name;
      this.info = pvp_record;
      this.bestClass = best_class;
      this.playerId = player_id;
      this.continueBackground = continue_background;
    }

    public BootArgument(
      string scene_name,
      Versus02613Scene.BootArgument arg,
      string battle_id = null,
      string player_id = null)
    {
      this.scene = scene_name;
      this.info = arg.info;
      this.bestClass = arg.bestClass;
      this.playerId = player_id != null ? player_id : arg.playerId;
      this.continueBackground = arg.continueBackground;
      this.battleId = battle_id != null ? battle_id : arg.battleId;
    }
  }
}

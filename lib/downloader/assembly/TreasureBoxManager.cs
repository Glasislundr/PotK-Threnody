// Decompiled with JetBrains decompiler
// Type: TreasureBoxManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class TreasureBoxManager : BattleManagerBase
{
  public string treasurebox_a_path = "Prefabs/BattleCommon/treasurebox/treasurebox_a/Prefab";
  public string treasurebox_b_path = "Prefabs/BattleCommon/treasurebox/treasurebox_b/Prefab";
  public string treasurebox_c_path = "Prefabs/BattleCommon/treasurebox/treasurebox_c/Prefab";
  public string dropicon_gold_path = "Prefabs/BattleCommon/get_item/dropicon_gold_M";
  public string dropicon_item_path = "Prefabs/BattleCommon/get_item/dropicon_item";
  public string dropicon_weapon_path = "Prefabs/BattleCommon/get_item/dropicon_weapon";
  public GameObject lastEffectPrefab;
  public GameObject startPrefab;
  public GameObject movePrefab_1;
  public GameObject movePrefab_2;
  public GameObject movePrefab_3;
  public GameObject treasurebox_a;
  public GameObject treasurebox_b;
  public GameObject treasurebox_c;
  public GameObject dropicon_gold;
  public GameObject dropicon_item;
  public GameObject dropicon_weapon;
  private const float moveTime = 0.2f;
  private NGBattleManager battleManager;
  private Queue<TreasureBoxManager.Continuer> exeQuene = new Queue<TreasureBoxManager.Continuer>();

  public override IEnumerator initialize(BattleInfo info, BE env_ = null)
  {
    ResourceManager rm = Singleton<ResourceManager>.GetInstance();
    Future<GameObject> startF = rm.Load<GameObject>("BattleEffects/field/ef026_item_get");
    IEnumerator e = startF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.startPrefab = startF.Result;
    Future<GameObject> moveF = rm.Load<GameObject>("BattleEffects/field/ef030_item_get_loop_1");
    e = moveF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.movePrefab_1 = moveF.Result;
    moveF = rm.Load<GameObject>("BattleEffects/field/ef030_item_get_loop_2");
    e = moveF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.movePrefab_2 = moveF.Result;
    moveF = rm.Load<GameObject>("BattleEffects/field/ef030_item_get_loop_3");
    e = moveF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.movePrefab_3 = moveF.Result;
    Future<GameObject> f = rm.Load<GameObject>("BattleEffects/field/ef034_item_get_fix");
    e = f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.lastEffectPrefab = f.Result;
    Future<GameObject> treasurebox_a_f = rm.Load<GameObject>(this.treasurebox_a_path);
    e = treasurebox_a_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.treasurebox_a = treasurebox_a_f.Result;
    Future<GameObject> treasurebox_b_f = rm.Load<GameObject>(this.treasurebox_b_path);
    e = treasurebox_b_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.treasurebox_b = treasurebox_b_f.Result;
    Future<GameObject> treasurebox_c_f = rm.Load<GameObject>(this.treasurebox_c_path);
    e = treasurebox_c_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.treasurebox_c = treasurebox_c_f.Result;
    Future<GameObject> dropicon_gold_f = rm.Load<GameObject>(this.dropicon_gold_path);
    e = dropicon_gold_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dropicon_gold = dropicon_gold_f.Result;
    Future<GameObject> dropicon_item_f = rm.Load<GameObject>(this.dropicon_item_path);
    e = dropicon_item_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dropicon_item = dropicon_item_f.Result;
    Future<GameObject> dropicon_weapon_f = rm.Load<GameObject>(this.dropicon_weapon_path);
    e = dropicon_weapon_f.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    this.dropicon_weapon = dropicon_weapon_f.Result;
  }

  public override IEnumerator cleanup()
  {
    this.startPrefab = (GameObject) null;
    this.movePrefab_1 = (GameObject) null;
    this.movePrefab_2 = (GameObject) null;
    this.movePrefab_3 = (GameObject) null;
    this.treasurebox_a = (GameObject) null;
    this.treasurebox_b = (GameObject) null;
    this.treasurebox_c = (GameObject) null;
    this.dropicon_gold = (GameObject) null;
    this.dropicon_item = (GameObject) null;
    this.dropicon_weapon = (GameObject) null;
    this.exeQuene.Clear();
    yield break;
  }

  public void execute(
    BL.DropData drop,
    BL.Panel panel,
    Vector3 target,
    Action<BL.DropData> firstCallback,
    Action<BL.DropData> endCallback,
    float time)
  {
    this.battleManager = Singleton<NGBattleManager>.GetInstance();
    this.battleManager.getManager<BattleTimeManager>().setSchedule((Schedule) new TreasureBoxManager.ExecuteSchedule(this, new TreasureBoxManager.Continuer(drop, panel, target, firstCallback, endCallback, time)));
  }

  public void cloneMoveEffect()
  {
    if (this.exeQuene.Count == 0)
      return;
    this.exeQuene.Peek().cloneMoveEffect(this);
  }

  public void startEffect()
  {
    if (this.exeQuene.Count == 0)
      return;
    this.exeQuene.Peek().startEffect(this);
  }

  private IEnumerator doExecute()
  {
    TreasureBoxManager tm = this;
    tm.battleManager.isBattleEnable = false;
    while (tm.exeQuene.Count > 0)
    {
      TreasureBoxManager.Continuer c = tm.exeQuene.Peek();
      c.execute(tm);
      while (c.isRunnging)
      {
        c.update(tm);
        yield return (object) null;
      }
      tm.exeQuene.Dequeue();
      c = (TreasureBoxManager.Continuer) null;
    }
    tm.battleManager.isBattleEnable = true;
  }

  private class ExecuteSchedule : ScheduleEnumerator
  {
    private TreasureBoxManager parent;
    private TreasureBoxManager.Continuer c;

    public ExecuteSchedule(TreasureBoxManager parent, TreasureBoxManager.Continuer c)
    {
      this.parent = parent;
      this.c = c;
      this.isCompleted = false;
      this.isInsertMode = true;
    }

    public override IEnumerator doBody()
    {
      TreasureBoxManager.ExecuteSchedule executeSchedule = this;
      executeSchedule.parent.exeQuene.Enqueue(executeSchedule.c);
      if (executeSchedule.parent.exeQuene.Count == 1)
        executeSchedule.parent.StartCoroutine(executeSchedule.parent.doExecute());
      do
      {
        yield return (object) null;
      }
      while (executeSchedule.parent.exeQuene.Count != 0 || executeSchedule.c.isRunnging);
      executeSchedule.isCompleted = true;
    }
  }

  private class Continuer
  {
    public bool isRunnging;
    private BL.Panel panel;
    private Vector3 target;
    private float startTime;
    private GameObject boxObject;
    private GameObject moveEffect;
    private GameObject lastEffect;
    private BL.DropData drop;
    private Action<BL.DropData> firstCallback;
    private Action<BL.DropData> endCallback;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private Transform cameraParent;
    private Camera frontCamera;
    private float timeoutTime;
    private bool isDoEndRunning;

    public Continuer(
      BL.DropData drop,
      BL.Panel panel,
      Vector3 target,
      Action<BL.DropData> firstCallback,
      Action<BL.DropData> endCallback,
      float time)
    {
      this.drop = drop;
      this.panel = panel;
      this.target = target;
      this.firstCallback = firstCallback;
      this.endCallback = endCallback;
      this.timeoutTime = time;
    }

    public void execute(TreasureBoxManager tm)
    {
      this.isRunnging = true;
      Camera[] componentsInChildren = tm.battleManager.battleCamera.GetComponentsInChildren<Camera>(true);
      if (this.drop.isDropBox)
      {
        this.cloneBoxPrefab(this.drop, this.panel, tm);
        Animator componentInChildren = this.boxObject.GetComponentInChildren<Animator>();
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          componentInChildren.SetTrigger("open");
        else
          Debug.LogWarning((object) " === GetComponentInChildren<Animator> BUG!");
        Vector3 vector3 = Vector3.op_Multiply(Vector3.forward, Vector3.Distance(this.boxObject.transform.position, ((Component) componentsInChildren[0]).transform.position));
        this.targetPosition = componentsInChildren[0].ScreenToWorldPoint(Vector3.op_Addition(this.target, vector3));
        this.velocity = Vector3.zero;
        foreach (Camera camera in componentsInChildren)
        {
          if (((Object) camera).name == "3D Camera Front")
          {
            this.frontCamera = camera;
            this.cameraParent = ((Component) this.frontCamera).transform.parent;
            ((Component) this.frontCamera).transform.parent = (Transform) null;
          }
        }
      }
      else if (this.firstCallback != null)
        this.firstCallback(this.drop);
      this.startTime = Time.time;
    }

    public void cloneMoveEffect(TreasureBoxManager tm)
    {
      if (Object.op_Equality((Object) this.boxObject, (Object) null))
        return;
      if (this.drop.rarity <= 1)
        this.moveEffect = tm.movePrefab_1.Clone(this.boxObject.transform);
      else if (this.drop.rarity <= 3)
        this.moveEffect = tm.movePrefab_2.Clone(this.boxObject.transform);
      else
        this.moveEffect = tm.movePrefab_3.Clone(this.boxObject.transform);
    }

    public void startEffect(TreasureBoxManager tm)
    {
      GameObject self = (GameObject) null;
      switch (this.drop.reward.Type)
      {
        case MasterDataTable.CommonRewardType.supply:
        case MasterDataTable.CommonRewardType.quest_key:
        case MasterDataTable.CommonRewardType.gacha_ticket:
        case MasterDataTable.CommonRewardType.reincarnation_type_ticket:
          self = tm.dropicon_item;
          break;
        case MasterDataTable.CommonRewardType.gear:
        case MasterDataTable.CommonRewardType.gear_body:
          self = tm.dropicon_weapon;
          break;
        case MasterDataTable.CommonRewardType.money:
          self = tm.dropicon_gold;
          break;
      }
      if (Object.op_Inequality((Object) self, (Object) null))
        self.Clone(this.boxObject.transform);
      tm.startPrefab.Clone(this.boxObject.transform);
    }

    private void cloneBoxPrefab(BL.DropData drop, BL.Panel panel, TreasureBoxManager tm)
    {
      BE.PanelResource panelResource = Singleton<NGBattleManager>.GetInstance().environment.panelResource[panel];
      Transform transform = panelResource.gameObject.transform;
      this.boxObject = drop.rarity > 1 ? (drop.rarity > 3 ? tm.treasurebox_c.Clone(transform) : tm.treasurebox_b.Clone(transform)) : tm.treasurebox_a.Clone(transform);
      this.boxObject.transform.localPosition = new Vector3(0.0f, panelResource.gameObject.GetComponent<BattlePanelParts>().getHeight() + 3f, 0.0f);
    }

    public void update(TreasureBoxManager tm)
    {
      if (this.endCallback == null && Object.op_Equality((Object) this.boxObject, (Object) null) || this.isDoEndRunning)
        return;
      if ((double) Time.time - (double) this.startTime > (double) this.timeoutTime)
      {
        tm.StartCoroutine(this.doEnd(tm));
      }
      else
      {
        if (!Object.op_Inequality((Object) this.moveEffect, (Object) null))
          return;
        this.moveEffect.transform.position = Vector3.SmoothDamp(this.moveEffect.transform.position, this.targetPosition, ref this.velocity, 0.2f);
        if ((double) Vector3.Distance(this.moveEffect.transform.position, this.targetPosition) >= 0.0099999997764825821)
          return;
        tm.StartCoroutine(this.doEnd(tm));
      }
    }

    private IEnumerator doEnd(TreasureBoxManager tm)
    {
      this.isDoEndRunning = true;
      if (Object.op_Inequality((Object) this.boxObject, (Object) null))
      {
        if (Object.op_Inequality((Object) this.moveEffect, (Object) null))
        {
          this.lastEffect = tm.lastEffectPrefab.Clone(this.boxObject.transform);
          this.lastEffect.transform.localPosition = this.moveEffect.transform.localPosition;
          this.moveEffect.SetActive(false);
        }
        yield return (object) new WaitForSeconds(0.8f);
        if (Object.op_Inequality((Object) this.frontCamera, (Object) null))
        {
          ((Component) this.frontCamera).transform.parent = this.cameraParent;
          ((Component) this.frontCamera).transform.localPosition = Vector3.zero;
          this.frontCamera = (Camera) null;
        }
        Object.Destroy((Object) this.boxObject);
        this.boxObject = (GameObject) null;
        this.moveEffect = (GameObject) null;
        this.lastEffect = (GameObject) null;
        if (this.endCallback != null)
        {
          this.endCallback(this.drop);
          this.endCallback = (Action<BL.DropData>) null;
          this.drop = (BL.DropData) null;
        }
      }
      else if (this.endCallback != null)
      {
        this.endCallback(this.drop);
        this.endCallback = (Action<BL.DropData>) null;
        this.drop = (BL.DropData) null;
      }
      this.isDoEndRunning = false;
      this.isRunnging = false;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: SeaHomeUnitController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

#nullable disable
public class SeaHomeUnitController : MonoBehaviour
{
  [SerializeField]
  private Transform m_OutR;
  [SerializeField]
  private Transform m_InR;
  [SerializeField]
  private Transform m_InC;
  [SerializeField]
  private Transform m_InL;
  [SerializeField]
  private Transform m_OutL;
  [SerializeField]
  private float m_FadeTime = 0.3f;
  private const int ACTION_TYPE_MOVE = 0;
  private const int ACTION_TYPE_ACTION = 1;
  private const int ACTION_TYPE_SIT = 2;
  private const int ACTION_TYPE_DANCE = 3;
  private const int ACTION_SIT_TYPE_WAIT = 0;
  private const int ACTION_SIT_TYPE_ACTION = 1;
  private const int NO_ACTION = 2147483647;
  private List<int> actionList = new List<int>()
  {
    0,
    1,
    1,
    1,
    1,
    1,
    2,
    int.MaxValue,
    int.MaxValue,
    int.MaxValue
  };
  private List<int> sitActionList = new List<int>()
  {
    0,
    1,
    1,
    1,
    int.MaxValue,
    int.MaxValue,
    int.MaxValue,
    int.MaxValue,
    int.MaxValue
  };
  private SeaHomeManager owner;
  private bool isInit;
  private SeaHomeManager.UnitConrtolleData unitData;
  private SeaHomeUnitController.UnitPositions nowPosition;
  private SeaHomeUnitController.UnitPositions targetPosition;
  private GameObject unitObject;
  private GameObject effectShadow;
  private Transform unitTransform;
  private Transform bipTransform;
  private Animator myAnimator;
  private SeaHomeUnitController.UnitStatus status;
  private SeaHomeUnitController.UnitStatus prevStatus;
  private float actionWait;
  private float hideWeight;
  private float goalWeight = 1f;
  private float startWeight;
  private bool nowFading;
  private float fadeElapsedTime;
  private int alphaLayerIndex;
  private float velocity;
  private float acceleration;
  private Vector3 direction = Vector3.zero;
  private int actionCount;
  private int sitActionCount = -1;
  private int waitCount;
  public static readonly SeaHomeUnitController.UnitPositions[] allPositions = new SeaHomeUnitController.UnitPositions[5]
  {
    SeaHomeUnitController.UnitPositions.inR,
    SeaHomeUnitController.UnitPositions.inC,
    SeaHomeUnitController.UnitPositions.inL,
    SeaHomeUnitController.UnitPositions.outR,
    SeaHomeUnitController.UnitPositions.outL
  };
  public static readonly SeaHomeUnitController.UnitPositions[] inPositions = new SeaHomeUnitController.UnitPositions[3]
  {
    SeaHomeUnitController.UnitPositions.inR,
    SeaHomeUnitController.UnitPositions.inC,
    SeaHomeUnitController.UnitPositions.inL
  };
  public static readonly SeaHomeUnitController.UnitPositions[] outPositions = new SeaHomeUnitController.UnitPositions[2]
  {
    SeaHomeUnitController.UnitPositions.outR,
    SeaHomeUnitController.UnitPositions.outL
  };

  public bool IsInit => this.isInit;

  public SeaHomeManager.UnitConrtolleData UnitData => this.unitData;

  public SeaHomeUnitController.UnitPositions NowPosition => this.nowPosition;

  public SeaHomeUnitController.UnitPositions TargetPosition => this.targetPosition;

  public Transform UnitTransform => this.unitTransform;

  public SeaHomeUnitController.UnitStatus Status => this.status;

  public SeaHomeUnitController.UnitStatus PrevStatus => this.prevStatus;

  public bool NowStand => this.status == SeaHomeUnitController.UnitStatus.Stand;

  public bool NowWalk => this.status == SeaHomeUnitController.UnitStatus.Walk;

  public bool NowAction => this.status == SeaHomeUnitController.UnitStatus.Action;

  public bool NowSit => this.status == SeaHomeUnitController.UnitStatus.Sit;

  public bool NowDance => this.status == SeaHomeUnitController.UnitStatus.Dance;

  public bool NowWait => (double) this.actionWait > 0.0;

  public bool NowHide => !this.nowFading && Mathf.Approximately(this.hideWeight, 1f);

  public void Clear()
  {
    this.nowPosition = this.targetPosition = SeaHomeUnitController.UnitPositions.outR;
    this.status = this.prevStatus = SeaHomeUnitController.UnitStatus.Stand;
    this.actionWait = 0.0f;
    this.hideWeight = 0.0f;
    this.nowFading = false;
    this.velocity = 0.0f;
    this.acceleration = 0.0f;
    this.direction = Vector3.zero;
    this.actionCount = 0;
    this.sitActionCount = -1;
    this.waitCount = 0;
    Object.DestroyObject((Object) this.unitObject);
    this.unitObject = (GameObject) null;
    this.effectShadow = (GameObject) null;
    this.unitTransform = (Transform) null;
    this.bipTransform = (Transform) null;
    this.myAnimator = (Animator) null;
    this.isInit = false;
  }

  public void SetUnitData(SeaHomeManager.UnitConrtolleData unitData, SeaHomeManager owner)
  {
    this.owner = owner;
    this.unitData = unitData;
  }

  public IEnumerator Init(GameObject shadowEffect)
  {
    SeaHomeUnitController homeUnitController = this;
    homeUnitController.Clear();
    UnitUnit unit = homeUnitController.unitData.Unit;
    Future<GameObject> duelModelF = unit.LoadModelDuel();
    IEnumerator e = duelModelF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    homeUnitController.unitObject = duelModelF.Result.Clone(Singleton<CommonRoot>.GetInstance().LoadTmpObj.transform);
    homeUnitController.SetLayer(homeUnitController.unitObject.transform, LayerMask.NameToLayer("3DModels"));
    homeUnitController.bipTransform = homeUnitController.unitObject.transform.Find("Bip");
    homeUnitController.unitTransform = homeUnitController.unitObject.transform;
    homeUnitController.unitTransform.localScale = new Vector3(unit.duel_model_scale, unit.duel_model_scale, unit.duel_model_scale);
    homeUnitController.effectShadow = shadowEffect.Clone(homeUnitController.unitTransform);
    homeUnitController.effectShadow.transform.localScale = new Vector3(unit.duel_shadow_scale_x, 1f, unit.duel_shadow_scale_z);
    homeUnitController.effectShadow.transform.rotation = Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f));
    homeUnitController.effectShadow.AddComponent<EffectShadowController>().SetTransfome(homeUnitController.bipTransform);
    homeUnitController.myAnimator = homeUnitController.unitObject.GetComponentInChildren<Animator>();
    Future<RuntimeAnimatorController> homeAnimatorF = unit.LoadHomeDuelAnimator();
    e = homeAnimatorF.Wait();
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    homeUnitController.myAnimator.runtimeAnimatorController = homeAnimatorF.Result;
    homeUnitController.alphaLayerIndex = homeUnitController.myAnimator.GetLayerIndex("Alpha Layer");
    homeUnitController.actionCount = homeUnitController.myAnimator.GetInteger("action_count");
    homeUnitController.sitActionCount = homeUnitController.myAnimator.GetInteger("sit_action_count");
    homeUnitController.waitCount = homeUnitController.myAnimator.GetInteger("wait_count");
    CapsuleCollider orAddComponent = ((Component) homeUnitController.bipTransform).gameObject.GetOrAddComponent<CapsuleCollider>();
    orAddComponent.radius = 0.6f;
    orAddComponent.center = new Vector3(0.0f, 0.0f, 0.2f);
    orAddComponent.height = 2f;
    orAddComponent.direction = 2;
    homeUnitController.unitObject.GetOrAddComponent<SeaHomeUnitAnimeCallback>().controller = homeUnitController;
    homeUnitController.unitObject.SetParent(((Component) ((Component) homeUnitController).transform).gameObject);
    homeUnitController.targetPosition = homeUnitController.nowPosition = SeaHomeUnitController.outPositions[Random.Range(0, 100000) % SeaHomeUnitController.outPositions.Length];
    homeUnitController.unitTransform.localPosition = homeUnitController.GetPosition(homeUnitController.nowPosition);
    homeUnitController.ResetActionWait();
    homeUnitController.myAnimator.SetLayerWeight(homeUnitController.alphaLayerIndex, homeUnitController.hideWeight);
    if (homeUnitController.NowHide)
      homeUnitController.effectShadow.SetActive(false);
    else
      homeUnitController.effectShadow.SetActive(true);
    homeUnitController.isInit = true;
  }

  private void OnDisable()
  {
    if (this.nowPosition == this.targetPosition)
      return;
    this.ForceWait();
  }

  private void ForceWait()
  {
    this.unitTransform.localPosition = this.GetPosition(this.targetPosition);
    this.targetPosition = this.nowPosition;
    this.acceleration = 0.0f;
    this.velocity = 0.0f;
    this.SetTriggerToAnimation("force_wait");
  }

  private void Update()
  {
    if (!this.isInit)
      return;
    if (this.NowHide)
    {
      if (this.nowPosition != this.targetPosition)
      {
        this.ForceWait();
      }
      else
      {
        if (!((Component) this).gameObject.activeSelf)
          return;
        ((Component) this).gameObject.SetActive(false);
      }
    }
    else if (this.nowFading)
    {
      this.fadeElapsedTime += Time.deltaTime;
      this.hideWeight = Mathf.Lerp(this.startWeight, this.goalWeight, this.fadeElapsedTime / this.m_FadeTime);
      this.myAnimator.SetLayerWeight(this.alphaLayerIndex, this.hideWeight);
      if (!Mathf.Approximately(this.hideWeight, this.goalWeight))
        return;
      this.nowFading = false;
    }
    else if (this.nowPosition != this.targetPosition)
    {
      if ((double) this.velocity == 0.0 && (double) this.acceleration == 0.0)
        return;
      this.velocity = Mathf.Max(this.velocity + this.acceleration * Time.deltaTime, 0.0f);
      Vector3 vector3 = Vector3.op_Multiply(Vector3.op_Multiply(this.direction, this.velocity), Time.deltaTime);
      this.unitTransform.localPosition = new Vector3(this.unitTransform.localPosition.x + vector3.x, this.unitTransform.localPosition.y + vector3.y, this.unitTransform.localPosition.z + vector3.z);
    }
    else
    {
      if ((double) this.actionWait <= 0.0)
        return;
      this.actionWait -= Time.deltaTime;
      if ((double) this.actionWait > 0.0)
        return;
      if (this.nowPosition == SeaHomeUnitController.UnitPositions.outR || this.nowPosition == SeaHomeUnitController.UnitPositions.outL || this.owner.DuplicatePosition(this))
      {
        if (this.NowSit)
        {
          this.SetTriggerToAnimation("wait");
          this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Stand);
        }
        else
          this.Move(this.owner.GetNextMovePosition(this));
      }
      else if (this.NowSit)
      {
        switch (this.sitActionList.Shuffle<int>().First<int>())
        {
          case 0:
            this.SetTriggerToAnimation("wait");
            this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Stand);
            break;
          case 1:
            if (this.sitActionCount > 0)
            {
              this.SetIndexToAnimation("sit_action_index", Random.Range(0, 100000) % this.sitActionCount);
              this.SetTriggerToAnimation("sit_action");
              break;
            }
            this.ResetActionWait();
            break;
          default:
            this.ResetActionWait();
            break;
        }
      }
      else if (this.owner.CheckDance(this) && Random.Range(0, 100) <= 5)
      {
        this.owner.PlayDance();
      }
      else
      {
        switch (this.actionList.Shuffle<int>().First<int>())
        {
          case 0:
            if (this.Move(this.owner.GetNextMovePosition(this)))
              break;
            this.ResetActionWait();
            break;
          case 1:
            this.SetIndexToAnimation("action_index", Random.Range(0, 100000) % this.actionCount);
            this.SetTriggerToAnimation("action");
            break;
          case 2:
            if (this.sitActionCount >= 0)
            {
              this.SetTriggerToAnimation("sit");
              this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Sit);
              break;
            }
            this.ResetActionWait();
            break;
          default:
            this.ResetActionWait();
            break;
        }
      }
    }
  }

  private Vector3 GetPosition(SeaHomeUnitController.UnitPositions position)
  {
    switch (position)
    {
      case SeaHomeUnitController.UnitPositions.outR:
        return this.m_OutR.localPosition;
      case SeaHomeUnitController.UnitPositions.inR:
        return this.m_InR.localPosition;
      case SeaHomeUnitController.UnitPositions.inC:
        return this.m_InC.localPosition;
      case SeaHomeUnitController.UnitPositions.inL:
        return this.m_InL.localPosition;
      case SeaHomeUnitController.UnitPositions.outL:
        return this.m_OutL.localPosition;
      default:
        return new Vector3();
    }
  }

  private bool Move(SeaHomeUnitController.UnitPositions target)
  {
    if (this.nowPosition == target)
      return false;
    this.targetPosition = target;
    this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Walk);
    this.direction = Vector3.op_Subtraction(this.GetPosition(this.targetPosition), this.GetPosition(this.nowPosition));
    ((Vector3) ref this.direction).Normalize();
    if (this.waitCount > 1 && Random.Range(0, 10000) <= 1000)
      this.SetIndexToAnimation("wait_index", Random.Range(0, 100) % this.waitCount);
    else
      this.SetIndexToAnimation("wait_index", 0);
    if (this.targetPosition > this.nowPosition)
      this.SetTriggerToAnimation("walk_l");
    else
      this.SetTriggerToAnimation("walk_r");
    return true;
  }

  public void StartMove(float acceleration)
  {
    if (this.nowPosition == this.targetPosition)
      return;
    this.acceleration = acceleration;
  }

  public void LoopMove()
  {
    if (this.nowPosition == this.targetPosition)
      return;
    this.acceleration = 0.0f;
  }

  public void CheckStopMove()
  {
    if (this.nowPosition == this.targetPosition)
      return;
    AnimatorStateInfo animeState = this.myAnimator.GetCurrentAnimatorStateInfo(0);
    AnimatorClipInfo animatorClipInfo = ((IEnumerable<AnimatorClipInfo>) this.myAnimator.GetCurrentAnimatorClipInfo(0)).FirstOrDefault<AnimatorClipInfo>((Func<AnimatorClipInfo, bool>) (x => ((AnimatorStateInfo) ref animeState).IsName(((Object) ((AnimatorClipInfo) ref x).clip).name)));
    float length = ((AnimatorClipInfo) ref animatorClipInfo).clip.length;
    Vector3 vector3_1 = Vector3.op_Multiply(Vector3.op_Multiply(this.direction, this.velocity), length);
    Vector3 vector3_2 = Vector3.op_Addition(this.unitTransform.localPosition, vector3_1);
    Vector3 position = this.GetPosition(this.nowPosition);
    Vector3 vector3_3 = position;
    Vector3 vector3_4 = Vector3.op_Subtraction(vector3_2, vector3_3);
    Vector3 vector3_5 = Vector3.op_Subtraction(Vector3.op_Subtraction(this.GetPosition(this.targetPosition), Vector3.op_Division(vector3_1, 2f)), position);
    if ((double) ((Vector3) ref vector3_4).sqrMagnitude <= (double) ((Vector3) ref vector3_5).sqrMagnitude)
      return;
    this.SetTriggerToAnimation("wait");
    this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Stand);
  }

  public void StopStartMove(float acceleration)
  {
    if (this.nowPosition == this.targetPosition)
      return;
    this.acceleration = acceleration;
  }

  public void StopMove()
  {
    if (this.nowPosition == this.targetPosition)
      return;
    this.acceleration = 0.0f;
    this.velocity = 0.0f;
    this.nowPosition = this.targetPosition = this.GetTransformNearPosition();
  }

  public SeaHomeUnitController.UnitPositions GetTransformNearPosition()
  {
    float num = float.MaxValue;
    SeaHomeUnitController.UnitPositions transformNearPosition = SeaHomeUnitController.UnitPositions.outR;
    foreach (SeaHomeUnitController.UnitPositions allPosition in SeaHomeUnitController.allPositions)
    {
      Vector3 vector3 = Vector3.op_Subtraction(this.GetPosition(allPosition), this.unitTransform.localPosition);
      float sqrMagnitude = ((Vector3) ref vector3).sqrMagnitude;
      if ((double) num > (double) sqrMagnitude)
      {
        transformNearPosition = allPosition;
        num = sqrMagnitude;
      }
    }
    return transformNearPosition;
  }

  private void SetTriggerToAnimation(string trigger) => this.myAnimator.SetTrigger(trigger);

  private void SetIndexToAnimation(string name, int index)
  {
    this.myAnimator.SetInteger(name, index);
  }

  public void Hide()
  {
    if (Object.op_Equality((Object) this.unitObject, (Object) null))
    {
      this.hideWeight = 1f;
    }
    else
    {
      if (Mathf.Approximately(this.hideWeight, 1f))
        return;
      this.startWeight = this.hideWeight;
      this.goalWeight = 1f;
      this.nowFading = true;
      this.fadeElapsedTime = 0.0f;
      this.effectShadow.SetActive(false);
    }
  }

  public void Show()
  {
    if (Object.op_Equality((Object) this.unitObject, (Object) null))
    {
      this.hideWeight = 0.0f;
    }
    else
    {
      if (Mathf.Approximately(this.hideWeight, 0.0f))
        return;
      ((Component) this).gameObject.SetActive(true);
      this.myAnimator.SetLayerWeight(this.alphaLayerIndex, this.hideWeight);
      this.startWeight = this.hideWeight;
      this.goalWeight = 0.0f;
      this.nowFading = true;
      this.fadeElapsedTime = 0.0f;
      this.effectShadow.SetActive(true);
    }
  }

  public void ResetActionWait() => this.actionWait = Random.Range(1f, 3f);

  public void SetLookuped() => this.owner.SetLookupedAuto(this);

  public void ResetLookuped() => this.owner.ResetLookupedAuto(this);

  public bool IsTouch(GameObject bipObject)
  {
    return Object.op_Inequality((Object) this.bipTransform, (Object) null) && ((Component) this.bipTransform).gameObject.Equals((object) bipObject);
  }

  public virtual string ToString()
  {
    return this.unitData.Unit != null ? this.unitData.Unit.name : string.Empty;
  }

  private void SetLayer(Transform trans, int layer)
  {
    ((Component) trans).gameObject.layer = layer;
    foreach (Transform tran in trans)
      this.SetLayer(tran, layer);
  }

  public void PlayDance()
  {
    this.SetTriggerToAnimation("dance");
    this.actionWait = -1f;
    this.SetUnitStatus(SeaHomeUnitController.UnitStatus.Dance);
  }

  public void SetUnitStatus(SeaHomeUnitController.UnitStatus status)
  {
    if (this.status == status)
      return;
    if (this.status == SeaHomeUnitController.UnitStatus.Dance)
      this.owner.EndDance();
    this.prevStatus = this.status;
    this.status = status;
  }

  public void UpdatePlayerUnit(PlayerUnit playerUnit) => this.unitData.PlayerUnit = playerUnit;

  public enum UnitPositions
  {
    outR,
    inR,
    inC,
    inL,
    outL,
  }

  public enum UnitStatus
  {
    Stand,
    Walk,
    Action,
    Sit,
    Dance,
  }
}

// Decompiled with JetBrains decompiler
// Type: UI3DModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using MasterDataTable;
using SM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class UI3DModel : MonoBehaviour
{
  public bool lightOn = true;
  [SerializeField]
  private Transform ActionTarget;
  public Transform ModelTarget;
  public Camera ModelCamera;
  [SerializeField]
  private UITexture ui_texture;
  public List<string> PlayAnimStateList;
  public float SetScale = 220f;
  public UI3DModelCreate model_creater_;
  private GameObject Model;
  private Transform ModelTrans;
  private GameObject lightGameObject;
  private int debug_number;
  private bool isPlay;
  private bool isGear;
  private bool pick_up;
  private bool unit_edit;
  public bool isNotLotate;
  private float wait_counter;
  public UIWidget widget;
  public UIRoot root;
  private const int DEFAULT_SHIELD = 71002;
  private Dictionary<string, string> dicState;

  public UITexture uiTexture => this.ui_texture;

  public Texture GetRenderTexture() => (Texture) this.ModelCamera.targetTexture;

  public void setModelScale(Vector3 scale) => this.ModelTrans.localScale = scale;

  private void Awake()
  {
    this.root = Object.FindObjectOfType<UIRoot>();
    this.widget = ((Component) this).GetComponent<UIWidget>();
  }

  public void SetActiveAlpha(bool flag)
  {
    if (flag)
      ((UIRect) this.widget).alpha = 1f;
    else
      ((UIRect) this.widget).alpha = 0.0f;
  }

  public void Remove()
  {
    if (!Object.op_Inequality((Object) this.Model, (Object) null))
      return;
    Object.Destroy((Object) this.Model);
  }

  public IEnumerator UnitEdit(PlayerUnit pu)
  {
    UI3DModel ui3Dmodel = this;
    if (Object.op_Inequality((Object) ui3Dmodel.Model, (Object) null))
      Object.Destroy((Object) ui3Dmodel.Model);
    ((Component) ui3Dmodel).GetComponent<Collider>().enabled = false;
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = false;
    ui3Dmodel.unit_edit = true;
    ui3Dmodel.isGear = false;
    ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.GetOrAddComponent<UI3DModelCreate>();
    ui3Dmodel.model_creater_.winAnimator_ = false;
    IEnumerator e = ui3Dmodel.model_creater_.CreateModel(pu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui3Dmodel.UnitEditInit(pu.unit.camera_pattern);
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = true;
  }

  public IEnumerator JobChange(PlayerUnit pu)
  {
    if (Object.op_Inequality((Object) this.Model, (Object) null))
      Object.Destroy((Object) this.Model);
    this.unit_edit = true;
    ((Behaviour) this.ModelCamera).enabled = false;
    this.isGear = false;
    this.model_creater_ = ((Component) this.ModelTarget).gameObject.GetComponent<UI3DModelCreate>();
    if (Object.op_Equality((Object) this.model_creater_, (Object) null))
      this.model_creater_ = ((Component) this.ModelTarget).gameObject.AddComponent<UI3DModelCreate>();
    this.model_creater_.winAnimator_ = false;
    yield return (object) this.model_creater_.CreateModel(pu);
    this.JobChangeInit(pu.getJobData()?.rendering_pattern?.camera_pattern);
    ((Behaviour) this.ModelCamera).enabled = true;
  }

  public IEnumerator Unit(PlayerUnit pu, System.Action callback = null)
  {
    UI3DModel ui3Dmodel = this;
    if (Object.op_Inequality((Object) ui3Dmodel.Model, (Object) null))
      Object.Destroy((Object) ui3Dmodel.Model);
    ((Component) ui3Dmodel).GetComponent<Collider>().enabled = true;
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = false;
    ui3Dmodel.isGear = false;
    ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.GetComponent<UI3DModelCreate>();
    if (Object.op_Equality((Object) ui3Dmodel.model_creater_, (Object) null))
      ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.AddComponent<UI3DModelCreate>();
    ui3Dmodel.model_creater_.winAnimator_ = false;
    IEnumerator e = ui3Dmodel.model_creater_.CreateModel(pu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui3Dmodel.Init(pu.unit.camera_pattern);
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = true;
    if (callback != null)
      callback();
  }

  public IEnumerator Unit3D(PlayerUnit unit)
  {
    if (Object.op_Inequality((Object) this.Model, (Object) null))
      Object.Destroy((Object) this.Model);
    ((Behaviour) this.ModelCamera).enabled = false;
    this.isGear = false;
    this.model_creater_ = ((Component) this.ModelTarget).gameObject.GetComponent<UI3DModelCreate>();
    if (Object.op_Equality((Object) this.model_creater_, (Object) null))
      this.model_creater_ = ((Component) this.ModelTarget).gameObject.AddComponent<UI3DModelCreate>();
    this.model_creater_.winAnimator_ = false;
    IEnumerator e = this.model_creater_.CreateModel(unit, new int?(71002));
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
  }

  public IEnumerator HistoryUnit(UnitUnit unit)
  {
    UI3DModel ui3Dmodel = this;
    if (Object.op_Inequality((Object) ui3Dmodel.Model, (Object) null))
      Object.Destroy((Object) ui3Dmodel.Model);
    ((Component) ui3Dmodel).GetComponent<Collider>().enabled = true;
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = false;
    ui3Dmodel.isGear = false;
    ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.GetComponent<UI3DModelCreate>();
    if (Object.op_Equality((Object) ui3Dmodel.model_creater_, (Object) null))
      ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.AddComponent<UI3DModelCreate>();
    ui3Dmodel.model_creater_.winAnimator_ = false;
    IEnumerator e = ui3Dmodel.model_creater_.CreateModel(unit);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui3Dmodel.HistoryInit(unit.camera_pattern);
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = true;
  }

  public IEnumerator PickUpUnit(UnitUnit uu)
  {
    UI3DModel ui3Dmodel = this;
    if (Object.op_Inequality((Object) ui3Dmodel.Model, (Object) null))
      Object.Destroy((Object) ui3Dmodel.Model);
    ui3Dmodel.pick_up = true;
    ((Component) ui3Dmodel).GetComponent<Collider>().enabled = false;
    ++ui3Dmodel.debug_number;
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = false;
    ui3Dmodel.isGear = false;
    ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.GetComponent<UI3DModelCreate>();
    if (Object.op_Equality((Object) ui3Dmodel.model_creater_, (Object) null))
      ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.AddComponent<UI3DModelCreate>();
    ui3Dmodel.model_creater_.winAnimator_ = true;
    IEnumerator e = ui3Dmodel.model_creater_.CreateModel(uu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui3Dmodel.PickUpUnitInit(uu.camera_pattern);
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = true;
  }

  public IEnumerator GuildUnit(PlayerUnit pu, Material mat)
  {
    UI3DModel ui3Dmodel = this;
    if (Object.op_Inequality((Object) ui3Dmodel.Model, (Object) null))
      Object.Destroy((Object) ui3Dmodel.Model);
    ui3Dmodel.model_creater_ = ((Component) ui3Dmodel.ModelTarget).gameObject.GetOrAddComponent<UI3DModelCreate>();
    ui3Dmodel.model_creater_.winAnimator_ = false;
    ((Component) ui3Dmodel).GetComponent<Collider>().enabled = false;
    ui3Dmodel.isGear = false;
    ui3Dmodel.isNotLotate = true;
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = false;
    IEnumerator e = ui3Dmodel.model_creater_.CreateModel(pu);
    while (e.MoveNext())
      yield return e.Current;
    e = (IEnumerator) null;
    ui3Dmodel.GuildInit(pu.unit.camera_pattern, 25f, 56, 78, mat);
    ((Behaviour) ui3Dmodel.ModelCamera).enabled = true;
  }

  public void Action()
  {
    this.StartCoroutine(this.CenterRotate());
    if (Object.op_Equality((Object) this.model_creater_.UnitAnimator, (Object) null) || this.isPlay)
      return;
    AnimatorStateInfo animatorStateInfo = this.model_creater_.UnitAnimator.GetCurrentAnimatorStateInfo(0);
    if (!((AnimatorStateInfo) ref animatorStateInfo).IsName("wait"))
      return;
    this.AnimAction();
  }

  private void PickUpUnitInit(UnitCameraPattern cameraData)
  {
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null))
    {
      this.ModelCamera.targetTexture = new RenderTexture(512, 512, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
      this.ModelCamera.clearFlags = (CameraClearFlags) 2;
      this.ModelCamera.backgroundColor = Color.clear;
      UITexture component = ((Component) this).gameObject.GetComponent<UITexture>();
      ((UIWidget) component).mainTexture = (Texture) this.ModelCamera.targetTexture;
      ((UIWidget) component).SetDimensions((int) (float) ((Texture) this.ModelCamera.targetTexture).width, (int) (float) ((Texture) this.ModelCamera.targetTexture).height);
      component.uvRect = new Rect(0.25f, 0.0f, 1f, 1f);
    }
    this.isPlay = false;
    this.ModelTrans = this.model_creater_.BaseModel.transform;
    this.Model = this.model_creater_.BaseModel;
    ((Object) this.Model).name = ((Object) this.Model).name + (object) this.debug_number;
    this.Model.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f);
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = new Vector3(1f, 1f, 1f);
    this.ModelCamera.fieldOfView = 50f;
    this.ModelTrans.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localPosition = new Vector3(cameraData.unit_x, cameraData.unit_y, cameraData.unit_z);
    this.ModelTrans.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTarget.localPosition = new Vector3(cameraData.camera_x, cameraData.camera_y, cameraData.camera_z);
    this.ActionTarget.localPosition = new Vector3(0.0f, -1f, -99999f);
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
    if (Object.op_Inequality((Object) this.lightGameObject, (Object) null) || !this.lightOn)
      return;
    this.lightGameObject = new GameObject("Directional Light");
    this.lightGameObject.AddComponent<Light>();
    this.lightGameObject.GetComponent<Light>().type = (LightType) 1;
    this.lightGameObject.GetComponent<Light>().color = Color.white;
    this.lightGameObject.GetComponent<Light>().intensity = 0.1f;
    this.lightGameObject.transform.position = new Vector3(0.0f, 1f, 0.0f);
    this.lightGameObject.transform.localEulerAngles = Consts.GetInstance().UI3DMODEL_DIRECTIONAL_LIGHT_ROUTATE;
    this.lightGameObject.transform.parent = ((Component) this).transform;
    this.lightGameObject.layer = ((Component) this.ModelTarget).gameObject.layer;
  }

  private void UnitEditInit(UnitCameraPattern cameraData)
  {
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null))
    {
      this.ModelCamera.targetTexture = new RenderTexture(512, 512, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
      this.ModelCamera.clearFlags = (CameraClearFlags) 2;
      this.ModelCamera.backgroundColor = Color.clear;
      ((UIWidget) ((Component) this).gameObject.GetComponent<UITexture>()).mainTexture = (Texture) this.ModelCamera.targetTexture;
    }
    this.isPlay = false;
    this.ModelTrans = this.model_creater_.BaseModel.transform;
    this.Model = this.model_creater_.BaseModel;
    ((Object) this.Model).name = ((Object) this.Model).name + (object) this.debug_number;
    this.SetScale = 1f;
    ((UIWidget) this.ui_texture).SetDimensions(360, 360);
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
    Object.Destroy((Object) ((Component) this).GetComponent<UIPanel>());
    Object.Destroy((Object) ((Component) this).GetComponent<UIButton>());
    this.widget.depth = 14;
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTrans.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTarget.localPosition = new Vector3(0.0f, -1.8f, 5f);
    this.ActionTarget.localPosition = new Vector3(0.0f, 0.0f, 1f);
    if (Object.op_Inequality((Object) this.lightGameObject, (Object) null) || !this.lightOn)
      return;
    this.lightGameObject = new GameObject("Directional Light");
    this.lightGameObject.AddComponent<Light>();
    this.lightGameObject.GetComponent<Light>().type = (LightType) 1;
    this.lightGameObject.GetComponent<Light>().color = Color.white;
    this.lightGameObject.GetComponent<Light>().intensity = 0.1f;
    this.lightGameObject.transform.position = new Vector3(0.0f, 1f, 0.0f);
    this.lightGameObject.transform.localEulerAngles = Consts.GetInstance().UI3DMODEL_DIRECTIONAL_LIGHT_ROUTATE;
    this.lightGameObject.transform.parent = ((Component) this).transform;
    this.lightGameObject.layer = ((Component) this.ModelTarget).gameObject.layer;
  }

  private void JobChangeInit(UnitCameraPattern cameraData)
  {
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null))
    {
      this.ModelCamera.targetTexture = new RenderTexture(512, 512, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
      this.ModelCamera.clearFlags = (CameraClearFlags) 2;
      this.ModelCamera.backgroundColor = Color.clear;
      ((UIWidget) ((Component) this).gameObject.GetComponent<UITexture>()).mainTexture = (Texture) this.ModelCamera.targetTexture;
    }
    this.isPlay = false;
    this.ModelTrans = this.model_creater_.BaseModel.transform;
    this.Model = this.model_creater_.BaseModel;
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = Vector3.one;
    this.ModelTrans.localScale = Vector3.one;
    this.ModelTrans.localPosition = cameraData != null ? new Vector3(cameraData.unit_x, cameraData.unit_y, cameraData.unit_z) : Vector3.zero;
    this.ModelTrans.localEulerAngles = Vector3.zero;
    this.ModelTarget.localPosition = cameraData != null ? new Vector3(cameraData.camera_x, cameraData.camera_y, cameraData.camera_z) : new Vector3(0.0f, -1.8f, 5f);
    this.ActionTarget.localPosition = new Vector3(0.0f, 0.0f, 1f);
  }

  private void Init(UnitCameraPattern cameraData)
  {
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null))
    {
      this.ModelCamera.targetTexture = new RenderTexture(316, 376, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
      this.ModelCamera.clearFlags = (CameraClearFlags) 2;
      this.ModelCamera.backgroundColor = Color.clear;
      ((UIWidget) ((Component) this).gameObject.GetComponent<UITexture>()).mainTexture = (Texture) this.ModelCamera.targetTexture;
    }
    this.isPlay = false;
    this.ModelTrans = this.model_creater_.BaseModel.transform;
    this.Model = this.model_creater_.BaseModel;
    ((UIWidget) this.ui_texture).SetDimensions(158, 188);
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = new Vector3(1f, 1f, 1f);
    this.ActionTarget.localPosition = new Vector3(0.0f, -1f, -99999f);
    this.ModelCamera.fieldOfView = cameraData.angle_of_view;
    this.ModelTrans.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localPosition = new Vector3(cameraData.unit_x, cameraData.unit_y, cameraData.unit_z);
    this.ModelTrans.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTarget.localPosition = new Vector3(cameraData.camera_x, cameraData.camera_y, cameraData.camera_z);
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
    if (Object.op_Inequality((Object) this.lightGameObject, (Object) null) || !this.lightOn)
      return;
    this.lightGameObject = new GameObject("Directional Light");
    this.lightGameObject.AddComponent<Light>();
    this.lightGameObject.GetComponent<Light>().type = (LightType) 1;
    this.lightGameObject.GetComponent<Light>().color = Color.white;
    this.lightGameObject.GetComponent<Light>().intensity = 0.1f;
    this.lightGameObject.transform.position = new Vector3(0.0f, 1f, 0.0f);
    this.lightGameObject.transform.localEulerAngles = Consts.GetInstance().UI3DMODEL_DIRECTIONAL_LIGHT_ROUTATE;
    this.lightGameObject.transform.parent = ((Component) this).transform;
    this.lightGameObject.layer = ((Component) this.ModelTarget).gameObject.layer;
  }

  private void HistoryInit(UnitCameraPattern cameraData)
  {
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null))
    {
      this.ModelCamera.targetTexture = new RenderTexture(316, 396, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
      this.ModelCamera.farClipPlane = 30f;
      ((UIWidget) ((Component) this).gameObject.GetComponent<UITexture>()).mainTexture = (Texture) this.ModelCamera.targetTexture;
    }
    this.isPlay = false;
    this.SetScale = 0.001760563f / ((Component) this.root).transform.localScale.x;
    this.ModelTrans = this.model_creater_.BaseModel.transform;
    this.Model = this.model_creater_.BaseModel;
    ((UIWidget) this.ui_texture).SetDimensions(158, 198);
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = new Vector3(1f, 1f, 1f);
    this.ActionTarget.localPosition = new Vector3(0.0f, -1f, -99999f);
    this.ModelCamera.fieldOfView = cameraData.angle_of_view;
    this.ModelTrans.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localPosition = new Vector3(cameraData.unit_x, cameraData.unit_y, cameraData.unit_z);
    this.ModelTrans.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTarget.localPosition = new Vector3(cameraData.camera_x, cameraData.camera_y, cameraData.camera_z);
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
    if (Object.op_Inequality((Object) this.lightGameObject, (Object) null) || !this.lightOn)
      return;
    this.lightGameObject = new GameObject("Directional Light");
    this.lightGameObject.AddComponent<Light>();
    this.lightGameObject.GetComponent<Light>().type = (LightType) 1;
    this.lightGameObject.GetComponent<Light>().color = Color.white;
    this.lightGameObject.GetComponent<Light>().intensity = 0.1f;
    this.lightGameObject.transform.position = new Vector3(0.0f, 1f, 0.0f);
    this.lightGameObject.transform.localEulerAngles = Consts.GetInstance().UI3DMODEL_DIRECTIONAL_LIGHT_ROUTATE;
    this.lightGameObject.transform.parent = ((Component) this).transform;
    this.lightGameObject.layer = ((Component) this.ModelTarget).gameObject.layer;
  }

  private void GuildInit(UnitCameraPattern cameraData, float fov, int w, int h, Material mat)
  {
    this.isPlay = false;
    if (Object.op_Equality((Object) this.ModelCamera.targetTexture, (Object) null) || ((Texture) this.ModelCamera.targetTexture).width != w || ((Texture) this.ModelCamera.targetTexture).height != h)
    {
      this.ModelCamera.targetTexture = new RenderTexture(w * 2, h * 2, 16);
      this.ModelCamera.targetTexture.antiAliasing = 1;
      ((Texture) this.ModelCamera.targetTexture).wrapMode = (TextureWrapMode) 1;
      ((Texture) this.ModelCamera.targetTexture).filterMode = (FilterMode) 1;
      this.ModelCamera.targetTexture.enableRandomWrite = false;
    }
    this.ModelCamera.fieldOfView = fov;
    this.ModelCamera.clearFlags = (CameraClearFlags) 2;
    this.ModelCamera.backgroundColor = Color.clear;
    ((Component) this.ModelCamera).transform.parent = (Transform) null;
    ((Component) this.ModelCamera).transform.localScale = new Vector3(1f, 1f, 1f);
    ((UIWidget) this.uiTexture).material = new Material(mat);
    ((UIWidget) this.uiTexture).material.mainTexture = this.GetRenderTexture();
    ((UIWidget) this.uiTexture).SetDimensions(w, h);
    this.ActionTarget.localPosition = new Vector3(0.0f, -1f, -99999f);
    this.Model = this.model_creater_.BaseModel;
    this.ModelTrans = this.Model.transform;
    this.ModelTrans.localScale = new Vector3(1f, 1f, 1f);
    this.ModelTrans.localPosition = new Vector3(cameraData.unit_x, cameraData.unit_y, cameraData.unit_z);
    this.ModelTrans.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    this.ModelTarget.localPosition = new Vector3(cameraData.camera_x, cameraData.camera_y, cameraData.camera_z);
    this.LayerChange(((Component) this.ModelTarget).gameObject.layer, this.ModelTrans);
  }

  private void Update()
  {
    if (this.unit_edit || Object.op_Equality((Object) this.Model, (Object) null) || Object.op_Equality((Object) this.model_creater_.UnitAnimator, (Object) null))
      return;
    if (!this.isPlay && !this.isNotLotate)
    {
      if (this.isGear)
      {
        this.ModelTarget.Rotate(Vector3.op_Multiply(new Vector3(0.0f, 30f, 0.0f), Time.deltaTime));
      }
      else
      {
        AnimatorStateInfo animatorStateInfo = this.model_creater_.UnitAnimator.GetCurrentAnimatorStateInfo(0);
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName("wait") || this.pick_up)
          this.ModelTarget.Rotate(Vector3.op_Multiply(new Vector3(0.0f, 30f, 0.0f), Time.deltaTime));
      }
    }
    if (this.pick_up)
      return;
    AnimatorStateInfo animatorStateInfo1 = this.model_creater_.UnitAnimator.GetCurrentAnimatorStateInfo(0);
    if (((AnimatorStateInfo) ref animatorStateInfo1).IsName("wait"))
      this.wait_counter = 0.0f;
    else
      this.wait_counter += Time.deltaTime;
    if ((double) this.wait_counter <= 3.0 || this.isNotLotate)
      return;
    this.model_creater_.UnitAnimator.SetTrigger("to_wait");
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.SetTrigger("to_wait");
  }

  private void LayerChange(int layer, Transform to)
  {
    ((Component) to).gameObject.layer = layer;
    foreach (Transform to1 in to)
      this.LayerChange(layer, to1);
  }

  private Future<GameObject> LoadModel(int id)
  {
    Future<GameObject> future = !this.isGear ? MasterData.UnitUnit[id].LoadModelDuel() : MasterData.GearGear[id].LoadModel();
    if (future == null)
    {
      Debug.LogWarning((object) "error kari model");
      MasterData.UnitUnit[100111].LoadModelDuel();
    }
    return future;
  }

  private void AnimAction()
  {
    this.isPlay = true;
    string str = NC.RandomChoice<string>(this.PlayAnimStateList);
    this.model_creater_.UnitAnimator.Play(str, 0);
    this.model_creater_.UnitAnimator.SetBool("to_wait", false);
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.Play(str, 0);
    this.model_creater_.VehicleAnimator.SetBool("to_wait", false);
  }

  public IEnumerator TryPlayAction(
    string[] statePatterns,
    System.Action<bool> eventStartEnd,
    float intervalToWait = 0.0f)
  {
    if (!Object.op_Equality((Object) this.model_creater_.UnitAnimator, (Object) null) && statePatterns != null && statePatterns.Length != 0)
    {
      this.isPlay = true;
      if (this.dicState == null)
        this.dicState = new Dictionary<string, string>();
      int index = 0;
      bool isNew = true;
      string state;
      string str;
      AnimatorStateInfo animatorStateInfo;
      do
      {
        if (this.dicState.TryGetValue(statePatterns[index], out state))
          isNew = false;
        else
          state = statePatterns[index];
        if (isNew)
          this.model_creater_.UnitAnimator.Play(state, 0);
        else
          this.model_creater_.UnitAnimator.Play(state, 0, 0.0f);
        this.model_creater_.UnitAnimator.SetBool("to_wait", false);
        if (isNew)
        {
          yield return (object) null;
          str = this.model_creater_.UnitAnimator.GetLayerName(0) + "." + state;
          animatorStateInfo = this.model_creater_.UnitAnimator.GetCurrentAnimatorStateInfo(0);
        }
        else
          break;
      }
      while (!((AnimatorStateInfo) ref animatorStateInfo).IsName(str) && ++index < statePatterns.Length);
      eventStartEnd(true);
      if (Object.op_Inequality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      {
        this.model_creater_.VehicleAnimator.Play(state, 0, 0.0f);
        this.model_creater_.VehicleAnimator.SetBool("to_wait", false);
      }
      if (isNew)
      {
        for (int index1 = 0; index1 < statePatterns.Length; ++index1)
          this.dicState[statePatterns[index1]] = state;
      }
      yield return (object) new WaitForAnimation(this.model_creater_.UnitAnimator, timeout: 30f, waitframeHashset: isNew ? 1 : 2);
      eventStartEnd(false);
      yield return (object) null;
      if ((double) intervalToWait > 0.0)
        yield return (object) new WaitForSeconds(intervalToWait);
      this.resetWaitAction();
      yield return (object) null;
      this.isPlay = false;
    }
  }

  public void ResetWaitAction()
  {
    this.resetWaitAction();
    this.isPlay = false;
  }

  private void resetWaitAction()
  {
    this.model_creater_.UnitAnimator.Play("wait", 0);
    if (!Object.op_Inequality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.Play("wait", 0);
  }

  public IEnumerator PlayAction(
    RuntimeAnimatorController animatorController,
    string animatorName,
    string state,
    float waitNext,
    Func<IEnumerator> playWait)
  {
    if (!Object.op_Equality((Object) this.model_creater_.UnitAnimator, (Object) null))
    {
      this.isPlay = true;
      RuntimeAnimatorController animatorController1 = this.model_creater_.UnitAnimator.runtimeAnimatorController;
      this.model_creater_.UnitAnimator.runtimeAnimatorController = animatorController;
      this.model_creater_.UnitAnimator.Play(animatorName, 0, 0.0f);
      if (Object.op_Inequality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      {
        this.model_creater_.VehicleAnimator.Play(state, 0, 0.0f);
        this.model_creater_.VehicleAnimator.SetBool("to_wait", false);
      }
      yield return (object) new WaitForAnimation(this.model_creater_.UnitAnimator, waitframeHashset: 2);
      yield return (object) playWait();
      if ((double) waitNext > 0.0)
        yield return (object) new WaitForSeconds(waitNext);
      this.model_creater_.ResetAnimator();
      if (Object.op_Inequality((Object) this.model_creater_.VehicleAnimator, (Object) null))
        this.model_creater_.VehicleAnimator.Play("wait", 0);
      yield return (object) null;
      this.isPlay = false;
    }
  }

  private IEnumerator CenterRotate()
  {
    float time_counter = 0.0f;
    do
    {
      ((Component) this.ModelTarget).transform.rotation = Quaternion.Slerp(((Component) this.ModelTarget).transform.rotation, Quaternion.LookRotation(Vector3.op_Subtraction(this.ActionTarget.position, this.ModelTrans.position)), 0.1f);
      yield return (object) true;
      time_counter += Time.deltaTime;
    }
    while ((double) time_counter <= 0.5);
    this.isPlay = false;
  }

  public void stopAnim()
  {
    this.model_creater_.UnitAnimator.speed = 0.0f;
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.speed = 0.0f;
  }

  public void AnimDeath()
  {
    this.model_creater_.UnitAnimator.SetTrigger("damage_to_death");
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.SetTrigger("damage_to_death");
  }

  public void AnimRaidMapDefeat()
  {
    this.model_creater_.UnitAnimator.SetTrigger("raid_map_defeat");
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.SetTrigger("raid_map_defeat");
  }

  public IEnumerator AnimRaidDamageReward()
  {
    this.model_creater_.UnitAnimator.SetTrigger("raid_constant_damage");
    yield return (object) new WaitForSeconds(1.5f);
  }

  public void setSpeedAnim(float value)
  {
    this.model_creater_.UnitAnimator.speed = value;
    if (Object.op_Equality((Object) this.model_creater_.VehicleAnimator, (Object) null))
      return;
    this.model_creater_.VehicleAnimator.speed = value;
  }

  public static int countEnabled { get; private set; }

  private void OnEnable() => ++UI3DModel.countEnabled;

  private void OnDisable() => --UI3DModel.countEnabled;

  private void OnDestroy() => this.DestroyModelCamera();

  public void DestroyModelCamera()
  {
    if (!Object.op_Implicit((Object) this.ModelCamera))
      return;
    Object.Destroy((Object) ((Component) this.ModelCamera).gameObject);
  }
}

// Decompiled with JetBrains decompiler
// Type: Ryusei_100111_ef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable disable
public class Ryusei_100111_ef : MonoBehaviour
{
  private GameObject efMasaCook;
  private GameObject effectPrefab;
  private Transform Root3d;
  private Transform bip;
  public Animator mAnim;
  public Vector3 dustPosition;
  public Vector3 dustRotate;
  public Vector3 position703;
  public Vector3 rotate703;
  public Vector3 position704;
  public Vector3 rotate704;
  public Vector3 position705;
  public Vector3 rotate705;
  public Vector3 position706;
  public Vector3 rotate706;
  public Vector3 position707;
  public Vector3 rotate707;
  public Vector3 position708;
  public Vector3 rotate708;
  public Vector3 position709;
  public Vector3 rotate709;
  public Vector3 position710;
  public Vector3 rotate710;
  public Vector3 position711;
  public Vector3 rotate711;
  public Vector3 position712;
  public Vector3 rotate712;
  public Vector3 position713;
  public Vector3 rotate713;
  public Vector3 position714;
  public Vector3 rotate714;
  public Vector3 position715;
  public Vector3 rotate715;
  public Vector3 position716;
  public Vector3 rotate716;
  public Vector3 position717;
  public Vector3 rotate717;
  public Vector3 position718;
  public Vector3 rotate718;
  public Vector3 position719;
  public Vector3 rotate719;
  public Vector3 position720;
  public Vector3 rotate720;
  public Vector3 position721;
  public Vector3 rotate721;
  public Vector3 position722;
  public Vector3 rotate722;
  public Vector3 position723;
  public Vector3 rotate723;
  public Vector3 position724;
  public Vector3 rotate724;
  public Vector3 position725;
  public Vector3 rotate725;
  public Vector3 position726;
  public Vector3 rotate726;
  private Vector3 position1000;
  private Vector3 rotate1000;
  private Vector3 position1001;
  private Vector3 rotate1001;
  private Vector3 position1002;
  private Vector3 rotate1002;
  private Vector3 position1003;
  private Vector3 rotate1003;
  private Vector3 position1004;
  private Vector3 rotate1004;
  private Vector3 position1005;
  private Vector3 rotate1005;
  private Vector3 position1006;
  private Vector3 rotate1006;
  private Vector3 position1007;
  private Vector3 rotate1007;
  private Vector3 position1008;
  private Vector3 rotate1008;
  public Vector3 cookPosition;
  public Vector3 cookRotate;
  private List<GameObject> destroyList;
  private NGDuelUnit myUnit;
  private NGDuelManager manager;

  private void Awake()
  {
    this.Root3d = GameObject.Find("Duel3DRoot").transform;
    this.myUnit = ((Component) this).gameObject.GetComponent<NGDuelUnit>();
    if (Object.op_Inequality((Object) this.myUnit, (Object) null))
    {
      this.manager = this.myUnit.manager;
      this.bip = this.myUnit.mBipTransform;
    }
    if (this.destroyList != null)
      return;
    this.destroyList = new List<GameObject>();
  }

  private void Start()
  {
    this.position1000 = Vector3.zero;
    this.rotate1000 = Vector3.zero;
    this.position1001 = Vector3.zero;
    this.rotate1001 = Vector3.zero;
    this.position1002 = Vector3.zero;
    this.rotate1002 = Vector3.zero;
    this.position1003 = Vector3.zero;
    this.rotate1003 = Vector3.zero;
    this.position1004 = Vector3.zero;
    this.rotate1004 = Vector3.zero;
    this.position1005 = Vector3.zero;
    this.rotate1005 = Vector3.zero;
    this.position1006 = Vector3.zero;
    this.rotate1006 = Vector3.zero;
    this.position1007 = Vector3.zero;
    this.rotate1007 = Vector3.zero;
    this.position1008 = Vector3.zero;
    this.rotate1008 = Vector3.zero;
    if (this.destroyList != null)
      return;
    this.destroyList = new List<GameObject>();
  }

  public void onReplay() => SceneManager.LoadScene(0);

  private IEnumerator loadPrefab(string pname, Vector3 pos, Vector3 rot)
  {
    GameObject go = Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(pname, this.Root3d);
    if (Object.op_Equality((Object) go, (Object) null))
    {
      Future<GameObject> fgo = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("BattleEffects/duel/{0}", (object) pname));
      IEnumerator e = fgo.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      this.effectPrefab = fgo.Result;
      go = this.effectPrefab.Clone(this.Root3d);
      fgo = (Future<GameObject>) null;
    }
    switch (pname)
    {
      case "ef1004_attack_rune":
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        break;
      case "ef703_duel_holysheeld":
        go.transform.position = Vector3.zero;
        go.transform.localPosition = new Vector3(140f, 0.0f, 0.0f);
        go.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        break;
      default:
        if (Object.op_Inequality((Object) null, (Object) this.bip))
        {
          go.transform.localPosition = Vector3.op_Addition(this.bip.position, pos);
          go.transform.localEulerAngles = rot;
          break;
        }
        break;
    }
    if (pname == "ef705_duel_sword_hit_moon")
    {
      yield return (object) new WaitForSeconds(2f);
      Object.DestroyImmediate((Object) go);
    }
    else
      this.destroyList.Add(go);
  }

  private IEnumerator loadPrefab(string data_name)
  {
    Ryusei_100111_ef ryusei100111Ef = this;
    ClipEventEffectData clipEventEffectData = Singleton<NGDuelDataManager>.GetInstance().GetPreloadClipEventEffectData(data_name);
    if (Object.op_Equality((Object) clipEventEffectData, (Object) null))
    {
      string path = string.Format("BattleEffects/duel/{0}", (object) data_name);
      Future<ClipEventEffectData> fgo = Singleton<ResourceManager>.GetInstance().Load<ClipEventEffectData>(path);
      IEnumerator e = fgo.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      clipEventEffectData = fgo.Result;
      fgo = (Future<ClipEventEffectData>) null;
    }
    foreach (ClipEventEffectData.EffectData data in clipEventEffectData.dataList)
      ryusei100111Ef.StartCoroutine(ryusei100111Ef.loadPrefab(data.effect_name, data.parent, data.is_add_bip, data.is_local_postion, data.position, data.is_local_rotation, data.rotation));
  }

  private IEnumerator loadPrefab(
    string pname,
    string parent_name,
    bool isAddBip,
    bool isLocalPos,
    Vector3 pos,
    bool isLocalRot,
    Vector3 rot)
  {
    Ryusei_100111_ef ryusei100111Ef = this;
    Transform parent = (Transform) null;
    if (!string.IsNullOrEmpty(parent_name))
    {
      parent = ((Component) ryusei100111Ef).transform.GetChildInFind(parent_name);
      if (Object.op_Equality((Object) parent, (Object) null) && Object.op_Inequality((Object) ryusei100111Ef.Root3d, (Object) null))
        parent = ryusei100111Ef.Root3d.Find(parent_name);
    }
    if (Object.op_Equality((Object) parent, (Object) null))
      parent = ryusei100111Ef.Root3d;
    GameObject gameObject = Singleton<NGDuelDataManager>.GetInstance().GetPreloadDuelEffect(pname, parent);
    if (Object.op_Equality((Object) gameObject, (Object) null))
    {
      Future<GameObject> fgo = Singleton<ResourceManager>.GetInstance().Load<GameObject>(string.Format("BattleEffects/duel/{0}", (object) pname));
      IEnumerator e = fgo.Wait();
      while (e.MoveNext())
        yield return e.Current;
      e = (IEnumerator) null;
      ryusei100111Ef.effectPrefab = fgo.Result;
      gameObject = ryusei100111Ef.effectPrefab.Clone(parent);
      fgo = (Future<GameObject>) null;
    }
    Vector3 vector3 = pos;
    if (isAddBip)
      vector3 = Vector3.op_Addition(vector3, ryusei100111Ef.bip.position);
    if (isLocalPos)
      gameObject.transform.localPosition = vector3;
    else
      gameObject.transform.position = vector3;
    if (isLocalRot)
      gameObject.transform.localRotation = Quaternion.Euler(rot);
    else
      gameObject.transform.rotation = Quaternion.Euler(rot);
    ryusei100111Ef.destroyList.Add(gameObject);
  }

  public void removeEffects()
  {
    foreach (Object destroy in this.destroyList)
      Object.Destroy(destroy);
    this.destroyList.Clear();
  }

  public void play_ef703()
  {
    this.StartCoroutine(this.loadPrefab("ef703_duel_holysheeld", this.position703, this.rotate703));
  }

  public void play_ef704()
  {
    this.StartCoroutine(this.loadPrefab("ef704_duel_holysheeld", this.position704, this.rotate704));
  }

  public void play_ef705()
  {
    this.StartCoroutine(this.loadPrefab("ef705_duel_sword_hit_moon", this.position705, this.rotate705));
  }

  public void play_ef706()
  {
    this.StartCoroutine(this.loadPrefab("ef706_duel_sword_reservoir", this.position706, this.rotate706));
  }

  public void play_ef707()
  {
    this.StartCoroutine(this.loadPrefab("ef707_duel_sword_trail", this.position707, this.rotate707));
  }

  public void play_ef708()
  {
    this.StartCoroutine(this.loadPrefab("ef708_duel_sword_smoke", this.position708, this.rotate708));
  }

  public void play_ef709()
  {
    this.StartCoroutine(this.loadPrefab("ef709_duel_spear_attack_star_s", this.position709, this.rotate709));
  }

  public void play_ef710()
  {
    this.StartCoroutine(this.loadPrefab("ef710_duel_spear_attack_star_l", this.position710, this.rotate710));
  }

  public void play_ef711()
  {
    this.StartCoroutine(this.loadPrefab("ef711_duel_spear_hit_star", this.position711, this.rotate711));
  }

  public void play_ef712()
  {
    this.StartCoroutine(this.loadPrefab("ef712_duel_spear_trail_star", this.position712, this.rotate712));
  }

  public void play_ef713()
  {
    this.StartCoroutine(this.loadPrefab("ef713_duel_damage_hit", this.position713, this.rotate713));
  }

  public void play_ef714()
  {
    this.StartCoroutine(this.loadPrefab("ef714_duel_scatters_heal_sun", this.position714, this.rotate714));
  }

  public void play_ef715()
  {
    this.StartCoroutine(this.loadPrefab("ef715_duel_sword_hit_sun", this.position715, this.rotate715));
  }

  public void play_ef716()
  {
    this.StartCoroutine(this.loadPrefab("ef716_duel_sword_trail_sun", this.position716, this.rotate716));
  }

  public void play_ef717()
  {
    this.StartCoroutine(this.loadPrefab("ef717_duel_dorain_sun", this.position717, this.rotate717));
  }

  public void play_ef718()
  {
    this.StartCoroutine(this.loadPrefab("ef718_duel_heal_sun", this.position718, this.rotate718));
  }

  public void play_ef719()
  {
    this.StartCoroutine(this.loadPrefab("ef719_duel_blast", this.position719, this.rotate719));
  }

  public void play_ef720()
  {
    this.StartCoroutine(this.loadPrefab("ef720_duel_spear_attack_moon_l", this.position720, this.rotate720));
  }

  public void play_ef721()
  {
    this.StartCoroutine(this.loadPrefab("ef721_duel_spear_hit_moon", this.position721, this.rotate721));
  }

  public void play_ef722()
  {
    this.StartCoroutine(this.loadPrefab("ef722_duel_spear_shine_moon", this.position722, this.rotate722));
  }

  public void play_ef723()
  {
    this.StartCoroutine(this.loadPrefab("ef723_duel_spear_explosion_moon", this.position723, this.rotate723));
  }

  public void play_ef724()
  {
    this.StartCoroutine(this.loadPrefab("ef724_duel_spear_trail_moon", this.position724, this.rotate724));
  }

  public void play_ef725()
  {
    this.StartCoroutine(this.loadPrefab("ef725_duel_damage_hit_sword", this.position725, this.rotate725));
  }

  public void play_ef726()
  {
    this.StartCoroutine(this.loadPrefab("ef726_duel_chocolate_damage", this.position726, this.rotate726));
  }

  public void play_ef1000()
  {
    this.StartCoroutine(this.loadPrefab("ef1000_cook_masamune", this.position1000, this.rotate1000));
  }

  public void play_ef1001()
  {
    this.StartCoroutine(this.loadPrefab("ef1001_cat_mjollnir", this.position1001, this.rotate1001));
  }

  public void play_ef1002()
  {
    this.StartCoroutine(this.loadPrefab("ef1002_rose_parashu", this.position1002, this.rotate1002));
  }

  public void play_ef1003()
  {
    this.StartCoroutine(this.loadPrefab("ef1003_idol_labrusse", this.position1003, this.rotate1003));
  }

  public void play_ef1004()
  {
    this.StartCoroutine(this.loadPrefab("ef1004_attack_rune", this.position1004, this.rotate1004));
  }

  public void play_ef1005()
  {
    this.StartCoroutine(this.loadPrefab("ef1005_alone_leavatain", this.position1005, this.rotate1005));
  }

  public void play_ef1006()
  {
    this.StartCoroutine(this.loadPrefab("ef1006_denial_megido", this.position1006, this.rotate1006));
  }

  public void play_ef1007()
  {
    this.StartCoroutine(this.loadPrefab("ef1007_scream_longinus", this.position1007, this.rotate1007));
  }

  public void play_ef1008()
  {
    this.StartCoroutine(this.loadPrefab("ef1008_valentine_perun", this.position1008, this.rotate1008));
  }

  public void play_effect(string data_name) => this.StartCoroutine(this.loadPrefab(data_name));

  public void play_Cutin()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.myUnit))
      return;
    this.myUnit.PlaySkillCutin();
  }

  public void play_CombineCutin(string unitIds)
  {
    if (!Object.op_Inequality((Object) null, (Object) this.myUnit))
      return;
    List<int> intList = this.PerseCutinId(unitIds);
    if (intList.Count == 3)
      this.myUnit.PlayMultiSkillCutin(intList[0], intList[1], intList[2]);
    else if (intList.Count == 2)
    {
      this.myUnit.PlayMultiSkillCutin(intList[0], intList[1]);
    }
    else
    {
      Debug.LogError((object) "[play_CombineCutin] Perse Error");
      this.myUnit.PlaySkillCutin();
    }
  }

  private List<int> PerseCutinId(string unitIds)
  {
    List<int> intList = new List<int>();
    string str = unitIds;
    char[] chArray = new char[1]{ ',' };
    foreach (string s in str.Split(chArray))
    {
      int num = 0;
      ref int local = ref num;
      if (int.TryParse(s, out local))
        intList.Add(num);
      else
        Debug.LogError((object) "[PerseCutinId] unitId Parse Error");
    }
    return intList;
  }

  public void WeaponTrailOn()
  {
    clipEffectPlayer clipEffectPlayer = ((Component) this).gameObject.GetComponentInChildren<clipEffectPlayer>();
    if (Object.op_Equality((Object) null, (Object) clipEffectPlayer))
      clipEffectPlayer = ((Component) this).gameObject.AddComponent<clipEffectPlayer>();
    clipEffectPlayer.playEffect("weapon_trail_on");
  }

  public void WeaponTrailOff()
  {
    clipEffectPlayer clipEffectPlayer = ((Component) this).gameObject.GetComponentInChildren<clipEffectPlayer>();
    if (Object.op_Equality((Object) null, (Object) clipEffectPlayer))
      clipEffectPlayer = ((Component) this).gameObject.AddComponent<clipEffectPlayer>();
    clipEffectPlayer.playEffect("weapon_trail_off");
  }

  public void play_DamageNumber() => this.myUnit.Enemy.damaged(true);

  public void play_DamageNumber_BiAttack() => this.myUnit.Enemy.damaged();

  public void play_HealNumber()
  {
    this.StartCoroutine(this.myUnit.showHealNumber());
    clipEffectPlayer clipEffectPlayer = ((Component) this).gameObject.GetComponentInChildren<clipEffectPlayer>();
    if (Object.op_Equality((Object) null, (Object) clipEffectPlayer))
      clipEffectPlayer = ((Component) this).gameObject.AddComponent<clipEffectPlayer>();
    clipEffectPlayer.playEffect("ef718_duel_heal_sun:0");
  }

  public void play_FaceNormal()
  {
  }

  public void play_FacePain()
  {
  }

  public void play_FaceAngry()
  {
  }

  public void playAttackVoice()
  {
    NGSoundManager instance = Singleton<NGSoundManager>.GetInstance();
    if (!this.myUnit.isPlayVoice)
      return;
    instance.playVoiceByID(this.myUnit.UnitVoicePattern, 28);
  }

  public void SetDuelAmbientLight(string color)
  {
    if (!Object.op_Inequality((Object) null, (Object) this.myUnit))
      return;
    if (string.IsNullOrEmpty(color))
    {
      double num = 0.90196079015731812;
      RenderSettings.ambientLight = new Color((float) num, (float) num, (float) num);
    }
    else
    {
      string[] strArray = color.Split(':');
      if (strArray.Length == 0)
        return;
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (int.TryParse(strArray[0], out result1))
        num1 = (float) result1 / (float) byte.MaxValue;
      if (strArray.Length > 1 && int.TryParse(strArray[1], out result2))
        num2 = (float) result2 / (float) byte.MaxValue;
      if (strArray.Length > 2 && int.TryParse(strArray[2], out result3))
        num3 = (float) result3 / (float) byte.MaxValue;
      RenderSettings.ambientLight = new Color(num1, num2, num3);
    }
  }

  public void ResetDuelAmbientLight()
  {
    if (!Object.op_Inequality((Object) null, (Object) this.manager))
      return;
    this.manager.ResetDuelAmbient();
  }
}

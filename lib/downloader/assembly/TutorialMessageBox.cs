// Decompiled with JetBrains decompiler
// Type: TutorialMessageBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System.Collections;
using UnityEngine;

#nullable disable
public class TutorialMessageBox : MonoBehaviour
{
  [SerializeField]
  private UILabel label;
  [SerializeField]
  private GameObject arrow;
  [SerializeField]
  private GameObject[] characters;
  [SerializeField]
  private NGxFaceSprite[] face;
  [SerializeField]
  private GameObject ItemIconRoot;
  private GameObject UniqueIconsPrefab;
  private GameObject effectPrefab;

  public void Init(
    string message,
    int characterIndex,
    bool dispArrow,
    string faceName,
    string itemName,
    int keyId,
    int boxType)
  {
    this.label.SetTextLocalize(message);
    if (Object.op_Inequality((Object) this.arrow, (Object) null))
      this.arrow.SetActive(dispArrow);
    if (boxType == 0)
    {
      if (string.IsNullOrEmpty(faceName))
        faceName = "base";
      UISprite component = this.characters[0].GetComponent<UISprite>();
      component.spriteName = string.Format("slc_character{0}_{1}.png__GUI__tutorial__tutorial_prefab", (object) characterIndex, (object) faceName);
      ((UIWidget) component).MakePixelPerfect();
    }
    else
    {
      this.DispCharacter(characterIndex);
      this.StartCoroutine(this.ChangeItemIcon(itemName, keyId));
      this.StartCoroutine(this.FaceChange(faceName, characterIndex));
    }
  }

  public IEnumerator FaceChange(string faceName, int characterIndex)
  {
    if (this.face.Length != 0)
    {
      if (string.IsNullOrEmpty(faceName))
        faceName = "normal";
      NGxFaceSprite ngxFaceSprite = (NGxFaceSprite) null;
      try
      {
        ngxFaceSprite = this.face[characterIndex];
      }
      catch
      {
        Debug.LogWarning((object) ("キャラID:" + (object) characterIndex + "のfaceがアタッチされていない"));
      }
      if (Object.op_Inequality((Object) ngxFaceSprite, (Object) null))
      {
        IEnumerator i = ngxFaceSprite.ChangeFace(faceName);
        while (i.MoveNext())
          yield return i.Current;
        i = (IEnumerator) null;
      }
    }
  }

  private IEnumerator ChangeItemIcon(string itemName, int keyId = 1)
  {
    if (!Object.op_Equality((Object) this.ItemIconRoot, (Object) null))
    {
      if (string.IsNullOrEmpty(itemName))
      {
        this.ItemIconRoot.gameObject.SetActive(false);
      }
      else
      {
        this.ItemIconRoot.gameObject.SetActive(true);
        IEnumerator e;
        if (Object.op_Equality((Object) this.effectPrefab, (Object) null))
        {
          Future<GameObject> effectPrefabF = Res.Prefabs.Tutorial.dir_effect.Load<GameObject>();
          e = effectPrefabF.Wait();
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.effectPrefab = effectPrefabF.Result;
          effectPrefabF = (Future<GameObject>) null;
        }
        foreach (Component component in this.ItemIconRoot.transform)
          Object.Destroy((Object) component.gameObject);
        CreateIconObject target = this.ItemIconRoot.GetOrAddComponent<CreateIconObject>();
        GameObject icon = target.GetIcon();
        MasterDataTable.CommonRewardType? reward = this.GetReward(itemName);
        if (reward.HasValue)
        {
          if (reward.Value != MasterDataTable.CommonRewardType.quest_key)
            keyId = 0;
          e = target.CreateThumbnail(reward.Value, keyId);
          while (e.MoveNext())
            yield return e.Current;
          e = (IEnumerator) null;
          this.effectPrefab.Clone(target.GetIcon().transform);
        }
        else if (Object.op_Inequality((Object) icon, (Object) null))
          icon.SetActive(false);
      }
    }
  }

  private void DispCharacter(int characterIndex)
  {
    for (int index = 0; index < this.characters.Length; ++index)
      this.characters[index].SetActive(index == characterIndex);
  }

  private MasterDataTable.CommonRewardType? GetReward(string itemName)
  {
    switch (itemName)
    {
      case "battlemedal":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.battle_medal);
      case "gachaticket":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.gacha_ticket);
      case "key":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.quest_key);
      case "kiseki":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.coin);
      case "medal":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.medal);
      case "none":
        return new MasterDataTable.CommonRewardType?();
      case "point":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.friend_point);
      case "zeny":
        return new MasterDataTable.CommonRewardType?(MasterDataTable.CommonRewardType.money);
      default:
        Debug.LogWarning((object) ("ChangeItemIcon アイテム名：" + itemName + "が見つかりませんでした。"));
        return new MasterDataTable.CommonRewardType?();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: PopupClassChange
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using MasterDataTable;
using UnityEngine;

#nullable disable
public class PopupClassChange : MonoBehaviour
{
  [SerializeField]
  private UISprite sprite;
  private static readonly string[] spriteName = new string[12]
  {
    "text_ClassDown.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassStayed.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassStayedTop.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassUp.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassTitle.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassTitleTop.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassDownZone.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassStayedZone.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassStayedTop.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassUpZone.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassTitleZone.png__GUI__versus_results_common__versus_results_common_prefab",
    "text_ClassTitleTop.png__GUI__versus_results_common__versus_results_common_prefab"
  };
  private static readonly string spriteNameIsLowest = "text_ClassStayedBottom.png__GUI__versus_results_common__versus_results_common_prefab";
  private static readonly string spriteNameIsLowestZone = "text_ClassStayedZoneBottom.png__GUI__versus_results_common__versus_results_common_prefab";

  public void ChangeSprite(PvpClassKind.Condition c, bool isLowest = false)
  {
    this.sprite.spriteName = !isLowest || c != PvpClassKind.Condition.STAY_ZONE ? (!isLowest || c != PvpClassKind.Condition.STAY ? PopupClassChange.spriteName[(int) c] : PopupClassChange.spriteNameIsLowest) : PopupClassChange.spriteNameIsLowestZone;
    ((UIWidget) this.sprite).MakePixelPerfect();
  }
}

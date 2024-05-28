// Decompiled with JetBrains decompiler
// Type: UnitPositionExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class UnitPositionExtension
{
  public static void startMoveRoute(
    this BL.UnitPosition up,
    List<BL.Panel> route,
    float speed,
    BE env,
    Action action = null)
  {
    BL.Unit unit = up.unit;
    if (!unit.isView)
      return;
    BE.UnitResource unitResource = env.unitResource[up.unit];
    if (!unit.isEnable || Object.op_Equality((Object) unitResource.gameObject, (Object) null) || unit.hp <= 0)
      return;
    unitResource.unitParts_.startMoveRoute(route, speed);
    if (action == null)
      return;
    action();
  }

  public static void cancelMove(this BL.UnitPosition up, BE env, Action endMoving = null)
  {
    BL.Unit unit = up.unit;
    if (!unit.isView)
      return;
    BE.UnitResource unitResource = env.unitResource[up.unit];
    if (!unit.isEnable || Object.op_Equality((Object) unitResource.gameObject, (Object) null) || unit.hp <= 0)
      return;
    unitResource.unitParts_.cancelMove(Singleton<NGBattleManager>.GetInstance().defaultUnitSpeed * 3f, endMoving);
  }

  public static bool isMoving(this BL.UnitPosition up, BE env)
  {
    BL.Unit unit = up.unit;
    if (!unit.isView)
      return false;
    BE.UnitResource unitResource = env.unitResource[up.unit];
    return unit.isEnable && !Object.op_Equality((Object) unitResource.gameObject, (Object) null) && unitResource.unitParts_.isMoving;
  }

  public static float calcMoveTime(this BL.UnitPosition up, int routeCount, float speed)
  {
    if (routeCount < 2)
      return 0.0f;
    float num1 = BattleUnitParts.CalcMoveSpeed(routeCount, speed);
    if ((double) num1 <= 0.0)
      return 0.0f;
    if ((double) Time.timeScale <= 0.0)
      return (float) (routeCount - 1) * 2f / num1;
    float num2 = Time.timeScale / (float) Application.targetFrameRate;
    return (Mathf.Ceil(2f / (num1 * num2)) + 1f) * (float) (routeCount - 1) * num2;
  }
}

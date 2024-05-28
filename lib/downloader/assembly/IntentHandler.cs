// Decompiled with JetBrains decompiler
// Type: IntentHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84692B6C-DF14-44E0-9A18-AFF35C631E79
// Assembly location: B:\workspace\PotK Unit Database\Assembly Decompile\Assembly-CSharp.dll

#nullable disable
public class IntentHandler
{
  private static IntentHandler instance = new IntentHandler();

  public static IntentHandler GetInstance() => IntentHandler.instance;

  public void ClearIntentHandlers()
  {
  }

  public void AddNoopIntentHandler()
  {
  }

  public void AddUrlIntentHandler()
  {
  }

  public void AddCustomIntentHandler(string gameObjectName, string methodName)
  {
  }
}

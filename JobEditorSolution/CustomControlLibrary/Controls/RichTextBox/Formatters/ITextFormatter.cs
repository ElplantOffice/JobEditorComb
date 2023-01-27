// Decompiled with JetBrains decompiler
// Type: CustomControlLibrary.ITextFormatter
// Assembly: CustomControlLibrary, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66BA39D6-2353-4631-A8B8-54C2A119AFE3
// Assembly location: D:\deflate\costura.CustomControlLibrary.dll

using System.Windows.Documents;

namespace CustomControlLibrary
{
  public interface ITextFormatter
  {
    string GetText(FlowDocument document);

    void SetText(FlowDocument document, string text);
  }
}

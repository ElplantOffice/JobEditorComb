
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Models
{
  [DataContract(Name = "UiLocalisationItem", Namespace = "")]
  public class UiLocalisationItem : ILocalisationItem, ILocalisationLanguage
  {
    [DataMember(IsRequired = true, Name = "TextId", Order = 0)]
    public string TextId { get; set; }

    [DataMember(IsRequired = true, Name = "LocalisationId", Order = 1)]
    public int LocalisationId { get; set; }

    [IgnoreDataMember]
    public string LocalisationIdName
    {
      get
      {
        string str = "";
        try
        {
          str = new CultureInfo(this.LocalisationId).Name;
        }
        catch (ArgumentOutOfRangeException ex)
        {
        }
        catch (CultureNotFoundException ex)
        {
        }
        return str;
      }
      set
      {
        this.LocalisationId = -1;
        try
        {
          this.LocalisationId = new CultureInfo(value).LCID;
        }
        catch (ArgumentOutOfRangeException ex)
        {
        }
        catch (CultureNotFoundException ex)
        {
        }
      }
    }

    [DataMember(IsRequired = false, Name = "LanguageName", Order = 2)]
    public string LocalisationIdEnglishName
    {
      get
      {
        string str = "";
        try
        {
          str = new CultureInfo(this.LocalisationId).EnglishName;
        }
        catch (ArgumentOutOfRangeException ex)
        {
        }
        catch (CultureNotFoundException ex)
        {
        }
        return str;
      }
      private set
      {
      }
    }

    [DataMember(IsRequired = false, Name = "Text", Order = 3)]
    public string Text { get; set; }

    [DataMember(IsRequired = false, Name = "Used", Order = 4)]
    public bool Used { get; set; }

    [DataMember(IsRequired = false, Name = "New", Order = 5)]
    public bool New { get; set; }

    public UiLocalisationItem()
    {
      this.LocalisationId = 0;
    }

    public UiLocalisationItem(UiLocalisationItem source)
    {
      this.LocalisationId = 0;
      if (source == null)
        return;
      this.LocalisationId = source.LocalisationId;
      this.TextId = source.TextId;
      this.Text = source.Text;
      this.Used = source.Used;
      this.New = source.New;
    }

    public UiLocalisationItem(ILocalisationItem source)
    {
      this.LocalisationId = 0;
      if (source == null)
        return;
      this.LocalisationId = source.LocalisationId;
      this.TextId = source.TextId;
      this.Text = source.Text;
      this.Used = false;
      this.New = true;
    }

    public UiLocalisationItem(int localisationId)
    {
      this.LocalisationId = localisationId;
    }

    public UiLocalisationItem(int localisationId, string textId, string text)
    {
      this.LocalisationId = localisationId;
      this.TextId = textId;
      this.Text = text;
      this.Used = false;
      this.New = true;
    }
  }
}

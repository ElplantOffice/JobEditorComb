
using Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Models
{
  [DataContract(Name = "UiLocalisation", Namespace = "")]
  public class UiLocalisation : ILocalisation
  {
    private object lockitemdictionary = new object();
    private object lockitemlist = new object();
    private bool changed;

    [IgnoreDataMember]
    private Dictionary<int, Dictionary<string, UiLocalisationItem>> ItemDictionary { get; set; }

    [DataMember]
    private List<UiLocalisationItem> ItemList { get; set; }

    [IgnoreDataMember]
    public bool IsChanged
    {
      get
      {
        return this.changed;
      }
    }

    [IgnoreDataMember]
    public UiLocalisationSettings Settings { get; set; }

    public UiLocalisation()
    {
      this.ItemDictionary = new Dictionary<int, Dictionary<string, UiLocalisationItem>>();
      this.ItemList = new List<UiLocalisationItem>();
    }

    public void Clear()
    {
      lock (this.lockitemdictionary)
        this.ItemDictionary.Clear();
    }

    public bool AddItem(ILocalisationItem item)
    {
      return this.AddItem(new UiLocalisationItem(item));
    }

    public bool AddItem(int localisationId, string textId, string text)
    {
      return this.AddItem(new UiLocalisationItem(localisationId, textId, text));
    }

    public bool Translate(int localisationId, string textId, out string text, string defaultText)
    {
      if (localisationId == 0)
      {
        text = textId;
        return true;
      }
      if (string.IsNullOrEmpty(textId))
      {
        text = "";
        return true;
      }
      lock (this.lockitemdictionary)
      {
        if (!this.ItemDictionary.ContainsKey(localisationId))
        {
          this.ItemDictionary.Add(localisationId, new Dictionary<string, UiLocalisationItem>());
          this.changed = true;
        }
        Dictionary<string, UiLocalisationItem> dictionary = this.ItemDictionary[localisationId];
        if (!dictionary.ContainsKey(textId))
        {
          string text1 = defaultText;
          if (defaultText == null)
            text1 = textId;
          dictionary.Add(textId, new UiLocalisationItem(localisationId, textId, text1));
          this.changed = true;
        }
        UiLocalisationItem localisationItem = dictionary[textId];
        if (!localisationItem.Used)
          this.changed = true;
        localisationItem.Used = true;
        text = localisationItem.Text;
      }
      return true;
    }

    public bool Translate(int localisationId, ref string text, string defaultText = null)
    {
      string textId = text;
      string defaultText1 = defaultText;
      if (defaultText == null)
        defaultText1 = textId;
      return this.Translate(localisationId, textId, out text, defaultText1);
    }

    public bool Translate(ITranslatable translatableObject, string defaultText = null)
    {
      if (translatableObject == null || translatableObject.Localisation == 0 || translatableObject.Localisation == this.Settings.LocalisationId)
        return false;
      string textId = translatableObject.TextId;
      if (!this.Translate(this.Settings.LocalisationId, ref textId, defaultText))
        return false;
      translatableObject.ContentText = textId;
      translatableObject.Localisation = this.Settings.LocalisationId;
      return true;
    }

    public bool Translate(IEventMessage Message)
    {
      if (Message.Value != null && Message.Value is ITranslatable)
        return this.Translate(Message.Value as ITranslatable, (string) null);
      return false;
    }

    public bool Translate(ILocalisationItem item, string defaultText = null)
    {
      if (item == null)
        return false;
      string defaultText1 = defaultText;
      if (defaultText == null)
        defaultText1 = item.TextId;
      string text = item.Text;
      int num = this.Translate(item.LocalisationId, item.TextId, out text, defaultText1) ? 1 : 0;
      item.Text = text;
      return num != 0;
    }

    public string Translate(string textId)
    {
      string text = textId;
      if (this.Translate(this.Settings.LocalisationId, textId, out text, textId))
        return text;
      return textId;
    }

    public bool Load(bool waituntildone = true)
    {
      if (this.Settings == null)
        return false;
      return this.LoadXmlData(this.Settings.LoadFilename, waituntildone, false);
    }

    public bool Save(bool waituntildone = true)
    {
      if (this.Settings == null)
        return false;
      return this.SaveXmlData(this.Settings.SaveFilename, waituntildone);
    }

    public List<ILocalisationLanguage> ListUsedLanguages
    {
      get
      {
        List<ILocalisationLanguage> localisationLanguageList = new List<ILocalisationLanguage>();
        lock (this.lockitemdictionary)
        {
          if (this.ItemDictionary != null)
          {
            foreach (int key in this.ItemDictionary.Keys)
            {
              ILocalisationLanguage localisationLanguage = (ILocalisationLanguage) new UiLocalisationItem(key);
              localisationLanguageList.Add(localisationLanguage);
            }
          }
        }
        return localisationLanguageList;
      }
    }

    public SortedList<string, ILocalisationLanguage> SortedListUsedLanguages
    {
      get
      {
        SortedList<string, ILocalisationLanguage> sortedList = new SortedList<string, ILocalisationLanguage>();
        lock (this.lockitemdictionary)
        {
          if (this.ItemDictionary != null)
          {
            foreach (int key in this.ItemDictionary.Keys)
            {
              ILocalisationLanguage localisationLanguage = (ILocalisationLanguage) new UiLocalisationItem(key);
              if (!sortedList.ContainsKey(localisationLanguage.LocalisationIdEnglishName))
                sortedList.Add(localisationLanguage.LocalisationIdEnglishName, localisationLanguage);
            }
          }
        }
        return sortedList;
      }
    }

    public static List<ILocalisationLanguage> ListValidLanguages
    {
      get
      {
        List<ILocalisationLanguage> localisationLanguageList = new List<ILocalisationLanguage>();
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
        {
          ILocalisationLanguage localisationLanguage = (ILocalisationLanguage) new UiLocalisationItem(culture.LCID);
          localisationLanguageList.Add(localisationLanguage);
        }
        return localisationLanguageList;
      }
    }

    public static SortedList<string, ILocalisationLanguage> SortedListValidLanguages
    {
      get
      {
        SortedList<string, ILocalisationLanguage> sortedList = new SortedList<string, ILocalisationLanguage>();
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
        {
          ILocalisationLanguage localisationLanguage = (ILocalisationLanguage) new UiLocalisationItem(culture.LCID);
          if (!sortedList.ContainsKey(localisationLanguage.LocalisationIdEnglishName))
            sortedList.Add(localisationLanguage.LocalisationIdEnglishName, localisationLanguage);
        }
        return sortedList;
      }
    }

    public bool LoadXmlData(string fileName, bool waituntildone = true, bool append = false)
    {
      Task<bool> task = Task.Run<bool>((Func<bool>) (() =>
      {
        lock (this.lockitemlist)
        {
          bool flag = false;
          this.ItemList.Clear();
          XmlTextReader xmlTextReader;
          try
          {
            xmlTextReader = new XmlTextReader(fileName);
          }
          catch (Exception ex)
          {
            Console.WriteLine("Could not open file!\nException: " + ex.ToString());
            return false;
          }
          try
          {
            this.ItemList = ((UiLocalisation) new DataContractSerializer(typeof (UiLocalisation)).ReadObject((XmlReader) xmlTextReader)).ItemList;
            flag = true;
          }
          catch (Exception ex)
          {
            Console.WriteLine("Could not load file!\nException: " + ex.ToString());
            this.ItemList.Clear();
          }
          xmlTextReader.Close();
          lock (this.lockitemdictionary)
          {
            this.LocalisationsListToDict(append);
            this.changed = false;
          }
          return flag;
        }
      }));
      bool flag1 = true;
      if (waituntildone)
        flag1 = task.Result;
      return flag1;
    }

    public bool SaveXmlData(string fileName, bool waituntildone = true)
    {
      Task<bool> task = Task.Run<bool>((Func<bool>) (() =>
      {
        lock (this.lockitemlist)
        {
          XmlTextWriter xmlTextWriter1;
          try
          {
            xmlTextWriter1 = new XmlTextWriter(fileName, Encoding.UTF8);
          }
          catch (Exception ex)
          {
            Console.WriteLine("Could not open file!\nException: " + ex.ToString());
            return false;
          }
          lock (this.lockitemdictionary)
          {
            this.LocalisationsDictToList();
            this.changed = false;
          }
          bool flag = false;
          try
          {
            DataContractSerializer contractSerializer = new DataContractSerializer(typeof (UiLocalisation));
            xmlTextWriter1.Formatting = Formatting.Indented;
            xmlTextWriter1.WriteStartDocument();
            XmlTextWriter xmlTextWriter2 = xmlTextWriter1;
            contractSerializer.WriteObject((XmlWriter) xmlTextWriter2, (object) this);
            flag = true;
          }
          catch (Exception ex)
          {
            Console.WriteLine("Could not save file!\nException: " + ex.ToString());
          }
          xmlTextWriter1.Close();
          return flag;
        }
      }));
      bool flag1 = true;
      if (waituntildone)
        flag1 = task.Result;
      return flag1;
    }

    private bool AddItem(UiLocalisationItem item)
    {
      bool flag = false;
      lock (this.lockitemdictionary)
      {
        if (!this.ItemDictionary.ContainsKey(item.LocalisationId))
          this.ItemDictionary.Add(item.LocalisationId, new Dictionary<string, UiLocalisationItem>());
        Dictionary<string, UiLocalisationItem> dictionary = this.ItemDictionary[item.LocalisationId];
        if (!dictionary.ContainsKey(item.TextId))
        {
          dictionary.Add(item.TextId, item);
          flag = true;
          this.changed = true;
        }
      }
      return flag;
    }

    private UiLocalisationItem GetItem(int localisationId, string textId)
    {
      UiLocalisationItem localisationItem = (UiLocalisationItem) null;
      lock (this.lockitemdictionary)
      {
        if (this.ItemDictionary.ContainsKey(localisationId))
        {
          if (this.ItemDictionary[localisationId].ContainsKey(textId))
            localisationItem = this.ItemDictionary[localisationId][textId];
        }
      }
      return localisationItem;
    }

    private void LocalisationsListToDict(bool append)
    {
      lock (this.lockitemlist)
      {
        lock (this.lockitemdictionary)
        {
          if (!append)
            this.ItemDictionary.Clear();
          foreach (UiLocalisationItem localisationItem in this.ItemList)
            this.AddItem(localisationItem);
        }
      }
    }

    private void LocalisationsDictToList()
    {
      lock (this.lockitemlist)
      {
        lock (this.lockitemdictionary)
        {
          this.ItemList.Clear();
          List<int> list = this.ItemDictionary.Keys.OrderBy<int, int>((Func<int, int>) (key => key != 1033 ? 2 : 1)).ThenBy<int, int>((Func<int, int>) (key => key)).ToList<int>();
          foreach (string textId in this.ItemDictionary.SelectMany<KeyValuePair<int, Dictionary<string, UiLocalisationItem>>, string>((Func<KeyValuePair<int, Dictionary<string, UiLocalisationItem>>, IEnumerable<string>>) (pair => pair.Value.Select<KeyValuePair<string, UiLocalisationItem>, string>((Func<KeyValuePair<string, UiLocalisationItem>, string>) (innerpair => innerpair.Value.TextId)))).OrderBy<string, string>((Func<string, string>) (innerpair => innerpair)).Distinct<string>().ToList<string>())
          {
            foreach (int localisationId1 in list)
            {
              UiLocalisationItem localisationItem = this.GetItem(localisationId1, textId);
              if (localisationItem == null)
              {
                int localisationId2 = localisationId1;
                string str = textId;
                localisationItem = new UiLocalisationItem(localisationId2, str, str);
              }
              this.ItemList.Add(localisationItem);
            }
          }
        }
      }
    }
  }
}

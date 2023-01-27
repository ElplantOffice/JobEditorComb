
using System.Collections;
using System.Collections.Generic;

namespace Patterns.EventAggregator
{
  public class ParentStringIterator : IEnumerable, IEnumerator
  {
    private int _enumeratorindex = -1;
    private readonly List<ParentStringIterator.SeparationItem> _separations;

    public ParentStringIterator(string parentsChild, string separatorString, bool includeSeparator = false, bool includeParentsChild = true, bool startWithRootParent = false)
      : this(parentsChild, new string[1]{ separatorString }, (includeSeparator ? 1 : 0) != 0, (includeParentsChild ? 1 : 0) != 0, (startWithRootParent ? 1 : 0) != 0)
    {
    }

    public ParentStringIterator(string parentsChild, string[] separatorStrings, bool includeSeparator = false, bool includeParentsChild = true, bool startWithRootParent = false)
    {
      this._separations = new List<ParentStringIterator.SeparationItem>();
      int num = 0;
      bool flag = false;
      while (!flag)
      {
        ParentStringIterator.SeparationItem separationItem = (ParentStringIterator.SeparationItem) null;
        ParentStringIterator.SeparationItem source = new ParentStringIterator.SeparationItem(-1, 0, (string) null);
        foreach (string separatorString in separatorStrings)
        {
          source.Length = separatorString.Length;
          source.Position = parentsChild.IndexOf(separatorString, num);
          if (source.Position != -1)
          {
            if (separationItem == null)
              separationItem = new ParentStringIterator.SeparationItem(source);
            else if (source.Position < separationItem.Position)
              separationItem = new ParentStringIterator.SeparationItem(source);
            else if (source.Position == separationItem.Position && source.Length > separationItem.Length)
              separationItem = new ParentStringIterator.SeparationItem(source);
          }
        }
        if (separationItem != null)
        {
          num = separationItem.Position + separationItem.Length;
          separationItem.Parent = !includeSeparator ? parentsChild.Substring(0, num - separationItem.Length) : parentsChild.Substring(0, num);
          if (startWithRootParent)
            this._separations.Add(separationItem);
          else
            this._separations.Insert(0, separationItem);
        }
        else
          flag = true;
      }
      if (!includeParentsChild)
        return;
      if (startWithRootParent)
        this._separations.Add(new ParentStringIterator.SeparationItem(-1, 0, parentsChild));
      else
        this._separations.Insert(0, new ParentStringIterator.SeparationItem(-1, 0, parentsChild));
    }

    public string Current
    {
      get
      {
        return this._separations[this._enumeratorindex].Parent;
      }
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) this.Current;
      }
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
      ++this._enumeratorindex;
      return this._enumeratorindex < this._separations.Count;
    }

    public void Reset()
    {
      this._enumeratorindex = -1;
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) this;
    }

    private class SeparationItem
    {
      public int Position { get; set; }

      public int Length { get; set; }

      public string Parent { get; set; }

      public SeparationItem(int position = -1, int length = 0, string parent = null)
      {
        this.Position = position;
        this.Length = length;
        this.Parent = parent;
      }

      public SeparationItem(ParentStringIterator.SeparationItem source)
      {
        this.Position = source.Position;
        this.Length = source.Length;
        this.Parent = source.Parent;
      }
    }
  }
}

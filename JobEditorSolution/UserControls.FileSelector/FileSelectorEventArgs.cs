
using System;

namespace UserControls.FileSelector
{
  public class FileSelectorEventArgs : EventArgs
  {
    public string FilePath { get; private set; }

    public string FileName { get; private set; }

    public FileSelectorEventArgs()
    {
    }

    public FileSelectorEventArgs(string filePath, string fileName)
    {
      this.FilePath = filePath;
      this.FileName = fileName;
    }
  }
}

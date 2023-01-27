
using System;
using System.IO;

namespace UserControls.FileSelector
{
  public class FileSelectorFolderInfo : IFileSelectorFolderInfo
  {
    private FileSelectorData fileSelectorData;
    private long folderIndex;
    private string folderName;
    private string folderPath;
    private DateTime folderDate;

    public string FolderInfoUid
    {
      get
      {
        return this.folderIndex.ToString();
      }
    }

    public string ViewFolderName
    {
      get
      {
        string str = this.folderName;
        if (this.fileSelectorData != null)
          str = this.fileSelectorData.ViewFolderName(this.FullFolderName);
        return str;
      }
    }

    public string ViewFolderPath
    {
      get
      {
        return this.folderPath;
      }
    }

    public string ViewFolderDate
    {
      get
      {
        return this.folderDate.ToString();
      }
    }

    public string FolderName
    {
      get
      {
        return this.folderName;
      }
    }

    public string FolderPath
    {
      get
      {
        return this.folderPath;
      }
    }

    public string FullFolderName
    {
      get
      {
        string path = this.folderName;
        if (!string.IsNullOrWhiteSpace(this.FolderPath))
          path = Path.Combine(this.FolderPath, this.folderName);
        return Path.GetFullPath(path);
      }
    }

    public DateTime FolderDate
    {
      get
      {
        return this.folderDate;
      }
    }

    public FileSelectorFolderInfo()
    {
      this.fileSelectorData = (FileSelectorData) null;
      this.folderIndex = -1L;
      this.folderName = "";
      this.folderPath = (string) null;
      this.folderDate = DateTime.MinValue;
    }

    public FileSelectorFolderInfo(FileSelectorData fileSelectorData, long folderIndex, string folderName, DateTime folderDate)
    {
      this.fileSelectorData = fileSelectorData;
      this.folderIndex = folderIndex;
      this.folderName = folderName;
      this.folderPath = (string) null;
      if (fileSelectorData != null)
        this.folderPath = fileSelectorData.CurrentPath;
      this.folderDate = folderDate;
    }

    public FileSelectorFolderInfo(FileSelectorFolderInfo source)
    {
      this.fileSelectorData = source.fileSelectorData;
      this.folderIndex = source.folderIndex;
      this.folderName = source.folderName;
      this.folderPath = source.folderPath;
      this.folderDate = source.folderDate;
    }
  }
}

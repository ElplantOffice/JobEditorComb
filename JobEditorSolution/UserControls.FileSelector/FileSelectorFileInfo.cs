
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace UserControls.FileSelector
{
  public class FileSelectorFileInfo : IFileSelectorFileInfo
  {
    private FileSelectorData fileSelectorData;
    private long fileIndex;
    private string fileName;
    private string filePath;
    private DateTime fileDate;
    private long fileSize;
    private ImageSource fileImage;

    public string FileInfoUid
    {
      get
      {
        return this.fileIndex.ToString();
      }
    }

    public string ViewFileName
    {
      get
      {
        if (this.fileSelectorData != null && !this.fileSelectorData.ViewShowFileExtension)
          return Path.GetFileNameWithoutExtension(this.fileName);
        return this.fileName;
      }
    }

    public string ViewFilePath
    {
      get
      {
        return this.filePath;
      }
    }

    public string ViewFileDate
    {
      get
      {
        return this.fileDate.ToString();
      }
    }

    public string ViewFileSize
    {
      get
      {
        if (this.fileSize >= 1073741824L)
          return ((double) this.fileSize / 1073741824.0).ToString("F2") + " GB";
        if (this.fileSize >= 1048576L)
          return ((double) this.fileSize / 1048576.0).ToString("F2") + " MB";
        if (this.fileSize >= 1024L)
          return ((double) this.fileSize / 1024.0).ToString("F2") + " KB";
        return this.fileSize.ToString() + " bytes";
      }
    }

    public ImageSource ViewFileImage
    {
      get
      {
        return this.fileImage;
      }
    }

    public string FileName
    {
      get
      {
        return this.fileName;
      }
    }

    public string FilePath
    {
      get
      {
        return this.filePath;
      }
    }

    public string FullFileName
    {
      get
      {
        string path = this.FileName;
        if (!string.IsNullOrWhiteSpace(this.FilePath))
          path = Path.Combine(this.FilePath, this.FileName);
        return Path.GetFullPath(path);
      }
    }

    public DateTime FileDate
    {
      get
      {
        return this.fileDate;
      }
    }

    public long FileSize
    {
      get
      {
        return this.fileSize;
      }
    }

    public ImageSource FileImage
    {
      get
      {
        return this.fileImage;
      }
    }

    public bool IsSelected { get; set; }

    public FileSelectorFileInfo()
    {
      this.fileSelectorData = (FileSelectorData) null;
      this.fileIndex = -1L;
      this.fileName = "";
      this.filePath = (string) null;
      this.fileSize = 0L;
      this.fileDate = DateTime.MinValue;
      this.fileImage = (ImageSource) null;
    }

    public FileSelectorFileInfo(FileSelectorData fileSelectorData, long fileIndex, string fileName, long fileSize, DateTime fileDate, ImageSource fileImage)
    {
      this.fileSelectorData = fileSelectorData;
      this.fileIndex = fileIndex;
      this.fileName = fileName;
      this.filePath = (string) null;
      if (fileSelectorData != null)
        this.filePath = fileSelectorData.CurrentPath;
      this.fileSize = fileSize;
      this.fileDate = fileDate;
      this.fileImage = fileImage;
    }

    public FileSelectorFileInfo(FileSelectorFileInfo source)
    {
      this.fileSelectorData = source.fileSelectorData;
      this.fileIndex = source.fileIndex;
      this.fileName = source.fileName;
      this.filePath = source.filePath;
      this.fileSize = source.fileSize;
      this.fileDate = source.fileDate;
      this.fileImage = source.fileImage;
    }

    public static bool IsValidFileName(string fileName)
    {
      return !fileName.StartsWith(" ") && !string.IsNullOrWhiteSpace(fileName) && !new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]").IsMatch(fileName);
    }
  }
}

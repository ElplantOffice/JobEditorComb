
using System.Windows.Data;

namespace UserControls.FileSelector
{
  public interface IFileSelectorData
  {
    bool ViewShowFileExtension { get; set; }

    bool ViewIsSortedByName();

    bool ViewIsSortedByDate();

    void ViewSortByName();

    void ViewSortByDate();

    ListCollectionView ViewListFileInfo { get; }

    ListCollectionView ViewListFolderInfo { get; }

    string ViewCurrentFolderName { get; }

    string ParentPath { get; }

    string DefaultFileExtension { get; set; }

    string FileSearchPattern { get; set; }

    string RootPath { get; set; }

    void GetFolderFileList(string currentPath);

    bool UpdateFolderFileList(string selectFile);

    string CurrentPath { get; }

    IFileSelectorFileInfo GetFileSelectorFileInfo(string fileInfoUid);

    IFileSelectorFolderInfo GetFileSelectorFolderInfo(string folderInfoUid);

    IFileSelectorFileInfo SelectedFile { get; }

    IFileSelectorFileInfo SelectFile(string fileName, bool refresh);

    bool IsValidFileName(string fileName);
  }
}

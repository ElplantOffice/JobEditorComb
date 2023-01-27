using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UserControls.FileSelector
{
    public class FileSelectorData : IFileSelectorData
    {
        private FileSelectorData.SortMode sortMode;

        private List<IFileSelectorFileInfo> listFileInfo = new List<IFileSelectorFileInfo>();

        private List<IFileSelectorFolderInfo> listFolderInfo = new List<IFileSelectorFolderInfo>();

        private ListCollectionView viewListFileInfo;

        private ListCollectionView viewListFolderInfo;

        private Dictionary<string, ImageSource> dictFileImageCache;

        private string currentPath;

        public string CurrentPath
        {
            get
            {
                return this.currentPath;
            }
        }

        public string DefaultFileExtension
        {
            get;
            set;
        }

        private bool FileListNeedsUpdating
        {
            get
            {
                bool flag;
                List<IFileSelectorFileInfo> fileSelectorFileInfos = new List<IFileSelectorFileInfo>();
                try
                {
                    this.GetFileList(this.currentPath, fileSelectorFileInfos, false);
                    if (fileSelectorFileInfos.Count == this.listFileInfo.Count)
                    {
                        int num = 0;
                        while (num < fileSelectorFileInfos.Count)
                        {
                            if (fileSelectorFileInfos[num].FileName != this.listFileInfo[num].FileName)
                            {
                                flag = true;
                                return flag;
                            }
                            else if (fileSelectorFileInfos[num].FileDate != this.listFileInfo[num].FileDate)
                            {
                                flag = true;
                                return flag;
                            }
                            else if (fileSelectorFileInfos[num].FileSize == this.listFileInfo[num].FileSize)
                            {
                                num++;
                            }
                            else
                            {
                                flag = true;
                                return flag;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                return flag;
            }
        }

        public string FileSearchPattern
        {
            get;
            set;
        }

        private bool FolderListNeedsUpdating
        {
            get
            {
                bool flag;
                List<IFileSelectorFolderInfo> fileSelectorFolderInfos = new List<IFileSelectorFolderInfo>();
                try
                {
                    this.GetFolderList(this.currentPath, fileSelectorFolderInfos);
                    if (fileSelectorFolderInfos.Count == this.listFolderInfo.Count)
                    {
                        int num = 0;
                        while (num < fileSelectorFolderInfos.Count)
                        {
                            if (fileSelectorFolderInfos[num].FolderName == this.listFolderInfo[num].FolderName)
                            {
                                num++;
                            }
                            else
                            {
                                flag = true;
                                return flag;
                            }
                        }
                        return false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                return flag;
            }
        }

        public string ParentPath
        {
            get
            {
                string directoryName = null;
                if (string.IsNullOrWhiteSpace(this.currentPath))
                {
                    return directoryName;
                }
                string fullPath = null;
                try
                {
                    fullPath = Path.GetFullPath(this.currentPath);
                }
                catch (ArgumentNullException argumentNullException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(currentPath) -> ArgumentNullException !");
                    throw argumentNullException;
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(currentPath) -> ArgumentException, currentPath:{0} !", this.currentPath);
                    throw argumentException;
                }
                catch (SecurityException securityException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(currentPath) -> SecurityException, currentPath:{0} !", this.currentPath);
                    throw securityException;
                }
                catch (NotSupportedException notSupportedException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(currentPath) -> NotSupportedException, currentPath:{0} !", this.currentPath);
                    throw notSupportedException;
                }
                catch (PathTooLongException pathTooLongException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(currentPath) -> PathTooLongException, currentPath:{0} !", this.currentPath);
                    throw pathTooLongException;
                }
                if (!string.IsNullOrWhiteSpace(this.RootPath))
                {
                    if (string.Equals(this.RootPath.TrimEnd(new char[] { Path.DirectorySeparatorChar }), fullPath.TrimEnd(new char[] { Path.DirectorySeparatorChar })))
                    {
                        return directoryName;
                    }
                }
                directoryName = Path.GetDirectoryName(fullPath.TrimEnd(new char[] { '\\' }));
                if (directoryName == null && !string.IsNullOrWhiteSpace(fullPath) && fullPath[1] == ':')
                {
                    directoryName = "";
                }
                return directoryName;
            }
        }

        public string RootPath
        {
            get;
            set;
        }

        public IFileSelectorFileInfo SelectedFile
        {
            get
            {
                return this.listFileInfo.FirstOrDefault<IFileSelectorFileInfo>((IFileSelectorFileInfo i) => i.IsSelected);
            }
        }

        public string ViewCurrentFolderName
        {
            get
            {
                return this.ViewFolderName(this.currentPath);
            }
        }

        public ListCollectionView ViewListFileInfo
        {
            get
            {
                if (this.viewListFileInfo == null)
                {
                    this.viewListFileInfo = new ListCollectionView(this.listFileInfo);
                }
                return this.viewListFileInfo;
            }
        }

        public ListCollectionView ViewListFolderInfo
        {
            get
            {
                if (this.viewListFolderInfo == null)
                {
                    this.viewListFolderInfo = new ListCollectionView(this.listFolderInfo);
                }
                return this.viewListFolderInfo;
            }
        }

        public bool ViewShowFileExtension
        {
            get;
            set;
        }

        public FileSelectorData()
        {
        }

        public static Icon ExtractAssociatedIcon(string filePath)
        {
            Uri uri;
            int num = 0;
            if (filePath == null)
            {
                throw new ArgumentException(string.Format("'{0}' is not valid for '{1}'", "null", "filePath"), "filePath");
            }
            try
            {
                uri = new Uri(filePath);
            }
            catch (UriFormatException uriFormatException)
            {
                filePath = Path.GetFullPath(filePath);
                uri = new Uri(filePath);
            }
            if (uri.IsFile)
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException(filePath);
                }
                StringBuilder stringBuilder = new StringBuilder(260);
                stringBuilder.Append(filePath);
                IntPtr intPtr = FileSelectorData.SafeNativeMethods.ExtractAssociatedIcon(new HandleRef(null, IntPtr.Zero), stringBuilder, ref num);
                if (intPtr != IntPtr.Zero)
                {
                    return Icon.FromHandle(intPtr);
                }
            }
            return null;
        }

        private void GetFileList(string path, List<IFileSelectorFileInfo> listFileInfo, bool includeImage)
        {
            listFileInfo.Clear();
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            DirectoryInfo directoryInfo = null;
            try
            {
                directoryInfo = new DirectoryInfo(path);
            }
            catch (ArgumentNullException argumentNullException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, DirectoryInfo(path) ->  ArgumentNullException !");
                throw argumentNullException;
            }
            catch (SecurityException securityException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, DirectoryInfo(path) ->  SecurityException, path:{0} !", path);
                return;
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, DirectoryInfo(path) ->  ArgumentException, path:{0} !", path);
                throw argumentException;
            }
            catch (PathTooLongException pathTooLongException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, DirectoryInfo(path) ->  PathTooLongException, path:{0} !", path);
                throw pathTooLongException;
            }
            if (!directoryInfo.Exists)
            {
                Console.WriteLine("FileSelectorData:GetFileList, the given path '{0}' was not found !", path);
                return;
            }
            try
            {
                directoryInfo.GetDirectories();
                goto Label1;
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, dirinfo.GetDirectories() ->  DirectoryNotFoundException, path:{0} !", path);
            }
            catch (SecurityException securityException1)
            {
                Console.WriteLine("FileSelectorData:GetFileList, dirinfo.GetDirectories() ->  SecurityException, path:{0} !", path);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine("FileSelectorData:GetFileList, dirinfo.GetDirectories() ->  UnauthorizedAccessException, path:{0} !", path);
            }
            return;
        Label1:
            if (this.dictFileImageCache == null & includeImage)
            {
                this.dictFileImageCache = new Dictionary<string, ImageSource>();
            }
            string fileSearchPattern = this.FileSearchPattern;
            if (string.IsNullOrWhiteSpace(fileSearchPattern))
            {
                fileSearchPattern = (!string.IsNullOrWhiteSpace(this.DefaultFileExtension) ? string.Format("*{0}", this.DefaultFileExtension) : "*.*");
            }
            FileInfo[] files = directoryInfo.GetFiles(fileSearchPattern);
            for (int i = 0; i < (int)files.Length; i++)
            {
                FileInfo fileInfo = files[i];
                string name = fileInfo.Name;
                long length = fileInfo.Length;
                DateTime lastWriteTime = fileInfo.LastWriteTime;
                ImageSource item = null;
                if (includeImage)
                {
                    if (this.dictFileImageCache.ContainsKey(fileInfo.Extension))
                    {
                        item = this.dictFileImageCache[fileInfo.Extension];
                    }
                    else
                    {
                        item = FileSelectorData.GetImageSourceFromIcon(FileSelectorData.ExtractAssociatedIcon(fileInfo.FullName));
                        bool flag = true;
                        if (fileInfo.Extension.ToLower() == ".exe")
                        {
                            flag = false;
                        }
                        if (fileInfo.Extension.ToLower() == ".dll")
                        {
                            flag = false;
                        }
                        if (fileInfo.Extension.ToLower() == ".lnk")
                        {
                            flag = false;
                        }
                        if (flag && item != null)
                        {
                            this.dictFileImageCache.Add(fileInfo.Extension, item);
                        }
                    }
                }
                long count = (long)listFileInfo.Count;
                listFileInfo.Add(new FileSelectorFileInfo(this, count, name, length, lastWriteTime, item));
            }
        }

        public IFileSelectorFileInfo GetFileSelectorFileInfo(string fileInfoUid)
        {
            return this.listFileInfo.FirstOrDefault<IFileSelectorFileInfo>((IFileSelectorFileInfo i) => i.FileInfoUid == fileInfoUid);
        }

        public IFileSelectorFolderInfo GetFileSelectorFolderInfo(string folderInfoUid)
        {
            return this.listFolderInfo.FirstOrDefault<IFileSelectorFolderInfo>((IFileSelectorFolderInfo i) => i.FolderInfoUid == folderInfoUid);
        }

        public void GetFolderFileList(string currentPath)
        {
            this.listFolderInfo.Clear();
            this.listFileInfo.Clear();
            this.currentPath = currentPath;
            try
            {
                this.GetFolderList(currentPath, this.listFolderInfo);
                this.GetFileList(currentPath, this.listFileInfo, true);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            this.viewListFolderInfo.Refresh();
            this.viewListFileInfo.Refresh();
        }

        private void GetFolderList(string path, List<IFileSelectorFolderInfo> listFolderInfo)
        {
            int i;
            listFolderInfo.Clear();
            if (string.IsNullOrWhiteSpace(path))
            {
                string[] logicalDrives = Directory.GetLogicalDrives();
                for (i = 0; i < (int)logicalDrives.Length; i++)
                {
                    string str = logicalDrives[i];
                    DateTime minValue = DateTime.MinValue;
                    long count = (long)listFolderInfo.Count;
                    listFolderInfo.Add(new FileSelectorFolderInfo(this, count, str, minValue));
                }
                return;
            }
            DirectoryInfo directoryInfo = null;
            try
            {
                directoryInfo = new DirectoryInfo(path);
            }
            catch (ArgumentNullException argumentNullException)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, DirectoryInfo(path) ->  ArgumentNullException !");
                throw argumentNullException;
            }
            catch (SecurityException securityException)
            {
                return;
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, DirectoryInfo(path) ->  ArgumentException, path:{0} !", path);
                throw argumentException;
            }
            catch (PathTooLongException pathTooLongException)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, DirectoryInfo(path) ->  PathTooLongException, path:{0} !", path);
                throw pathTooLongException;
            }
            if (!directoryInfo.Exists)
            {
                return;
            }
            DirectoryInfo[] directories = null;
            try
            {
                directories = directoryInfo.GetDirectories();
                DirectoryInfo[] directoryInfoArray = directories;
                for (i = 0; i < (int)directoryInfoArray.Length; i++)
                {
                    DirectoryInfo directoryInfo1 = directoryInfoArray[i];
                    string name = directoryInfo1.Name;
                    DateTime lastWriteTime = directoryInfo1.LastWriteTime;
                    long num = (long)listFolderInfo.Count;
                    listFolderInfo.Add(new FileSelectorFolderInfo(this, num, name, lastWriteTime));
                }
                return;
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, dirinfo.GetDirectories() ->  DirectoryNotFoundException, path:{0} !", path);
            }
            catch (SecurityException securityException1)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, dirinfo.GetDirectories() ->  SecurityException, path:{0} !", path);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Console.WriteLine("FileSelectorData:GetFolderList, dirinfo.GetDirectories() ->  UnauthorizedAccessException, path:{0} !", path);
            }
        }

        public static ImageSource GetImageSourceFromIcon(Icon icon)
        {
            ImageSource imageSource = null;
            if (icon != null)
            {
                imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            return imageSource;
        }

        public bool IsValidFileName(string fileName)
        {
            return FileSelectorFileInfo.IsValidFileName(fileName);
        }

        public IFileSelectorFileInfo SelectFile(string fileName, bool refresh)
        {
            IFileSelectorFileInfo fileSelectorFileInfo = this.listFileInfo.FirstOrDefault<IFileSelectorFileInfo>((IFileSelectorFileInfo i) => i.IsSelected);
            IFileSelectorFileInfo fileSelectorFileInfo1 = null;
            if (fileName != null)
            {
                fileSelectorFileInfo1 = this.listFileInfo.FirstOrDefault<IFileSelectorFileInfo>((IFileSelectorFileInfo i) => string.Equals(i.FileName, fileName, StringComparison.OrdinalIgnoreCase));
            }
            bool flag = false;
            if (fileSelectorFileInfo != null && fileSelectorFileInfo1 != fileSelectorFileInfo && fileSelectorFileInfo.IsSelected)
            {
                fileSelectorFileInfo.IsSelected = false;
                flag = true;
            }
            if (fileSelectorFileInfo1 != null && !fileSelectorFileInfo1.IsSelected)
            {
                fileSelectorFileInfo1.IsSelected = true;
                flag = true;
            }
            if (flag & refresh)
            {
                this.viewListFileInfo.Refresh();
            }
            return fileSelectorFileInfo1;
        }

        public bool UpdateFolderFileList(string selectFile)
        {
            bool flag = false;
            try
            {
                if (this.FolderListNeedsUpdating)
                {
                    this.listFolderInfo.Clear();
                    this.GetFolderList(this.currentPath, this.listFolderInfo);
                    this.viewListFolderInfo.Refresh();
                    flag = true;
                }
                if (this.FileListNeedsUpdating)
                {
                    if (this.SelectedFile != null)
                    {
                        string fileName = this.SelectedFile.FileName;
                    }
                    this.listFileInfo.Clear();
                    this.GetFileList(this.currentPath, this.listFileInfo, true);
                    this.SelectFile(selectFile, false);
                    this.viewListFileInfo.Refresh();
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag;
        }

        public string ViewFolderName(string folderPath)
        {
            string str = "";
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                string fullPath = null;
                try
                {
                    fullPath = Path.GetFullPath(folderPath);
                }
                catch (ArgumentNullException argumentNullException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(folderPath) -> ArgumentNullException !");
                    throw argumentNullException;
                }
                catch (ArgumentException argumentException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(folderPath) -> ArgumentException, folderPath:{0} !", folderPath);
                    throw argumentException;
                }
                catch (SecurityException securityException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(folderPath) -> SecurityException, folderPath:{0} !", folderPath);
                    throw securityException;
                }
                catch (NotSupportedException notSupportedException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(folderPath) -> NotSupportedException, folderPath:{0} !", folderPath);
                    throw notSupportedException;
                }
                catch (PathTooLongException pathTooLongException)
                {
                    Console.WriteLine("FileSelectorData:ParentFolderPath, Path.GetFullPath(folderPath) -> PathTooLongException, folderPath:{0} !", folderPath);
                    throw pathTooLongException;
                }
                fullPath = fullPath.TrimEnd(new char[] { Path.DirectorySeparatorChar });
                string[] strArrays = fullPath.Split(new char[] { Path.DirectorySeparatorChar });
                if (strArrays.Length != 0)
                {
                    string[] strArrays1 = strArrays;
                    str = strArrays1[(int)strArrays1.Length - 1];
                }
                if (str.Length >= 2 && str[1] == ':')
                {
                    string str1 = string.Concat(str.Substring(0, 2), Path.DirectorySeparatorChar);
                    DriveInfo driveInfo = DriveInfo.GetDrives().FirstOrDefault<DriveInfo>((DriveInfo i) => i.Name == str1);
                    if (driveInfo != null)
                    {
                        string volumeLabel = driveInfo.DriveType.ToString();
                        switch (driveInfo.DriveType)
                        {
                            case DriveType.Removable:
                            case DriveType.Fixed:
                            case DriveType.CDRom:
                                {
                                    if (!driveInfo.IsReady || string.IsNullOrWhiteSpace(driveInfo.VolumeLabel))
                                    {
                                        goto case DriveType.Network;
                                    }
                                    volumeLabel = driveInfo.VolumeLabel;
                                    goto case DriveType.Network;
                                }
                            case DriveType.Network:
                                {
                                    char[] directorySeparatorChar = new char[] { Path.DirectorySeparatorChar };
                                    str = string.Format("{0} ({1})", volumeLabel, str.TrimEnd(directorySeparatorChar));
                                    break;
                                }
                            default:
                                {
                                    goto case DriveType.Network;
                                }
                        }
                    }
                }
            }
            return str;
        }

        public bool ViewIsSortedByDate()
        {
            if (this.sortMode == FileSelectorData.SortMode.ByDate)
            {
                return true;
            }
            return false;
        }

        public bool ViewIsSortedByName()
        {
            if (this.sortMode == FileSelectorData.SortMode.ByName)
            {
                return true;
            }
            return false;
        }

        public void ViewSortByDate()
        {
            if (this.ViewIsSortedByDate())
            {
                return;
            }
            this.sortMode = FileSelectorData.SortMode.ByDate;
            this.viewListFileInfo.SortDescriptions.Clear();
            this.viewListFileInfo.SortDescriptions.Add(new SortDescription("FileDate", ListSortDirection.Descending));
        }

        public void ViewSortByName()
        {
            if (this.ViewIsSortedByName())
            {
                return;
            }
            this.sortMode = FileSelectorData.SortMode.ByName;
            this.viewListFileInfo.SortDescriptions.Clear();
            this.viewListFileInfo.SortDescriptions.Add(new SortDescription("FileName", ListSortDirection.Ascending));
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("shell32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            internal static extern IntPtr ExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);
        }

        private enum SortMode
        {
            ByName,
            ByDate
        }
    }
}
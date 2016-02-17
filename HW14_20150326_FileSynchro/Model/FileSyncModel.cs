using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW14_20150326_FileSynchro.Model
{
    class FileSyncModel
    {
        /// <summary>
        /// Source folder - 1st folder
        /// </summary>
        DirectoryInfo _sourceFolder;

        /// <summary>
        /// Destination folder - 2nd folder
        /// </summary>
        DirectoryInfo _destinationFolder;

        /// <summary>
        /// property to get/set 1st folder
        /// </summary>
        public DirectoryInfo DestinationFolder
        {
            get { return _destinationFolder; }
            set { _destinationFolder = value; }
        }

        /// <summary>
        /// property to get/set 2nd folder
        /// </summary>
        public DirectoryInfo SourceFolder
        {
            get { return _sourceFolder; }
            set { _sourceFolder = value; }
        }

        public string CurrentFile { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public FileSyncModel()
        {

        }

        /// <summary>
        /// Synchronize function
        /// Receives direction parameter from presenter
        /// </summary>
        /// <param name="direction">0 - Equivalent synchronization (both directions)
        /// 1- 1st folder as main folder
        /// 2- 2nd folder as main folder</param>
        public void Synchronize(int direction)
        {
            switch (direction)
            {
                default:
                case 0:
                    ///DirectorySync described below
                    ///
                    ///1st - synchronizing files and directories from 1st folder to 2nd folder
                    DirectorySync(SourceFolder, DestinationFolder, false);
                    ///2nd - synchronizing files and directories from 2nd folder to 1st folder
                    DirectorySync(DestinationFolder, SourceFolder, false);
                    break;
                case 1:
                    DirectorySync(SourceFolder, DestinationFolder, false);
                    DirectorySync(DestinationFolder, SourceFolder, true);
                    break;
                case 2:
                    DirectorySync(DestinationFolder, SourceFolder, false);
                    DirectorySync(SourceFolder, DestinationFolder, true);

                    break;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sdir">Source directory - from</param>
        /// <param name="ddir">Destination directory - to</param>
        /// <param name="Remove">True, if non-existing files from Source directory
        /// should be deleted from destination directory. False, if files should be keeped</param>
        private void DirectorySync(DirectoryInfo sdir, DirectoryInfo ddir, bool Remove)
        {
            ///creating file list from source directory
            FileInfo[] tmpFI = sdir.GetFiles();

            foreach (FileInfo fi in tmpFI)
            {
                ///synchronize each file from source directory
                FileSync(fi, ddir, Remove);
            }
            ///get directories in source directory
            DirectoryInfo[] tmpSDI = sdir.GetDirectories();
            ///get directories in destination directory
            DirectoryInfo[] tmpDDI = ddir.GetDirectories();

            ///Create associated dictionary destinations directories names, destination directories DirectoryInfo's
            Dictionary<string, DirectoryInfo> tmpDDINames = new Dictionary<string, DirectoryInfo>();
            foreach (DirectoryInfo ddi in tmpDDI)
            {
                tmpDDINames.Add(ddi.Name, ddi);
            }

            ///for each Source directory's directory DirectoryInfo :)
            foreach (DirectoryInfo sdi in tmpSDI)
            {
                ///if destination folder has same as source directory's directory
                if (tmpDDINames.ContainsKey(sdi.Name))
                {
                    ///synchronize these directories
                    DirectorySync(sdi, tmpDDINames[sdi.Name], Remove);
                }
                else
                {
                    try
                    {
                        ///if there (in destination folder) is no such directory,
                        ///should this directory be deleted in source directory?
                        ///Remove:True - yes, destroy the folder!
                        ///Remove:False - no, let it be...
                        if (Remove)
                        {
                            sdi.Delete(true);
                        }
                        else
                        {
                            ///...and create in destination folder subdirectory
                            ///with such name as in source directory...
                            DirectoryInfo tmp = ddir.CreateSubdirectory(sdi.Name);
                            ///...and don't forget to synchronize files
                            DirectorySync(sdi, tmp, Remove);
                        }
                    }
                    ///in case of exception call it on higher level -> to Presenter
                    catch (Exception ex)
                    { throw ex; }
                }
            }

        }

        /// <summary>
        /// File synchronization function
        /// </summary>
        /// <param name="fi">FileInfo of the file to be synchronized</param>
        /// <param name="dest">Destination folder DirectoryInfo</param>
        /// <param name="Remove">True, if non-existing file in Destination directory
        /// should be deleted from source directory. False, if files should be keeped</param>
        private void FileSync(FileInfo fi, DirectoryInfo dest, bool Remove)
        {
            string sourceFile = fi.Name;
            ///get FI's from destination directory
            FileInfo[] tmpDFIs = dest.GetFiles();
            ///Create associated Dictionary: filename - FileInfo
            Dictionary<string, FileInfo> dfiles = new Dictionary<string, FileInfo>();
            foreach (FileInfo tmpFI in tmpDFIs)
            {
                dfiles.Add(tmpFI.Name, tmpFI);
                CurrentFile = tmpFI.FullName;
            }
            ///Look if destination file Contains source file
            if (!dfiles.ContainsKey(sourceFile))
            {
                ///cannot find file in destination folder
                ///Should we remove source file?
                ///Remove:true - yes,remove
                ///Remove:false - no, let it live)
                if (Remove)
                {
                    fi.Delete();
                }
                else
                {
                    if (fi.Exists)
                    {///copy source file to destionation directory
						try
                        {
                            fi.CopyTo(dest.FullName + "\\" + fi.Name);
                        }
                        catch (Exception ex)
                        { throw ex; }
                    }
                }
            }
            else
            {
                ///if we found a source file in destination directory
                FileInfo dtfi = dfiles[sourceFile];
                ///Refresh FileInfo's of both files
                fi.Refresh();
                dtfi.Refresh();

                ///Check which file is newer
                ///If destination file is older...
                if (dtfi.LastWriteTime < fi.LastWriteTime)
                {
                    try
                    {
                        ///...overwrite older file with newer one
                        fi.CopyTo(dtfi.FullName, true);
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                ///if destination file is newer...
                else if (dtfi.LastWriteTime > fi.LastWriteTime)
                {
                    try
                    {///...overwrite source file with newer one
						dtfi.CopyTo(fi.FullName, true);
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                ///and there is no "else" if they are equivalent,
                ///because we don't have to do smth if they are same
            }

        }
    }
}

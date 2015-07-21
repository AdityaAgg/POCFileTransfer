using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Http;

namespace POCFileTransfer.Controllers
{
    public class TreeRequest
    {
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("Url={0}", this.Url);
        }
    }
    public abstract class FileObject
    {
        public string path { get; set; }
        public string ObjectName { get; set; }
        public override string ToString()
        {
            return string.Format("ObjectName={0}", this.ObjectName);
        }
    }

    public class File:FileObject
    {
        public string FileSize { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, FileSize={1}", base.ToString(), this.FileSize);
        }
    }
    public class Folder:FileObject
    {
       
        public IList<FileObject> FilesinFolder { get; set; }
        public override string ToString()
        {
            return string.Format("FolderName (variable FolderName)={0}, FilesinFolder={1}", this.ObjectName, this.FilesinFolder);
        }
    }

    /*public class TreeResponse
    {
        public string filterPath { get; set; }

    }*/

    public class TreeDownloadAPIController : ApiController
    {
        public Folder PostFileTree([FromBody]TreeRequest request)//(Person obj )
        {
            string path = @"C:\workspace\uploads\" + request.Url + @"\";
            DirectoryInfo diMain = new DirectoryInfo(path);
            Folder folder = new Folder();
            folder.ObjectName = "root";
            Debug.Print(folder.ObjectName);
            traverseFileTree(System.IO.Directory.EnumerateFileSystemEntries(path), folder, "");
            return folder;
        }


        private void traverseFileTree (IEnumerable<string> allEntriesinDirectoryLocal, Folder mainFolderLocal, string path)
        {
            mainFolderLocal.FilesinFolder = new List<FileObject>();
            foreach (string name in allEntriesinDirectoryLocal)
            {

                
                if (System.IO.Directory.Exists(name))
                {

                    Debug.Print("Directory: "+name);
                    DirectoryInfo di = new DirectoryInfo(name);
                    Folder localFolder = new Folder();
                    string path2 = path + di.Name + @"\";

                    //for the sake of drag and drop folder name added to path of folder because path actually signifies upload path on drop
                    localFolder.ObjectName = di.Name;
                    localFolder.path = path2;
                    mainFolderLocal.FilesinFolder.Add(localFolder);
                    
                    
                    traverseFileTree(System.IO.Directory.EnumerateFileSystemEntries(name),(Folder)mainFolderLocal.FilesinFolder[mainFolderLocal.FilesinFolder.Count-1], path2);
                    
                    
                }
                else
                {
                    Debug.Print("File: "+name);
                    FileInfo fi = new FileInfo(name);
                 
                    File localFile = new File();
                    localFile.ObjectName = fi.Name;
                    localFile.FileSize = fi.Length + " bytes";
                    localFile.path = path;
                    mainFolderLocal.FilesinFolder.Add(localFile);

                }
            }
        }


    }
}

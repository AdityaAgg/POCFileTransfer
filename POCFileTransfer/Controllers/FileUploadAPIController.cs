using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace POCFileTransfer.Controllers
{
    public class FileUploadAPIController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            await

                        // check if multi file upload with large files works and with different
                        Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider()).ContinueWith(async (tsk) =>
                        {
                            MultipartMemoryStreamProvider prvdr = tsk.Result;

                            foreach (HttpContent ctnt in prvdr.Contents)
                            {
                                if (ctnt.Headers.ContentDisposition.FileName != null)
                                {
                                    //find file path
                                    string fileName = ctnt.Headers.ContentDisposition.FileName.Replace("\"", "");

                                    Debug.Print("fileName: "+fileName);
                                   
                                    string filePath = @"C:\workspace\uploads\" + fileName;
                                    

                                    //create a new directory
                                    (new FileInfo(filePath)).Directory.Create();


                                    //copy to a file using fileStream
                                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                    {
                                        await ctnt.CopyToAsync(fileStream);
                                        fileStream.Close();
                                    }

                                }
                                else
                                {

                                }
                  
                            }
                        });
           

            return Ok(new { Message = "Done." });
        }
    }
}

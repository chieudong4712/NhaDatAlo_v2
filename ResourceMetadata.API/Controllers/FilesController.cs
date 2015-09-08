using Newtonsoft.Json;
using ResourceMetadata.API.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Renci.SshNet;
using System.Web.Http;
using ResourceMetadata.API.Models;
using ResourceMetadata.Model;
using AutoMapper;
using ResourceMetadata.Service;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Core.Util;
using System.Web.Script.Serialization;

namespace ResourceMetadata.API.Controllers
{
    public class FilesController : ApiController
    {
        private readonly IPictureService pictureService;
        public FilesController(IPictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        [System.Web.Http.HttpPost]
        [Route("api/files/UploadFile")]
        public void UploadFile()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments"));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments"));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments"),
                                                httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);

                if (File.Exists(fileSavePath))
                {
                    //AppConfig is static class used as accessor for SFTP configurations from web.config
                    using (SftpClient sftpClient = new SftpClient(AppConfig.SftpServerIp,
                                                                 Convert.ToInt32(AppConfig.SftpServerPort),
                                                                 AppConfig.SftpServerUserName,
                                                                 AppConfig.SftpServerPassword))
                    {
                        sftpClient.Connect();
                        if (!sftpClient.Exists(AppConfig.SftpPath + "UserID"))
                        {
                            sftpClient.CreateDirectory(AppConfig.SftpPath + "UserID");
                        }

                        Stream fin = File.OpenRead(fileSavePath);
                        sftpClient.UploadFile(fin, AppConfig.SftpPath + "/" + httpPostedFile.FileName,
                                              true);
                        fin.Close();
                        sftpClient.Disconnect();
                    }
                }
            }
        }

        [HttpPost] // This is from System.Web.Http, and not from System.Web.Mvc
        [Route("api/files/Upload")]
        public async Task<HttpResponseMessage> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = GetMultipartProvider();
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
            // so this is how you can get the original file name
            var originalFileName = GetDeserializedFileName(result.FileData.First());
            var fileName = Path.GetFileNameWithoutExtension(originalFileName) + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetExtension(originalFileName);

            // uploadedFileInfo object will give you some additional stuff like file length,
            // creation time, directory name, a few filesystem methods etc..
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);

            // Save Picture
            var fileUploadObj = GetFormData(result);
            fileUploadObj.FileName = fileName;
            var picture = new Picture();
            picture = Mapper.Map(fileUploadObj, picture);
            pictureService.Add(picture);

            // Save file
            string saveFolder = HttpContext.Current.Server.MapPath("~/Images/" + fileUploadObj.PictureType + "/" + PictureSize.Auto + "/");
            string filePath = Path.Combine(saveFolder, fileName);
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            File.Move(uploadedFileInfo.FullName, filePath);
            FileUtil.CreateThumbPicture(filePath, pictureService.GetThumbLocalPath(fileName, ConvertUtil.ToEnum<PictureType>(fileUploadObj.PictureType), PictureSize.Tiny), PictureSize.Tiny);
            FileUtil.CreateThumbPicture(filePath, pictureService.GetThumbLocalPath(fileName, ConvertUtil.ToEnum<PictureType>(fileUploadObj.PictureType), PictureSize.Medium), PictureSize.Medium);

            // Through the request response you can return an object to the Angular controller
            // You will be able to access this in the .success callback through its data attribute
            // If you want to send something to the .error callback, use the HttpStatusCode.BadRequest instead
            return this.Request.CreateResponse(HttpStatusCode.OK, new { fileName });
        }

        // You could extract these two private methods to a separate utility class since
        // they do not really belong to a controller class but that is up to you
        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            // IMPORTANT: replace "(tilde)" with the real tilde character
            // (our editor doesn't allow it, so I just wrote "(tilde)" instead)
            var uploadFolder = "/FileUploads"; // you could put this to web.config
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }


        private PictureViewModel GetFormData(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var dictionary = ConvertUtil.NameValueCollectionToDictionary(result.FormData, false);
                string json = new JavaScriptSerializer().Serialize(dictionary);
                return JsonConvert.DeserializeObject<PictureViewModel>(json);
            }

            return null;
        }

        // Extracts Request FormatData as a strongly typed model
        private T GetFormData<T>(MultipartFormDataStreamProvider result) where T : class
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
	}
}
using System.IO;
using System.Security.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using ResourceMetadata.Model;
using ResourceMetadata.Model.SearchModels;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Model.Base;
using ResourceMetadata.Data.Infrastructure;
using ResourceMetadata.Data.Repositories;
using ResourceMetadata.Core.Util;
using ResourceMetadata.Core.Setting;

namespace ResourceMetadata.Service
{
    public interface IPictureService : IBaseService<Picture, PictureSE>
    {
        Picture GetByPictureId(long id);
        IEnumerable<Picture> GetByRef(long refId, string pictureType);
        IEnumerable<Picture> GetAllPictures();
        void CreatePicture(Picture picture);
        void SavePicture();
        void UpdatePicture(Picture picture);
        void DeletePicture(Picture picture);
        IEnumerable<string> GetPictureUrlsByRef(long refId, PictureType pictureType, PictureSize pictureSize = PictureSize.Auto);
        string GetPictureUrlByRef(long refId, PictureType pictureType, PictureSize pictureSize = PictureSize.Auto);
        string GetPictureUrl(long refId, PictureType pictureType, PictureSize pictureSize = PictureSize.Auto);
        string GetPictureUrl(string fileName, PictureType pictureType, PictureSize pictureSize);
        string GetDefaultPictureUrl(PictureType pictureType, PictureSize pictureSize);
        string GetThumbLocalPath(string thumbFileName, PictureType pictureType, PictureSize pictureSize);
    }

    public class PictureService : BaseService<Picture, BaseSearchEntity<Picture>>, IPictureService
    {
        #region Const

        private const int MULTIPLE_THUMB_DIRECTORIES_LENGTH = 3;

        #endregion

        private readonly IWorkContext _webHelper;
        private readonly MediaSettings _mediaSettings;

        public PictureService(IUnitOfWork unitOfWork, IPictureRepository pictureRepository, IWorkContext webHelper)
            : base(unitOfWork, pictureRepository)
        {
            _webHelper = webHelper;
        }

        public Picture GetByPictureId(long pictureId)
        {
            if (pictureId == 0) return null;
            return repository.GetById(pictureId);
        }

        public IEnumerable<Picture> GetByRef(long refId, string pictureType)
        {
            var q = repository.GetMany(x => x.RefId == refId && x.PictureType == pictureType && x.Status!= StatusObject.Deleted);
            return q;
        }

        public IEnumerable<Picture> GetAllPictures()
        {
            return repository.GetAll();
        }

        public void CreatePicture(Picture picture)
        {
            repository.Add(picture);
            SavePicture();
        }

        public void UpdatePicture(Picture picture)
        {
            repository.Update(picture);
            SavePicture();
        }

        public void SavePicture()
        {
            unitOfWork.SaveChanges();
        }

        public void DeletePicture(Picture picture)
        {
            DeletePictureOnFileSystem(picture);
            DeletePictureThumbs(picture);
            repository.Delete(picture);
            SavePicture();
        }

        public IQueryable<Picture> Search(PictureSE se, bool isNoTracking = true, bool withoutDeleted = true)
        {
            var q = base.Search(se, isNoTracking, withoutDeleted);

            if (!String.IsNullOrEmpty(se.Title))
            {
                q = q.Where(x => x.Title.Contains(se.Title));
            }
            if (!String.IsNullOrEmpty(se.Description))
            {
                q = q.Where(x => x.Description.Contains(se.Description));
            }

            if (se.PictureType != null)
            {
                q = q.Where(x => x.PictureType == se.PictureType);
            }

            if (se.RefId != 0)
            {
                q = q.Where(x => x.RefId == se.RefId);
            }

            return q;
        }

        #region Utilities
        public string GetDefaultPictureUrl(PictureType pictureType, PictureSize pictureSize)
        {
            var url = "images/" + pictureType + "/" + pictureSize + "/";

            url = url + "default.jpg";
            return url;
        }

        
        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var fileName = picture.FileName;
            var pictureType = ConvertUtil.ToEnum<PictureType>(picture.PictureType);

            string filePath = GetPictureLocalPath(fileName, pictureType);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            var fileName = picture.FileName;
            var pictureType = ConvertUtil.ToEnum<PictureType>(picture.PictureType);

            foreach (var pictureSize in EnumUtil.GetValues<PictureSize>())
            {
                var thumbFilePath = GetThumbLocalPath(fileName, pictureType, pictureSize);
                File.Delete(thumbFilePath);
            }
        }

        /// <summary>
        /// Get pictureURL by RefId
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="pictureType"></param>
        /// <param name="pictureSize"></param>
        /// <returns></returns>
        public IEnumerable<string> GetPictureUrlsByRef(long refId, PictureType pictureType, PictureSize pictureSize = PictureSize.Auto)
        {
            var pictures = GetByRef(refId, pictureType.ToString());
            List<string> pictureURLs = new List<string>();
            foreach (var picture in pictures)
            {
                var pictureUrl = GetPictureUrl(picture.Id,pictureType,pictureSize);
                pictureURLs.Add(pictureUrl);
            }
            return pictureURLs;

        }

        /// <summary>
        /// Get first pictureURL by ref
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="pictureType"></param>
        /// <param name="pictureSize"></param>
        /// <returns></returns>
        public string GetPictureUrlByRef(long refId, PictureType pictureType, PictureSize pictureSize = PictureSize.Auto)
        {
            return GetPictureUrlsByRef(refId,pictureType,pictureSize).FirstOrDefault();
        }

        /// <summary>
        /// Get picture URL by Picture id
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="pictureType"></param>
        /// <param name="pictureSize"></param>
        /// <returns></returns>
        public string GetPictureUrl(long pictureId, PictureType pictureType, PictureSize pictureSize)
        {
            var picture = GetByPictureId(pictureId);
            if (picture==null)
            {
                return null;
            }
            
            var fileName = picture.FileName;
            
            var thumbFileUrl = GetPictureUrl(fileName, pictureType, pictureSize);
            return thumbFileUrl;
        }
        /// <summary>
        /// Get picture (thumb) URL 
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Local picture thumb path</returns>
        public string GetPictureUrl(string fileName, PictureType pictureType, PictureSize pictureSize)
        {
            if (String.IsNullOrEmpty(fileName)) return null;

            var fileExtension = Path.GetExtension(fileName);
            var filePath = GetPictureLocalPath(fileName, pictureType);
            var thumbFilePath = String.Empty;
            if (!File.Exists(filePath)) return null;

            if (pictureSize == PictureSize.Auto)
            {
                thumbFilePath = filePath;
            }
            else
            {
                thumbFilePath = GetThumbLocalPath(fileName, pictureType, pictureSize);
                if (!File.Exists(thumbFilePath))
                {
                    FileUtil.CreateThumbPicture(fileName, thumbFilePath, pictureSize);
                }
            }

            var url = "images/" + pictureType + "/" + pictureSize + "/";

            url = url + fileName;
            return url;
        }
        private string GetPictureLocalPath(string fileName, PictureType pictureType)
        {
            var imagesDirectoryPath = _webHelper.MapPath("~/Images/" + pictureType + "/");
            var filePath = Path.Combine(imagesDirectoryPath, fileName);
            return filePath;
        }
        public string GetThumbLocalPath(string thumbFileName,PictureType pictureType, PictureSize pictureSize)
        {
            var thumbsDirectoryPath = _webHelper.MapPath("~/Images/" + pictureType + "/"+ pictureSize);
            
                //get the first two letters of the file name
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
            if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
            {
                if (!Directory.Exists(thumbsDirectoryPath))
                {
                    Directory.CreateDirectory(thumbsDirectoryPath);
                }
            }
            
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }
        
        
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceMetadata.API.ViewModels
{
    public class PictureViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Ref to PictureType enum: Avatar, Background...
        /// </summary>
        public string PictureType { get; set; }

        public int OrderNumber { get; set; }

        public string RefType { get; set; }
        public long RefId { get; set; }
    }
}
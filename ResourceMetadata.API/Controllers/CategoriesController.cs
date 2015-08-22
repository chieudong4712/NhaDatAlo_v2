using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ResourceMetadata.API.ViewModels;
using ResourceMetadata.Model;
using ResourceMetadata.Service;
using AutoMapper;
using System.Threading;
using Microsoft.AspNet.Identity;
using System.Web.Http.Filters;
using ResourceMetadata.Model.SearchModels;
using ResourceMetadata.Core.Common;
using ResourceMetadata.Core.Util;
using ResourceMetadata.API.Filters;
using System.Data.Entity.SqlServer;

namespace ResourceMetadata.API.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }


        #region CRUD

        [HttpGet]
        public IHttpActionResult Get(int pageSize, int pageNumber, string sortField)
        {
            int totalCount = 0;
            var page = new Page { PageSize = pageSize, PageNumber = pageNumber };
            var categories = categoryService.GetPaged(pageSize, pageNumber, sortField, ref totalCount);

            IEnumerable<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            Mapper.Map(categories, categoryViewModels);
            PagedCollectionViewModel<CategoryViewModel> viewModel = new PagedCollectionViewModel<CategoryViewModel> { Data = categoryViewModels, TotalCount = totalCount };

            return Ok(viewModel);
        }


        [HttpPost]
        [Route("api/Categories/search")]
        public IHttpActionResult Search(CategorySE se)
        {
            var page = new Page { PageSize = se.PageSize, PageNumber = se.PageNumber};
            var categories = categoryService.Search(se);
            int totalCount = categories.Count();
            categories = categories.OrderByPropertyName(se.SortField, se.SortOrder);
            categories = categories.GetPage<Category>(page);
            

            IEnumerable<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            Mapper.Map(categories, categoryViewModels);
            PagedCollectionViewModel<CategoryViewModel> viewModel = new PagedCollectionViewModel<CategoryViewModel> { Data = categoryViewModels, TotalCount = totalCount };

            return Ok(viewModel);

        }

        [HttpGet]
        [Route("api/categories/getparent")]
        public IHttpActionResult GetParent(long? parentId)
        {
            CategorySE se = new CategorySE();
            se.ParentId = parentId;
            var categories = categoryService.Search(se);
            var t = categories.ToList();
            IEnumerable<ComboItem> items = categories.Select(x => new ComboItem
            {
                Value = SqlFunctions.StringConvert((double)x.Id).Trim(),
                Text = x.Title
            }).ToList();
            return Ok(items);
        }

        [HttpPost]
        [Route("api/categories/order")]
        public IHttpActionResult Order(List<Category> categories)
        {
            for (int i = 0; i < categories.Count; i++)
            {
                var category = categoryService.GetById(categories[i].Id);
                category.OrderNumber = categories[i].OrderNumber;
                categoryService.Update(category);
            }

            return Ok();
        }


        public IHttpActionResult Get(long id)
        {
            Category category = categoryService.GetById(id);
            var viewModel = new CategoryViewModel();
            Mapper.Map(category, viewModel);
            return Ok(viewModel);

        }

        [HttpPost]
        [ValidateModel]
        [OverrideAuthorization]
        public IHttpActionResult Post(CategoryViewModel CategoryViewModel)
        {
            Category category = new Category();
            Mapper.Map(CategoryViewModel, category);

            category = categoryService.Add(category);
            CategoryViewModel.Id = category.Id;

            return Ok(CategoryViewModel);
            
        }

        public IHttpActionResult Put(long id, CategoryViewModel CategoryViewModel)
        {
            CategoryViewModel.Id = id;
            var category = categoryService.GetById(id);
            Mapper.Map(CategoryViewModel, category);
            categoryService.Update(category);
            return Ok(CategoryViewModel);
        }

        public IHttpActionResult Delete(long id)
        {
            categoryService.Delete(id);
            return Ok();
        }

        public IHttpActionResult Delete([FromUri] long[] ids)
        {
            categoryService.Delete(ids);
            return Ok();
        }
        #endregion
    }
}
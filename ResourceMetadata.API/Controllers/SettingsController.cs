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

namespace ResourceMetadata.API.Controllers
{
    public class SettingsController : ApiController
    {
        private readonly ISettingService settingService;

        public SettingsController(ISettingService settingService)
        {
            this.settingService = settingService;
        }


        #region CRUD

        [HttpGet]
        public IHttpActionResult Get(int pageSize, int pageNumber, string sortField)
        {
            int totalCount = 0;
            var page = new Page { PageSize = pageSize, PageNumber = pageNumber };
            var settings = settingService.GetPaged(pageSize, pageNumber, sortField, ref totalCount);

            IEnumerable<SettingViewModel> settingViewModels = new List<SettingViewModel>();
            Mapper.Map(settings, settingViewModels);
            PagedCollectionViewModel<SettingViewModel> viewModel = new PagedCollectionViewModel<SettingViewModel> { Data = settingViewModels, TotalCount = totalCount };

            return Ok(viewModel);
        }


        [HttpPost]
        [Route("api/Settings/search")]
        public IHttpActionResult Search(SettingSE se)
        {
            var page = new Page { PageSize = se.PageSize, PageNumber = se.PageNumber};
            var settings = settingService.Search(se);
            int totalCount = settings.Count();
            settings = settings.OrderByPropertyName(se.SortField, se.SortOrder);
            settings = settings.GetPage<Setting>(page);
            

            IEnumerable<SettingViewModel> settingViewModels = new List<SettingViewModel>();
            Mapper.Map(settings, settingViewModels);
            PagedCollectionViewModel<SettingViewModel> viewModel = new PagedCollectionViewModel<SettingViewModel> { Data = settingViewModels, TotalCount = totalCount };

            return Ok(viewModel);

        }

        [HttpPost]
        [Route("api/Settings/order")]
        public IHttpActionResult Order(List<Setting> settings)
        {
            for (int i = 0; i < settings.Count; i++)
            {
                var setting = settingService.GetById(settings[i].Id);
                setting.OrderNumber = settings[i].OrderNumber;
                settingService.Update(setting);
            }

            return Ok();
        }


        public IHttpActionResult Get(long id)
        {
            Setting setting = settingService.GetById(id);
            var viewModel = new SettingViewModel();
            Mapper.Map(setting, viewModel);
            return Ok(viewModel);

        }

        [HttpPost]
        [ValidateModel]
        [OverrideAuthorization]
        public IHttpActionResult Post(SettingViewModel settingViewModel)
        {
            Setting setting = new Setting();
            Mapper.Map(settingViewModel, setting);

            setting = settingService.Add(setting);
            settingViewModel.Id = setting.Id;

            return Ok(settingViewModel);
            
        }

        public IHttpActionResult Put(long id, SettingViewModel settingViewModel)
        {
            settingViewModel.Id = id;
            var setting = settingService.GetById(id);
            Mapper.Map(settingViewModel, setting);
            settingService.Update(setting);
            return Ok(settingViewModel);
        }

        public IHttpActionResult Delete(long id)
        {
            settingService.Delete(id);
            return Ok();
        }

        public IHttpActionResult Delete([FromUri] long[] ids)
        {
            settingService.Delete(ids);
            return Ok();
        }
        #endregion
    }
}
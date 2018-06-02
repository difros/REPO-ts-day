using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;
using GQService.com.gq.controller;
using GQService.com.gq.paging;
using GQService.com.gq.security;
using GQService.com.gq.service;
using Microsoft.AspNetCore.Mvc;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
    public class Error500Controller : BaseController
    {
        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public IActionResult Index()
        {
            return PartialView();
        }

      
    }
}
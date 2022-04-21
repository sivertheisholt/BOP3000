using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ImagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public ImagesController(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService) : base(mapper)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("customizer_images")]
        public async Task<ActionResult<List<string>>> GetCustomizerImages()
        {
            return await _photoService.GetBackgroundPictures();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Images;
using API.Interfaces;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ImagesController : BaseApiController
    {
        private readonly IPhotoService _photoService;
        public ImagesController(IMapper mapper, IPhotoService photoService) : base(mapper)
        {
            _photoService = photoService;
        }

        [HttpGet("customizer_images")]
        public async Task<ActionResult<List<BackgroundCustomizerDto>>> GetCustomizerImages()
        {
            var images = await _photoService.GetBackgroundPictures();
            var newImages = new List<BackgroundCustomizerDto>();

            foreach (var image in images)
            {
                var keyword = "upload";
                var index = image.IndexOf(keyword);

                var imageDto = new BackgroundCustomizerDto
                {
                    BackgroundUrl = image,
                    IconUrl = image.Insert(index + keyword.Length, "/c_scale,h_140,w_206")
                };
                newImages.Add(imageDto);
            }
            return newImages;
        }
    }
}
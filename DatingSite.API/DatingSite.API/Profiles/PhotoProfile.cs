using AutoMapper;
using DatingSite.API.Dtos;
using DatingSite.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Profiles {
    public class PhotoProfile : Profile {

        public PhotoProfile() {
            CreateMap<Photo, PhotoForDetailsDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }

    }
}

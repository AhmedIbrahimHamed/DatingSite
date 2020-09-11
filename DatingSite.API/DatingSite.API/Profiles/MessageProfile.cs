using AutoMapper;
using DatingSite.API.Dtos;
using DatingSite.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Profiles {
    public class MessageProfile : Profile {

        public MessageProfile() {
            CreateMap<Message, MessageForReturnDto>();
            CreateMap<MessageForCreationDto, Message>();
            CreateMap<Message, MessageForReturnListsDto>()
                .ForMember(destination => destination.SenderPhotoUrl, options => {
                    options.MapFrom(source => source.Sender.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(destination => destination.RecipientPhotoUrl, options => {
                    options.MapFrom(source => source.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url);
                });
        }
    }
}

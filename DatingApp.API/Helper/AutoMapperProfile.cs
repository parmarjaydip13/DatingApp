using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helper {
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile () {

            CreateMap<User, UserForListsDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.MapFrom (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<User, UserForDetaildDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.MapFrom (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<Photo, PhotosForDetaildDto> ();

            CreateMap<Photo, PhotoForReturnDto> ();

            CreateMap<PhotoForCreationDto, Photo> ();

            CreateMap<UserForUpdateDto, User> ();

            CreateMap<UserForRegisterDto, User> ();

            CreateMap<MessagrForCreationDto, Message> ().ReverseMap ();

            CreateMap<Message, MessageToReturnDto> ()
                .ForMember (m => m.SenderPhotoUrl, opt => opt.MapFrom (x => x.Sender.Photos.FirstOrDefault (p => p.IsMain).Url))
                .ForMember (m => m.RecipientPhotoUrl, opt => opt.MapFrom (x => x.Recipient.Photos.FirstOrDefault (p => p.IsMain).Url));

        }
    }
}
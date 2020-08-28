using System.Linq;
using AutoMapper;
using BattleShip.API.ViewModels;
using BattleShip.Models.Entities;

namespace BattleShip.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Coordinate, CoordinateView>()
                .ForMember("HaveShip", opt => opt.MapFrom(c => c.Ship != null));
            this.CreateMap<Move, RecordsView>();
            this.CreateMap<WinnerShip, StatisticsShipViewModel>();
            this.CreateMap<StatisticsRecord, StatisticView>()
                .ForMember("WinnerShipsCount", opt => opt.MapFrom(s => s.WinnerShips.Count()));
        }
    }
}
﻿namespace BattleShip.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using BattleShip.API.Helpers;
    using BattleShip.API.ViewModels;
    using BattleShip.BusinessLogic.Enums;
    using BattleShip.BusinessLogic.Interfaces;
    using BattleShip.Models.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpGet]
        public IActionResult GetStatistics(int page = 1, string name = "", bool onlyIntactShips = false, FilterMoveState filterMoveState = FilterMoveState.All, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            // SortedStatisticsRecord sortedStatisticsRecord = new SortedStatisticsRecord(sortState);
            var records = this.statisticsService.GetStatisticsRecords(name, onlyIntactShips, sortOrder, filterMoveState).ToList();
            var mapperWS = new MapperConfiguration(cfg => cfg.CreateMap<WinnerShip, StatisticsShipViewModel>()).CreateMapper();

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<StatisticsRecord, StatisticView>()
               .ForMember("WinnerShipsCount", opt => opt.MapFrom(s => s.WinnerShips.Count()))
               .ForMember("WinnerShips", opt => opt.MapFrom(s => mapperWS.Map<IEnumerable<WinnerShip>, List<StatisticsShipViewModel>>(s.WinnerShips))))
               .CreateMapper();
            var pagedRecords = PagedList<StatisticsRecord>.Create(records, page, pageSize);
            var statisticsRecords = mapper.Map<IEnumerable<StatisticsRecord>, List<StatisticView>>(pagedRecords);

            this.Response.AddPagination(pagedRecords.CurrentPage, pagedRecords.PageSize, pagedRecords.TotalCount, pagedRecords.TotalPages);

            return this.Ok(new
            {
                totalPages = pagedRecords.TotalCount,
                statisticsRecords,
            });
        }
    }
}

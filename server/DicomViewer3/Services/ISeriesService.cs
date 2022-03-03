﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DicomViewer3.Dtos;
using DicomViewer3.Models;

namespace DicomViewer3.Services
{
    public interface ISeriesService
    {
        Task<Page<SeriesDto>> GetSeriesPaged(long patientId, PageRequestDto request);
        Task<SeriesDto> GetSeriesByPatientAndSeriesId(long patientId, long seriesId);
        Task AddArea(long seriesId, AreaAddRequestDto request);
        Task<IEnumerable<AreaDto>> GetAreas(long seriesId);
    }
}
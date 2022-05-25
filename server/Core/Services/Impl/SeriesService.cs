﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Core.Dtos;
using Core.Entities;
using Core.Helpers;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Core.Services.Impl;

public class SeriesService : ISeriesService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SeriesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Page<SeriesDto>> GetSeriesPaged(long patientId, PageRequestDto request)
    {
        var instances = await _unitOfWork.Series.GetInstancesForPatientId(
            patientId,
            request.PageNumber,
            request.PageSize
        );
            
        return _mapper.Map<Page<SeriesDto>>(instances);
    }

    public async Task<SeriesDto> GetSeriesByPatientAndSeriesId(long patientId, long seriesId)
    {
        var series = await _unitOfWork.Series.GetSeriesByPatientAndSeriesId(patientId, seriesId);
        return _mapper.Map<SeriesDto>(series);
    }

    public async Task<dynamic> GetInstanceMetaForSeries(long seriesId)
    {
        var series = await _unitOfWork.Series.GetById(seriesId);
        var instance = await _unitOfWork.Instances.GetInstanceById(series.Instances.First().Id);
        return instance.DicomMeta;
    }

    public async Task<AreaDto> AddArea(long seriesId, AreaAddRequestDto request)
    {
        var series = await _unitOfWork.Series.GetById(seriesId);
        var area = new Area
        {
            Series = series,
            Label = request.Label,
            Orientation = request.Orientation,
            Slice = request.Slice,
            Vertices = request.Vertices
        };
        await _unitOfWork.Areas.Add(area);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<AreaDto>(area);
    }

    public async Task UpdateAreaLabel(long seriesId, long areaId, AreaUpdateLabelRequestDto request)
    {
        var area = await _unitOfWork.Areas.GetAreaById(areaId);
        area.Label = request.Label;
        await _unitOfWork.CompleteAsync();
    }

    public async Task RemoveArea(long seriesId, long areaId)
    {
        _unitOfWork.Areas.RemoveById(areaId);
       await _unitOfWork.CompleteAsync();
    }

    public async Task<IEnumerable<AreaDto>> GetAreas(long seriesId)
    {
        var areas = await _unitOfWork.Areas.GetAreasBySeriesId(seriesId);
        return _mapper.Map<IList<AreaDto>>(areas);
    }

    public async Task<FileStreamResult> GetSeriesStream(long seriesId)
    {
        var series = await _unitOfWork.Series.GetById(seriesId);
        var sourceStream = File.Open(series.FilePath, FileMode.OpenOrCreate);
        return new FileStreamResult(sourceStream, "application/octet-stream");
    }
}
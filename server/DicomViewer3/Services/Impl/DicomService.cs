﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DicomParser;
using DicomViewer.Helpers;
using DicomViewer3.Dtos;
using DicomViewer3.Entities;
using DicomViewer3.Helpers;
using DicomViewer3.Hubs;
using DicomViewer3.Models;
using DicomViewer3.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace DicomViewer3.Services.Impl
{
    public class DicomService : IDicomService
    {

        private readonly IHubContext<ProgressHub> _progressHub;
        private readonly IUserAccessor _userAccessor;
        private readonly IMongoService _mongoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DicomService(
            IHubContext<ProgressHub> progressHub, 
            IUserAccessor userAccessor, 
            IMongoService mongoService, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _progressHub = progressHub;
            _mongoService = mongoService;
            _userAccessor = userAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Guid RunTaskSaveFiles(long patientId, IEnumerable<IFormFile> files)
        {
            var progressGeneratedId = Guid.NewGuid();
            //SaveFiles(patientId, files, progressGeneratedId)
            return progressGeneratedId;
        }

        public async Task SaveFiles(long patientId, IEnumerable<IFormFile> files, Guid guid)
        {
            var invokerId = _userAccessor.GetUserId();
            //var progressGeneratedId = Guid.NewGuid();
            var formFiles = files as IFormFile[] ?? files.ToArray();
            var filesList = formFiles.ToList();
            for (var i = 0; i < filesList.Count; ++i)
            {
                var file = filesList[i];
                await SaveDicomFile(patientId, file);
                await _progressHub.Clients.Group(invokerId.ToString()).SendAsync(
                    "broadcastprogress",
                    new Progress
                    {
                        Id = guid,
                        CurrentProgress = i + 1,
                        TotalProgress = filesList.Count
                    });
            }
        }
        
        private async Task SaveDicomFile(long patientId, IFormFile file)
        {
            var parser = DicomParse.GetDefaultParser();

            var user = await _unitOfWork.Users.GetById(patientId);
            
            await using var stream = file.OpenReadStream();
            var dicom = parser.Parse(stream);
            
            var studyDate = dicom.Entries[DicomConstats.StudyDate].GetAsDateTime();
            var studyTime = dicom.Entries[DicomConstats.StudyTime].GetAsTimeSpan();
            var studyDateTime = studyDate.SetTime(studyTime);

            var studyOriginalId = dicom.Entries[DicomConstats.StudyId].GetAsString().Trim();
            var study = await _unitOfWork.Studies.GetByOriginalId(studyOriginalId);
            if (study == null)
            {
                study = new Study
                {
                    Date = studyDateTime,
                    Description = dicom.Entries[DicomConstats.StudyDescription].GetAsString().Trim(),
                    OriginalId = studyOriginalId,
                    User = user
                };
                await _unitOfWork.Studies.Add(study);
            }

            var seriesDate = dicom.Entries[DicomConstats.SeriesDate].GetAsDateTime();
            var seriesTime = dicom.Entries[DicomConstats.SeriesTime].GetAsTimeSpan();
            var seriesDateTime = seriesDate.SetTime(seriesTime);
            var seriesOriginalId = dicom.Entries[DicomConstats.SeriesId].GetAsString().Trim();
            var series = await _unitOfWork.Series.GetByOriginalId(seriesOriginalId);
            if (series == null)
            {
                series = new Series
                {
                    Date = seriesDateTime,
                    Description = dicom.Entries[DicomConstats.SeriesDescription].GetAsString().Trim(),
                    OriginalId = seriesOriginalId,
                    Modality = dicom.Entries[DicomConstats.Modality].GetAsString().Trim(),
                    Study = study
                };
                await _unitOfWork.Series.Add(series);
            }

            // override patient id to ours.
            dicom.GetEntryByTag(DicomConstats.PatientId).Value = patientId.ToString();

            var frame = dicom.Entries[DicomConstats.PixelData].GetAsListBytes()[0];
            dicom.Entries.Remove(DicomConstats.PixelData);

            var sliceMongoId = await _mongoService.UploadFile(file.Name, frame, new GridFSUploadOptions
            {
                Metadata = BsonDocument.Parse(JsonSerializer.Serialize(dicom, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new LowercaseJsonNamingPolicy(),
                }))
            });

            var instance = new Instance
            {
                MongoId = sliceMongoId.ToString(),
                OriginalId = dicom.Entries[DicomConstats.InstanceId].GetAsUInt(),
                Series = series
            };

            await _unitOfWork.Instances.Add(instance);
            
            Console.WriteLine($"{patientId}/{study.Id}/{series.Id}");

            await _unitOfWork.CompleteAsync();
        }
        
    }
}
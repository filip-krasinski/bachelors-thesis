﻿using System.Threading.Tasks;
using DicomViewer.Entities.Dtos.Request;
using DicomViewer.Entities.Dtos.Response;
using DicomViewer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicomViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly UserService _userService;

        public AuthenticationController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<SignUpResponseDto> SignUp([FromBody] SignUpRequestDto request)
        {
            return await _userService.SignUp(request);
        }
        
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<SignInResponseDto> SignIn([FromBody] SignInRequestDto request)
        {
            return await _userService.SignIn(request);
        }
        
    }
}
﻿using System.Threading.Tasks;
using DicomViewer3.Dtos;
using DicomViewer3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicomViewer3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
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
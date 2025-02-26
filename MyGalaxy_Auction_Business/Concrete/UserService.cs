using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyGalaxy_Auction_Business.Abstraction;
using MyGalaxy_Auction_Business.Dtos;
using MyGalaxy_Auction_Core.Model;
using MyGalaxy_Auction_Data_Access.Context;
using MyGalaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGalaxy_Auction_Business.Concrete
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ApiResponse _response;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private string _secretKey;
	

		public UserService(ApplicationDbContext context, IMapper mapper, 
		ApiResponse response, UserManager<ApplicationUser> userManager, 
		RoleManager<IdentityRole> roleManager, IConfiguration _configuration)
		{
			_context = context;
			_mapper = mapper;
			_response = response;
			_userManager = userManager;
			_roleManager = roleManager;
			_secretKey = _configuration.GetValue<string>("SecretKey:jwtKey");
		}

		public Task<ApiResponse> Login(LoginRequestDto model)
		{
			throw new NotImplementedException();
		}

		public Task<ApiResponse> Register(RegisterRequestDto model)
		{
			throw new NotImplementedException();
		}
		
	}
}

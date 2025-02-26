using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyGalaxy_Auction_Business.Abstraction;
using MyGalaxy_Auction_Business.Dtos;
using MyGalaxy_Auction_Core.Model;
using MyGalaxy_Auction_Data_Access.Context;
using MyGalaxy_Auction_Data_Access.Enums;
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

		public async Task<ApiResponse> Register(RegisterRequestDto model)
		{
			var userFromDb = _context.ApplicationUsers.FirstOrDefault(x => x.UserName!.ToLower() ==
			model.UserName.ToLower());
			if(userFromDb!=null)
			{
				_response.StatusCode = System.Net.HttpStatusCode.BadRequest;
				_response.isSuccess = false;
				_response.ErrorMessages.Add("Username allready exists");
				return _response;
			}
			var newUser = _mapper.Map<ApplicationUser>(model);
			var result = await _userManager.CreateAsync(newUser, model.Password);
			if(result.Succeeded)
			{
				var isAdminRoleExist = await _roleManager.RoleExistsAsync(UserType.Administrator.ToString());
				if (!isAdminRoleExist)
				{
					await _roleManager.CreateAsync(new IdentityRole(UserType.Administrator.ToString()));
					await _roleManager.CreateAsync(new IdentityRole(UserType.Seller.ToString()));
					await _roleManager.CreateAsync(new IdentityRole(UserType.NormalUser.ToString()));
				}
				var userType = model.UserType.ToString().ToLower();
				if(userType == UserType.Administrator.ToString().ToLower())
				{
					await _userManager.AddToRoleAsync(newUser, UserType.Administrator.ToString());
				}
				if (userType == UserType.Seller.ToString().ToLower())
				{
					await _userManager.AddToRoleAsync(newUser, UserType.Seller.ToString());
				}
				if (userType == UserType.NormalUser.ToString().ToLower())
				{
					await _userManager.AddToRoleAsync(newUser, UserType.NormalUser.ToString());
				}
				_response.StatusCode = System.Net.HttpStatusCode.Created;
				_response.isSuccess = true;
				return _response;
			}
			foreach (var error in result.Errors)
			{
				_response.ErrorMessages.Add(error.ToString()!);
			}
			return _response;
		}
		
	}
}

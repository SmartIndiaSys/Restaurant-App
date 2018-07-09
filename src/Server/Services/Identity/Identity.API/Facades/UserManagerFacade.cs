﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.API.Abstraction;
using Identity.API.Data;
using Identity.API.Model;
using Identity.API.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Facades
{
	public class UserManagerFacade : IUserManagerFacade
	{
		private readonly ApplicatiobDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserManagerFacade(UserManager<ApplicationUser> userManager, ApplicatiobDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public Task<IdentityResult> Create(ApplicationUser applicationUser, string password)
		{
			return _userManager.CreateAsync(applicationUser, password);
		}

		public async Task<ApplicationUser> GetAsync(ClaimsPrincipal principal)
		{
			var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;

			return await _context.Users
				.Include(x => x.UserProfile)
				.SingleOrDefaultAsync(x => x.Id == userId);
		}

		public IEnumerable<ApplicationUser> GetAllUsers()
		{
			return _userManager.Users
				.Include(x => x.UserProfile)
				.ToList();
		}


		public Task<IdentityResult> UpdateAsync(ApplicationUser applicationUser)
		{
			return _userManager.UpdateAsync(applicationUser);
		}
	}
}
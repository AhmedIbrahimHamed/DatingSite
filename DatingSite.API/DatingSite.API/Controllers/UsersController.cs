﻿using AutoMapper;
using DatingSite.API.Data;
using DatingSite.API.Dtos;
using DatingSite.API.Helpers;
using DatingSite.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingSite.API.Controllers {
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper) {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams) {
            var currentUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender)) {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}", Name = "GetUser"),]
        public async Task<IActionResult> GetUser(int id) {
            var user = await _repo.GetUser(id);
            
            if (user == null) {
                return NotFound("User wasn't found!");
            }

            var userToReturn = _mapper.Map<UserForDetailsDto>(user);

            return Ok(userToReturn);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto) {
            if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll()) {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId) {
            if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var like = await _repo.GetLike(id, recipientId);

            if (like != null) {
                return BadRequest("You already like this user");
            }

            if (await _repo.GetUser(recipientId) == null) {
                return NotFound();
            }

            like = new Like() {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll()) {
                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        [HttpDelete("{id}/like/{recipientId}")]
        public async Task<IActionResult> UnlikeUser(int id, int recipientId) {
            if (id != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var like = await _repo.GetLike(id, recipientId);

            if (like == null) {
                return BadRequest("You don't have a like for this user");
            }

            if (await _repo.GetUser(recipientId) == null) {
                return NotFound();
            }

            _repo.Delete(like);

            if (await _repo.SaveAll()) {
                return NoContent();
            }

            return BadRequest("Failed to Unlike user");
        }

    }
}

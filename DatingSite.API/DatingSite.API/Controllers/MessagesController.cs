using AutoMapper;
using DatingSite.API.Data;
using DatingSite.API.Dtos;
using DatingSite.API.Helpers;
using DatingSite.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingSite.API.Controllers {
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper) {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id) {
            if (userId != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var sender = await _repo.GetUser(userId);

            var messageFromRepo = await _repo.GetMessage(id);


            if (messageFromRepo == null) {
                return NotFound();
            }

            var messageForReturn = _mapper.Map<MessageForReturnDto>(messageFromRepo);

            return Ok(messageForReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId,[FromQuery] MessageParams messageParams) {
            if (userId != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

           

            messageParams.UserId = userId;

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messagesToReturn = _mapper.Map<IEnumerable<MessageForReturnListsDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messagesToReturn);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId) {
            if (userId != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageForReturnListsDto>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto) {
            if (userId != Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            messageForCreationDto.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);
            var sender = await _repo.GetUser(userId);

            if (recipient == null) {
                return BadRequest("Couldn't find recipient user");
            }

            var messageEntity = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add(messageEntity);

            if (await _repo.SaveAll()) {
                var messageToReturn = _mapper.Map<MessageForReturnListsDto>(messageEntity);
                return CreatedAtRoute("GetMessage",
                    new { id = messageEntity.Id },
                    messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }

        // For deleting a message, we are adding an HttpPost instead of HttpDelete
        // because we are not actually deleting a message unless both sides of the
        // conversation have chosen to delete the message.  
        [HttpPost("{id}")] // id - is the message ID
        public async Task<IActionResult> DeleteMessage(int id, int userId) {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo == null)
                return BadRequest("Could not find message.");

            if (messageFromRepo.SenderId == userId)
                messageFromRepo.SenderDeleted = true;

            if (messageFromRepo.RecipientId == userId)
                messageFromRepo.RecipientDeleted = true;

            if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
                _repo.Delete(messageFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message.");
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id,[FromBody] JsonPatchDocument<MessageForReadDto> patchDocument) {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                return Unauthorized();
            }

            var messageFromRepo = await _repo.GetMessage(id);

            if (messageFromRepo.RecipientId != userId ) {
                return Unauthorized();
            }

            var messageForUpdateToPatch = _mapper.Map<MessageForReadDto>(messageFromRepo);

            messageForUpdateToPatch.DateRead = DateTime.Now;

            patchDocument.ApplyTo(messageForUpdateToPatch, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            _mapper.Map(messageForUpdateToPatch, messageFromRepo);

            _repo.UpdateMessageIsRead(userId, id);

            if (await _repo.SaveAll()) {
                return NoContent();
            }

            throw new Exception("Error updating the message.");
        }

    }
}

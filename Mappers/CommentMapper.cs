using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beckend.Dtos.Comment;
using beckend.Models;

namespace beckend.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            if (commentModel == null)
                throw new ArgumentNullException(nameof(commentModel));

            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CretedBy = commentModel.AppUser?.UserName ?? "Unknown User",
                StockId = commentModel.StockId
            };
        }
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
            };
        }
    }
}
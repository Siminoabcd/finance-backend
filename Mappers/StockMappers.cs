using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beckend.Dtos.Stock;
using beckend.Dtos.Comment;
using beckend.Models;

namespace beckend.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            if (stockModel == null)
                throw new ArgumentNullException(nameof(stockModel));

            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                // If stockModel.Comments is null, initialize with an empty list
                Comments = stockModel.Comments != null
                    ? stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
                    : new List<CommentDto>()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
    }    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beckend.Extensions;
using beckend.Interfaces;
using beckend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace beckend.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPorfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            return Ok(userPorfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null) return BadRequest("Stock not found");

            var userPorfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if(userPorfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("Cannot add same stock twice");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);
            
            if(portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                return Created();
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPorfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            var filteredStock = userPorfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock is not in your Portfolio");
            }

            return Ok();
        }
    }
}
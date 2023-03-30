using ChatGptExam.Data;
using ChatGptExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatGptExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly DbContextClass _context;

        public ShopController(DbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetShop")]
        public async Task<ActionResult<IEnumerable<ShopSubscription>>> Get()
        {
            var shop = await _context.shopSubscriptions.ToListAsync();
            if (shop == null || shop.Count == 0)
            {
                return NotFound();
            }
            return Ok(shop);
        }

        [HttpPost]
        [Route("AddShop")]
        public IActionResult AddShop([FromBody] ShopSubscription shopController)
        {
            var shop = _context.shopSubscriptions.FirstOrDefault(x => x.SubscriptionName.Equals(shopController.SubscriptionName));
            if (shop == null)
            {
                _context.shopSubscriptions.Add(shopController);
                _context.SaveChanges();
                return Ok();
            }

            return Forbid();
        }

        [HttpPost]
        [Route("UpdateShop")]
        public IActionResult UpdateShop([FromBody] ShopSubscription shopController)
        {
            var shop = _context.shopSubscriptions.FirstOrDefault(x => x.SubscriptionId.Equals(shopController.SubscriptionId));
            if (shop == null)
            {
                return NotFound();
            }
            shop.SubscriptionName = shopController.SubscriptionName;
            shop.SubscriptionImage = shopController.SubscriptionImage;
            shop.SubscriptionPrice = shopController.SubscriptionPrice;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("DeleteShop")]
        public IActionResult DeleteShop([FromQuery] int Id)
        {
            var shop = _context.shopSubscriptions.FirstOrDefault(x => x.SubscriptionId.Equals(Id));

            if (shop == null)
            {
                return NotFound();
            }

            _context.shopSubscriptions.Remove(shop);
            _context.SaveChanges();
            return Ok();
        }
    }
}

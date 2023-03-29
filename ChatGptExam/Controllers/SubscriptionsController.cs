using ChatGptExam.Data;
using ChatGptExam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatGptExam.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly DbContextClass _dbContext;

        public SubscriptionsController(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetSubscriptionsList")]
        public IResult Get()
        {
            return Results.Json(_dbContext.Subscriptions.ToList());
        }

        [HttpPost]
        [Route("CreateSubscriptions")]
        public IActionResult AddSubscriptions(Subscription subscription)
        {
            var item = _dbContext.Subscriptions.FirstOrDefault(x => x.Name.Equals(subscription.Name));

            if(item == null)
            {
                _dbContext.Subscriptions.Add(subscription);
                _dbContext.SaveChanges();
                return Ok();
            }
            return Forbid();
        }

        [HttpPost]
        [Route("EditSubscriptions")]
        public IActionResult EditSubscriptions(Subscription subscription)
        {
            var item = _dbContext.Subscriptions.FirstOrDefault(x => x.Id.Equals(subscription.Id));

            if (item != null)
            {
                item.Name = subscription.Name;
                item.Description = subscription.Description;
                item.Price = subscription.Price;
                _dbContext.SaveChanges();
                return Ok();
            }
            return Forbid();
        }

        [HttpPost]
        [Route("DelSubscriptions")]
        public IActionResult DeleteSubscriptions(Subscription subscription)
        {
            var item = _dbContext.Subscriptions.FirstOrDefault(x => x.Id.Equals(subscription.Id));

            if (item != null)
            {
                _dbContext.Remove(item);
                _dbContext.SaveChanges();
                return Ok();
            }
            return Forbid();
        }
    }
}

﻿namespace ChatGptExam.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}

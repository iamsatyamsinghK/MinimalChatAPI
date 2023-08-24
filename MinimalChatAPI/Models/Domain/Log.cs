﻿namespace MinimalChatAPI.Models.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        //public string Username { get; set; }
       // public string RequestPath { get; set; }
        public string RequestBody { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

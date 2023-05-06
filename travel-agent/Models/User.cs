using System.ComponentModel.DataAnnotations;

namespace travel_agent.Models
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public enum Role { AGENT, PASSENGER }
}

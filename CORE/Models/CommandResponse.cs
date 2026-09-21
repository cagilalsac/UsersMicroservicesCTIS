using CORE.Domain;

namespace CORE.Models
{
    public class CommandResponse : Record
    {
        public bool IsSuccessful { get; } // readonly

        public string Message { get; } // readonly

        public CommandResponse(bool isSuccessful, string message, int id = 0) : base(id)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}

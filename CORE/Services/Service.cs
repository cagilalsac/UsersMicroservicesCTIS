using CORE.Models;
using System.Globalization;

namespace CORE.Services
{
    public abstract class Service
    {
        private string _culture;

        protected string Culture
        {
            get
            {
                return _culture;
            }
            set 
            {
                _culture = value;
                Thread.CurrentThread.CurrentCulture = new CultureInfo(_culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(_culture);
            }
        }

        protected Service()
        {
            Culture = "en-US"; // tr-TR
        }

        protected CommandResponse Success(int id, string message = default)
            => new CommandResponse(true, message, id);

        protected CommandResponse Error(string message)
          => new CommandResponse(false, message);
    }
}

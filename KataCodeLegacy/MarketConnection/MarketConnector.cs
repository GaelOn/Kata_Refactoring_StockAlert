namespace MarketConnection
{
    public class MarketConnector
    {

        private string          _user;
        private string          _password;
        private readonly string _token;

        public MarketConnector(string marketName, string user, string password, string token)
        {
            _user     = user;
            _password = password;
            _token    = token;
        }

        public string GetQueryResult(string query)
        {
            return "17,7";
        }
    }
}

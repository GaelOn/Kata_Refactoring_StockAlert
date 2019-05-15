using System;
using System.Configuration;
using System.Collections;
using System.Threading;
using MarketConnection;

public class StockAlert
{
    private readonly string _user;
    private readonly string _password;
    private readonly string _token;
    private readonly Timer  _timer;
    private readonly string _connectionType;
    private readonly bool   _checkAbove;
    private readonly string _marketName;
    private readonly string _stockname;
    private readonly double _alert;

    public event Action OnAlert;

    public StockAlert(string connectionType, bool checkAbove, string marketName, string stockname, double alert)
    {
        if (connectionType == "MARKET_1")
        {
            var section = (Hashtable)ConfigurationManager.GetSection("MarketConfiguration/SECTION_MARKET_1");
            _user     = (string)section["user"];
            _password = (string)section["password"];
        }
        else if (connectionType == "MARKET_2")
        {
            var section = (Hashtable)ConfigurationManager.GetSection("MarketConfiguration/SECTION_MARKET_2");
            _user     = (string)section["user"];
            _password = (string)section["password"];
            _token    = (string)section["token"];
        }

        _timer          = new Timer(this.CheckStock, null, 0, 2000);
        _connectionType = connectionType;
        _checkAbove     = checkAbove;
        _marketName     = marketName;
        _stockname      = stockname;
        _alert          = alert;
    }

    private void CheckStock(object obj)
    {
        double price = 0.0;
        if (_connectionType == "MARKET_1")
        {
            var query = "from " + _marketName + "get price of " + _stockname;
            price = double.Parse(new MarketConnector(_connectionType, _user, _password, null).GetQueryResult(query));
        }
        else if (_connectionType == "MARKET_2")
        {
            var query = "MKT[" + _marketName + "].PRICE[" + _stockname + "]";
            price = double.Parse(new MarketConnector(_connectionType, _user, _password, _token).GetQueryResult(query));
        }

        if (_checkAbove)
        {
            if (price > _alert)
            {
                OnAlert();
            }
        }
        else
        {
            if (price < _alert)
            {
                OnAlert();
            }
        }
    }

}

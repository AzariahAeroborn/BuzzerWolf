using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BuzzerWolf.BBAPI.Model
{
    public class Economy
    {
        public Economy(XElement economyInfo)
        {
            Retrieved = DateTimeOffset.Parse(economyInfo.Descendants("economy").First().Attribute("retrieved")!.Value);
            LastWeek = new EconomyWeek(economyInfo.Descendants("lastWeek").First());
            ThisWeek = new EconomyWeek(economyInfo.Descendants("thisWeek").First());
        }

        public DateTimeOffset Retrieved { get; init; }
        public EconomyWeek LastWeek { get; init; }
        public EconomyWeek ThisWeek { get; init; }
    }

    public class EconomyWeek
    {
        public EconomyWeek(XElement economyWeekInfo)
        {
            WeekStart = DateTimeOffset.Parse(economyWeekInfo.Attribute("start")!.Value);
            Initial = int.Parse(economyWeekInfo.Descendants("initial").First().Value);
            var final = economyWeekInfo.Descendants("final").FirstOrDefault();
            Final = final != null ? int.Parse(final.Value) : null;

            foreach (var transactionElement in economyWeekInfo.Elements().Where(e => e.Name != "initial" && e.Name != "final" && e.Name != "current"))
            {
                Transactions.Add(new Transaction(transactionElement, WeekStart));
            }
        }

        public DateTimeOffset WeekStart { get; init; }
        public int Initial { get; init; }
        public List<Transaction> Transactions { get; } = new();
        public int? Final { get; init; }
    }

    public class Transaction
    {
        public Transaction(XElement transactionInfo, DateTimeOffset weekStart)
        {
            switch (transactionInfo.Name.ToString())
            {
                case "playerSalaries":
                    Type = TransactionType.PlayerSalaries;
                    break;
                case "staffSalaries":
                    Type = TransactionType.StaffSalaries;
                    break;
                case "scouting":
                    Type = TransactionType.Scouting;
                    break;
                case "matchRevenue":
                    Type = TransactionType.MatchRevenue;
                    break;
                case "merchandise":
                    Type = TransactionType.Merchandise;
                    break;
                case "tvMoney":
                    Type = TransactionType.TvMoney;
                    break;
                case "arenaExpansion":
                    Type = TransactionType.ArenaExpansion;
                    break;
                case "unknown":
                    DateTimeOffset transactionDate;
                    if (DateTimeOffset.TryParse(transactionInfo.Attribute("date")?.Value, out transactionDate) && transactionDate == weekStart)
                    {
                        Type = TransactionType.InfrastructureMaintenance;
                    }
                    else
                    {
                        Type = TransactionType.Other;
                    }
                    break;
                default:
                    Type = TransactionType.Other;
                    break;
            };

            Value = int.Parse(transactionInfo.Value);
        }

        public TransactionType Type { get; init; }
        public int Value { get; init; }
    }

    public enum TransactionType
    {
        PlayerSalaries,
        StaffSalaries,
        Scouting,
        InfrastructureMaintenance,
        MatchRevenue,
        Merchandise,
        TvMoney,
        ArenaExpansion,
        InfrastructureBuild,
        Other
    }
}

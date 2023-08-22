using Boro.EntityFramework.DbContexts.BoroMainDb.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Models.Output;

namespace UserService.DB.Extensions
{
    internal static class ScoreboardsExtensions
    {
        public static UserStatistics ToUserStatistics(this Scoreboards stats)
        {
            return new UserStatistics()
            {
                UserId = stats.UserId,
                AmountOfBorrowings = stats.AmountOfBorrowings,
                AmountOfItems = stats.AmountOfItems,
                AmountOfLendings = stats.AmountOfLendings,
                TotalScore = stats.TotalScore,
            };
        }
    }
}

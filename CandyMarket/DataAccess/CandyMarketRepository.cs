using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandyMarket.Models;
using Dapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.SqlClient;

namespace CandyMarket.DataAccess
{
    public class CandyMarketRepository
    {
        const string ConnectionString = "Server=localhost;Database=CandyMarket;Trusted_Connection=True;";
        public UserWithCandyInfo GetUserWithCandyInfo(int userId)
        {

            var sql = @"select [User].FirstName + ' ' + [User].LastName as [Name], [User].UserId
	                        from [User]
                        where UserId = @userId";

            var candy = @"SELECT [Name] as CandyType, Candy.CandyId
                        FROM Candy
	                        Join UserCandy
	                        ON Candy.CandyId = UserCandy.CandyId
                        WHERE UserCandy.UserId = @userId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var user = db.QueryFirstOrDefault<UserWithCandyInfo>(sql, new { UserId = userId });
                var candies = db.Query<Candy>(candy, new { UserId = userId });

                user.Candy = candies;

                return user;
            }
        }

        public IEnumerable<CandyWithAllInfo> GetAllCandies()
        {
            var sql = @"SELECT * FROM Candy";

            using (var db = new SqlConnection(ConnectionString))
            {
                var candy = db.Query<CandyWithAllInfo>(sql);

                return candy;
            }
        }

        public UserWithCandyInfo Eat(int userId, int candyId)
        {
            var sql = @"DELETE FROM UserCandy WHERE UserId = @userId AND CandyId = @candyId;";

            using(var db = new SqlConnection(ConnectionString))
            {
                db.ExecuteAsync(sql, new { UserId = userId, CandyId = candyId });

                var updatedUser = GetUserWithCandyInfo(userId);
                return updatedUser;
            }
        }

        public UserWithCandyInfo AddCandies(int candyId, int userId)
        {
            var sql = @$"insert into UserCandy(UserId, CandyId, DateAdded)
                         values (@userId, @candyId, '{DateTime.Now}')";

            using (var db = new SqlConnection(ConnectionString))
            {
                var result = db.QueryFirstOrDefault<UserCandy>(sql, new { UserId = userId, CandyId = candyId });

                var updatedUser = GetUserWithCandyInfo(userId);
                return updatedUser;
            }
        }
    }
}

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

            var candy = @"SELECT [Name] as CandyType, Candy.CandyId, UserCandy.DateAdded
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
            var candyToDelete = EatOldCandy(userId, candyId);

            var sql = @$"DELETE FROM UserCandy WHERE UserCandy.UserCandyId = {candyToDelete.UserCandyId};";

            using(var db = new SqlConnection(ConnectionString))
            {
                db.ExecuteAsync(sql);

                var updatedUser = GetUserWithCandyInfo(userId);
                return updatedUser;
            }
        }

        public UserWithOldestCandy EatOldCandy(int userId, int candyId) 
        {
            var sql = @"SELECT TOP(1) DateAdded, [Name] as CandyType, Candy.CandyId, UserCandy.UserCandyId
                        FROM Candy
	                        Join UserCandy
	                        ON Candy.CandyId = UserCandy.CandyId
                        WHERE UserCandy.UserId = @userId AND UserCandy.CandyId = @candyId
                        ORDER BY DateAdded";

            using (var db = new SqlConnection(ConnectionString))
            {
                var candy = db.QueryFirstOrDefault<UserWithOldestCandy>(sql, new { UserId = userId, CandyId = candyId });

                return candy;
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

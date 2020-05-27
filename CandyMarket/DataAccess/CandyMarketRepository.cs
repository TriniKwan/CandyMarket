using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandyMarket.Models;
using Dapper;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CandyMarket.DataAccess
{
    public class CandyMarketRepository
    {
        string ConnectionString;

        public CandyMarketRepository(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("CandyMarket");
        }

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

        public IEnumerable<UserWithCandyInfo> GetAllUsersWithCandy()
        {
            var sql = @"select [User].FirstName + ' ' + [User].LastName as [Name], [User].UserId
	                        from [User]";

            var candy = @"SELECT [Name] as CandyType, Candy.CandyId, UserCandy.DateAdded
                        FROM Candy
	                        Join UserCandy
	                        ON Candy.CandyId = UserCandy.CandyId
                        WHERE UserCandy.UserId = @userId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var users = db.Query<UserWithCandyInfo>(sql);

                foreach (var user in users)
                {
                    var candies = db.Query<Candy>(candy, new { UserId = user.UserId });

                    user.Candy = candies;

                }

                return users;
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

        public UserWithCandyInfo EatFlavoredCandy(int userId, string type)
        {
            var sql = @"SELECT TOP(1) [Name] as CandyType, Candy.CandyId, Category.[Type], Category.CategoryId
	                    FROM Candy
		                    JOIN UserCandy
		                    ON Candy.CandyId = UserCandy.CandyId
		                    JOIN Category
		                    ON Candy.CategoryId = Category.CategoryId
		                    WHERE UserCandy.UserId = @userId AND Category.[Type] = @type
                        ORDER BY NEWID()";

            using (var db = new SqlConnection(ConnectionString))
            {
                var candyType = db.QueryFirstOrDefault<CandyWithCategory>(sql, new { UserId = userId, Type = type });

                return Eat(userId, candyType.CandyId);
            }
        }

        public List<UserWithCandyInfo> TradeCandy(int oldUserId, int userId, int candyId)
        {
            var users = new List<UserWithCandyInfo>();
            var sql = @"UPDATE top(1) UserCandy
                        SET UserId = @userId
                        WHERE UserCandy.CandyId = @candyId AND UserCandy.UserId = @oldUserId";

            using (var db = new SqlConnection(ConnectionString))
            {
                db.ExecuteAsync(sql, new { OldUserId = oldUserId, UserId = userId, CandyId = candyId});

                var updatedUser = GetUserWithCandyInfo(userId);
                users.Add(updatedUser);

                var oldUser = GetUserWithCandyInfo(oldUserId);
                users.Add(oldUser);
            }

            return users;
        }
    }
}

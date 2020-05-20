using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CandyMarket.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CandyMarket.DataAccess
{
    public class CandyMarketRepository
    {
        const string ConnectionString = "Server=localhost;Database=CandyMarket;Trusted_Connection=True;";
        public IEnumerable<UserWithCandyInfo> GetUserWithCandyInfo(int userId)
        {
            //var sql = @"select Candy.[Name] as CandyType, [User].UserId, [User].FirstName + ' ' + [User].LastName as [Name]
            //            from UserCandy
            //             join Candy
            //              on UserCandy.CandyId = Candy.CandyId
            //             join [User]
            //              on UserCandy.UserId = [User].UserId
            //            where UserCandy.UserId = @userId";

            var sql = @"select [User].FirstName + ' ' + [User].LastName as [Name], UserCandy.UserId, UserCandy.CandyId
                        from UserCandy
	                        join [User]
		                        on UserCandy.UserId = [User].UserId
                        where UserCandy.UserId = @userId";

            var candy = @"select [Name] as CandyType, Candy.CandyId
                        from Candy";

            //var owner = @"select [User].FirstName + ' ' + [User].LastName as [Name]
            //            from [User]
            //            where [User].UserId = @userId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var users = db.Query<UserWithCandyInfo>(sql, new { UserId = userId });
                var candies = db.Query<Candy>(candy);
                //var candyOwner = db.QueryFirstOrDefault<User>(owner, new { UserId = userId });

                foreach (var user in users)
                {
                    user.Candy = candies.Where(c => c.CandyId == user.CandyId).Select(c => c.CandyType);
                }

                return users;
            }
        }
    }
}

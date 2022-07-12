using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CoffeeShop.Repositories
{
    public class CoffeeRepository : ICoffeeRepository
    {
        private readonly string _connectionString;

        public CoffeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public void Add(Coffee coffee)
        {
            using (var conn = Connection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Coffee (Id, Title, BeanVarietyId)
                                        OUTPUT INSERTED.ID
                                        VALUES( @title, @bvid)";

                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@bvid", coffee.BeanVarietyId);

                    coffee.Id = (int)cmd.ExecuteScalar();

                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE From Coffee WHERE Id=@id";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Coffee Get(int id)
        {
            using (var conn = Connection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id as coffeeId, 
                                               c.Title,
                                               c.BeanVarietyId,
                                               b.[Name],
                                               b.Id as beanId,
                                               b.Region,
                                               b.Notes
                                          FROM Coffee c 
                                     LEFT JOIN BeanVariety b on b.Id=c.Id
                                    WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        var beanVariety = new BeanVariety()
                        { 
                            Id=reader.GetInt32(reader.GetOrdinal("beanId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Region = reader.GetString(reader.GetOrdinal("Region")),

                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                        {
                            beanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }

                        var coffee = new Coffee()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("coffeeId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                            BeanVariety = beanVariety
                        };

                        return coffee;
                    }
                    return null;
                }
            }
        }

        public List<Coffee> GetAll()
        {
            using (var conn = Connection)
            { 
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id as coffeId, 
                                               c.Title,
                                               c.BeanVarietyId,
                                               b.[Name],
                                               b.Id as beanId,
                                               b.Region,
                                               b.Notes
                                          FROM Coffee c 
                                     LEFT JOIN BeanVariety b on b.Id=c.Id;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        var coffees = new List<Coffee>();
                        while (reader.Read())
                        {
                            var beanVariety = new BeanVariety()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("beanId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Region = reader.GetString(reader.GetOrdinal("Region")),

                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                beanVariety.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }

                            var coffee = new Coffee()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("coffeId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BeanVarietyId = reader.GetInt32(reader.GetOrdinal("BeanVarietyId")),
                                BeanVariety = beanVariety
                            };
                            coffees.Add(coffee);
                        }
                        return coffees;
                    }
                }
            }
        }

        public void Update(Coffee coffee)
        {
            using (var conn = Connection)
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Coffee
                                        SET Title=@title, BeanVarietyId=@bvid
                                        WHERE Id=@id";

                    cmd.Parameters.AddWithValue("@title", coffee.Title);
                    cmd.Parameters.AddWithValue("@bvid", coffee.BeanVarietyId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

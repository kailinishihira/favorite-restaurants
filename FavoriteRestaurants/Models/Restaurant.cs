using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace FavoriteRestaurants.Models
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private string _location;
    private string _hours;
    private int _cuisineId;

    public Restaurant(string name, string location, string hours, int cuisineId, int id=0)
    {
      _id = id;
      _name = name;
      _location = location;
      _hours = hours;
      _cuisineId = cuisineId;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetLocation()
    {
      return _location;
    }

    public string GetHours()
    {
      return _hours;
    }

    public int GetCuisineId()
    {
      return _cuisineId;
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText= @"SELECT * FROM restaurants ORDER BY name ASC;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string location = rdr.GetString(2);
        string hours = rdr.GetString(3);
        int cuisineId = rdr.GetInt32(4);
        Restaurant newRestaurant = new Restaurant(name,location,hours,cuisineId,id);
        allRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRestaurants;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO restaurants(name, location, hours, cuisine_id) VALUES(@name, @location, @hours, @cuisine_id);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter location = new MySqlParameter();
      location.ParameterName = "@location";
      location.Value = this._location;
      cmd.Parameters.Add(location);

      MySqlParameter hours = new MySqlParameter();
      hours.ParameterName = "@hours";
      hours.Value = this._hours;
      cmd.Parameters.Add(hours);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisine_id";
      cuisineId.Value = this._cuisineId;
      cmd.Parameters.Add(cuisineId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherEntry)
    {
      if(!(otherEntry is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newEntry = (Restaurant) otherEntry;
        bool idEquality = this.GetId() == newEntry.GetId();
        bool nameEquality = this.GetName() == newEntry.GetName();
        bool locationEquality = this.GetLocation() == newEntry.GetLocation();
        bool hoursEquality = this.GetHours() == newEntry.GetHours();
        bool cuisineEquality = this.GetCuisineId() == newEntry.GetCuisineId();
        return (idEquality && nameEquality && locationEquality && hoursEquality && cuisineEquality);
      }
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int restaurantId = 0;
      string name = "";
      string location = "";
      string hours = "";
      int cuisineId = 0;

      while (rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        location = rdr.GetString(2);
        hours = rdr.GetString(3);
        cuisineId = rdr.GetInt32(4);
      }
      Restaurant foundRestaurant = new Restaurant(name,location,hours,cuisineId,id);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRestaurant;
    }

  }
}

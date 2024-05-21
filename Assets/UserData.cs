using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

public class UserData : MonoBehaviour
{
    public ObjectId _id { set; get; }
    public string ID { set; get; }
    public string Password { set; get; }
    public int Guide_checksum { set; get; }
    public double Current_position_x { set; get; }
    public double Current_position_y { set; get; }
    public int Destination_ID { set; get; }
    public int Manager_check { set; get; }
}

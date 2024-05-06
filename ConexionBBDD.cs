using System;
using System.Data.Common;
using AppNotes.Models;
using SQLite;

public class ConexionBBDD
{
    public readonly SQLiteConnection Conn;

    public ConexionBBDD()
    {
        Conn = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "AppNotes.db"));
        //Conn.CreateTable<Usuario>();
        Conn.CreateTable<Note>();
    }
    public SQLiteConnection GetConnection()
    {
        return Conn;
    }

    public void CloseConnection()
    {
        Conn.Close();
    }

    public void Vaciar()
    {
        //Conn.DeleteAll<Usuario>();
        Conn.DeleteAll<Note>();
    }
}
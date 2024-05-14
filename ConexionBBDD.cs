using System;
using System.Data.Common;
using System.Diagnostics;
using AppNotes.Models;
using Microsoft.Maui.ApplicationModel.Communication;
using SQLite;

public class ConexionBBDD
{
    public readonly SQLiteConnection Conn;

    public ConexionBBDD()
    {
        Conn = new SQLiteConnection(Path.Combine(FileSystem.AppDataDirectory, "AppNotes.db"));
        Conn.CreateTable<Note>();
        Conn.CreateTable<Notebook>();
        Conn.CreateTable<DeleteQueue>();
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
        Conn.DeleteAll<Note>();
        Conn.DeleteAll<Notebook>();
        Conn.DeleteAll<DeleteQueue>();
    }

    public List<Note> GetNotes()
    {
        List<Note> notes = new List<Note>();
        var query = Conn.CreateCommand("select * from Note").ExecuteDeferredQuery<Note>();
        foreach (var note in query)
        {
            notes.Add(note);
        }
        return notes;
    }
    public List<Note> GetNotesByUser(string id)
    {
        List<Note> notes = new List<Note>();
        var query = Conn.CreateCommand($"SELECT * FROM Note WHERE User = '{id}'").ExecuteDeferredQuery<Note>();
        foreach (var note in query)
        {
            notes.Add(note);
        }
        return notes;
    }
    public List<Notebook> GetNotebooks()
    {
        List<Notebook> notebooks = new List<Notebook>();
        var query = Conn.CreateCommand("select * from Notebook").ExecuteDeferredQuery<Notebook>();
        foreach (var notebook in query)
        {
            notebooks.Add(notebook);
        }
        return notebooks;
    }
    public List<Notebook> GetNotebooksByUser(string id)
    {
        List<Notebook> notebooks = new List<Notebook>();
        var query = Conn.CreateCommand($"SELECT * FROM Notebook WHERE User = '{id}'").ExecuteDeferredQuery<Notebook>();
        foreach (var notebook in query)
        {
            notebooks.Add(notebook);
        }
        return notebooks;
    }
    public List<DeleteQueue> GetDeleteQueue()
    {
        List<DeleteQueue> queue = new List<DeleteQueue>();
        var query = Conn.CreateCommand("select * from DeleteQueue").ExecuteDeferredQuery<DeleteQueue>();
        foreach (var item in query)
        {
            queue.Add(item);
        }
        return queue;
    }
}
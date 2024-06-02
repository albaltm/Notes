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
        Conn.CreateTable<CreateQueue>();
        Conn.CreateTable<DeleteQueue>();
        Conn.CreateTable<Event>();
        Conn.CreateTable<ToDoItem>();
        Conn.CreateTable<SubToDo>();
        Conn.CreateTable<Routine>();
        Conn.CreateTable<RoutineRegistry>();
        //Conn.DeleteAll<RoutineRegistry>();
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
        Conn.DeleteAll<CreateQueue>();
        Conn.DeleteAll<DeleteQueue>();
        Conn.DeleteAll<Event>();
        Conn.DeleteAll<ToDoItem>();
        Conn.DeleteAll<SubToDo>();
        Conn.DeleteAll<Routine>();
        Conn.DeleteAll<RoutineRegistry>();
    }

    public Note GetNote(string id)
    {
        var note = Conn.CreateCommand($"select * from Note WHERE id = '{id}'").ExecuteDeferredQuery<Note>().FirstOrDefault();
        return note;
    }
    public List<Note> GetNotes()
    {
        List<Note> notes = new List<Note>();
        var query = Conn.CreateCommand("select * from Note").ExecuteDeferredQuery<Note>().ToList();
        foreach (var note in query)
        {
            notes.Add(note);
        }
        return notes;
    }

    public Notebook GetNotebook(string id)
    {
        var notebook = Conn.CreateCommand($"select * from Notebook WHERE id = '{id}'").ExecuteDeferredQuery<Notebook>().FirstOrDefault();
        return notebook;
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
    public Event GetEvent(string id)
    {
        var evento = Conn.CreateCommand($"select * from event WHERE id = '{id}'").ExecuteDeferredQuery<Event>().FirstOrDefault();
        return evento;
    }
    public List<Event> GetEvents()
    {
        List<Event> events = new List<Event>();
        var query = Conn.CreateCommand("select * from event").ExecuteDeferredQuery<Event>().ToList();
        foreach (var evento in query)
        {
            events.Add(evento);
        }
        return events;
    }
    public ToDoItem GetToDo(string id)
    {
        var toDo = Conn.CreateCommand($"select * from todoitem WHERE id = '{id}'").ExecuteDeferredQuery<ToDoItem>().FirstOrDefault();
        return toDo;
    }
    public List<ToDoItem> GetToDos()
    {
        List<ToDoItem> toDos = new List<ToDoItem>();
        var query = Conn.CreateCommand("select * from todoitem").ExecuteDeferredQuery<ToDoItem>().ToList();
        foreach (var toDo in query)
        {
            toDos.Add(toDo);
        }
        return toDos;
    }
    public SubToDo GetSubToDo(string id)
    {
        var subToDo = Conn.CreateCommand($"select * from subtodo WHERE id = '{id}'").ExecuteDeferredQuery<SubToDo>().FirstOrDefault();
        return subToDo;
    }
    public List<SubToDo> GetSubToDos()
    {
        List<SubToDo> subToDos = new List<SubToDo>();
        var query = Conn.CreateCommand("select * from subtodo").ExecuteDeferredQuery<SubToDo>().ToList();
        foreach (var subToDo in query)
        {
            subToDos.Add(subToDo);
        }
        return subToDos;
    }
    public Routine GetRoutine(string id)
    {
        var routine = Conn.CreateCommand($"select * from routine WHERE id = '{id}'").ExecuteDeferredQuery<Routine>().FirstOrDefault();
        return routine;
    }
    public List<Routine> GetRoutines()
    {
        List<Routine> routines = new List<Routine>();
        var query = Conn.CreateCommand("select * from routine").ExecuteDeferredQuery<Routine>().ToList();
        foreach (var routine in query)
        {
            routines.Add(routine);
        }
        return routines;
    }
    public RoutineRegistry GetRoutineRegistry(string id)
    {
        var routineregistry = Conn.CreateCommand($"select * from routineregistry WHERE id = '{id}'").ExecuteDeferredQuery<RoutineRegistry>().FirstOrDefault();
        return routineregistry;
    }
    public List<RoutineRegistry> GetRoutinesRegistry()
    {
        List<RoutineRegistry> routines = new List<RoutineRegistry>();
        var query = Conn.CreateCommand("select * from routineregistry").ExecuteDeferredQuery<RoutineRegistry>().ToList();
        foreach (var registry in query)
        {
            routines.Add(registry);
        }
        return routines;
    }
    public List<CreateQueue> GetCreateQueue()
    {
        List<CreateQueue> queue = new List<CreateQueue>();
        var query = Conn.CreateCommand("select * from CreateQueue").ExecuteDeferredQuery<CreateQueue>();
        foreach (var item in query)
        {
            queue.Add(item);
        }
        return queue;
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
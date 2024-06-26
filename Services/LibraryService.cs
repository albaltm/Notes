﻿using AppNotes.Models;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text;
using System.Threading.Tasks;

namespace AppNotes.Services
{
    public class LibraryService(ConexionBBDD _conn)
    {
        public async Task<string> CreateNote(string user, string FirebasePath, Note note)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var notefirebase = await client.Child("note").PostAsync(note);
                    note.Id = notefirebase.Key;
                    note.User = user;
                    await client.Child("note").Child(notefirebase.Key).PutAsync(note);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = note.Id,
                        Type = DocumentType.Note,
                        User = user,
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(note);
            return note.Id;
        }
        public async Task<string> CreateNotebook(string user, string FirebasePath, Notebook notebook)
        {
            if (!user.Equals("guest"))
            {
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    var notefirebookbase = await client.Child("notebook").PostAsync(notebook);
                    notebook.Id = notefirebookbase.Key;
                    notebook.User = user;
                    await client.Child("notebook").Child(notefirebookbase.Key).PutAsync(notebook);
                    client.Dispose();
                }
                catch
                {
                    var createQueue = new CreateQueue()
                    {
                        Id = notebook.Id,
                        Type = DocumentType.Notebook,
                        User = user,
                    };
                    _conn.Conn.Insert(createQueue);
                }
            }
            _conn.GetConnection().Insert(notebook);
            return notebook.Id;
        }
        public async Task DeleteNote(string user, string FirebasePath, Note note)
        {
            var notesinnotebook = _conn.GetNotes(user).Where(x => x.Notebook.Equals(note.Notebook) && !x.Id.Equals(note.Id)).OrderBy(x => x.Position).ToList();
            var num = 0;
            foreach (var notetoposition in notesinnotebook)
            {
                if (notetoposition.Position != num)
                {
                    notetoposition.Position = num;
                    notetoposition.Modified = DateTime.UtcNow;
                    _conn.Conn.Update(notetoposition);
                }
                num++;
            }

            if (!string.IsNullOrEmpty(note.Notebook))
            {
                var notebook = _conn.GetNotebook(note.Notebook);
                if (notebook.Bookmark.Equals(note.Id))
                {
                    notebook.Bookmark = "";
                    _conn.Conn.Update(notebook);
                }
            }

            if (!user.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Note && x.Id.Equals(note.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("note").Child(note.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = note.Id,
                        Type = DocumentType.Note,
                        User = user,
                    };
                    _conn.GetConnection().Insert(item);
                }
            }
            _conn.GetConnection().Delete(note);
        }
        public async Task DeleteNotebook(string user, string FirebasePath, Notebook notebook)
        {
            var notes = _conn.GetNotes(user).Where(x => x.Notebook.Equals(notebook.Id)).ToList();
            foreach (var note in notes)
            {
                note.Modified = DateTime.UtcNow;
                note.Notebook = "";
                note.Position = -1;
                _conn.GetConnection().Update(note);
            }

            if (!user.Equals("guest"))
            {
                var createqueueitem = _conn.GetCreateQueue().Where(x => x.Type == DocumentType.Notebook && x.Id.Equals(notebook.Id)).FirstOrDefault();
                if (createqueueitem != null)
                {
                    _conn.Conn.Delete(createqueueitem);
                }
                try
                {
                    FirebaseClient client = new FirebaseClient(FirebasePath);
                    await client.Child("notebook").Child(notebook.Id).DeleteAsync();
                    client.Dispose();
                }
                catch
                {
                    DeleteQueue item = new DeleteQueue()
                    {
                        Id = notebook.Id,
                        Type = DocumentType.Notebook,
                        User = user,
                    };
                    _conn.GetConnection().Insert(item);
                }
            }
            _conn.GetConnection().Delete(notebook);
        }

    }
}
